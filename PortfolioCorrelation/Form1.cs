using System.Collections.Concurrent;
using System.IO.Pipes;
using System.Text;

namespace PortfolioCorrelation
{
    public partial class Form1 : Form
    {
        // ══════════════════════════════════════════════
        //  DATA STRUCTURES
        // ══════════════════════════════════════════════

        private readonly ConcurrentDictionary<string, AssetPriceHistory> _assets = new();
        private double[,]? _correlationMatrix;
        private string[] _symbolOrder = Array.Empty<string>();
        private readonly object _calcLock = new();
        private CancellationTokenSource? _pipeCts;

        // ── Color Constants ──
        private static readonly Color CHighPos = Color.FromArgb(255, 42, 84);     // #FF2A54
        private static readonly Color CHighNeg = Color.FromArgb(255, 159, 0);      // #FF9F00
        private static readonly Color CSlate = Color.FromArgb(30, 41, 75);         // #1E294B
        private static readonly Color CDiagonal = Color.FromArgb(20, 28, 52);
        private static readonly Color CCyan = Color.FromArgb(0, 210, 255);

        // ══════════════════════════════════════════════
        //  ASSET PRICE HISTORY
        // ══════════════════════════════════════════════

        private sealed class AssetPriceHistory
        {
            private readonly object _lock = new();
            private readonly List<double> _prices = new();

            public void SetPrices(IEnumerable<double> prices)
            {
                lock (_lock)
                {
                    _prices.Clear();
                    _prices.AddRange(prices);
                }
            }

            public List<double> GetSnapshot(int maxCount)
            {
                lock (_lock)
                {
                    if (_prices.Count <= maxCount)
                        return new List<double>(_prices);
                    return _prices.GetRange(_prices.Count - maxCount, maxCount);
                }
            }

            public int Count
            {
                get { lock (_lock) { return _prices.Count; } }
            }
        }

        // ══════════════════════════════════════════════
        //  CONSTRUCTOR
        // ══════════════════════════════════════════════

        public Form1()
        {
            InitializeComponent();

            // Enable double-buffering on the DataGridView for flicker-free rendering
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, dgvMatrix, new object[] { true });

            Load += Form1_Load;
            FormClosing += Form1_FormClosing;
        }

        // ══════════════════════════════════════════════
        //  LIFECYCLE
        // ══════════════════════════════════════════════

        private void Form1_Load(object? sender, EventArgs e)
        {
            LogInfo("Mathematical correlation engine initialized.");
            LogInfo("Awaiting price data on pipe: \\\\.\\pipe\\AUTOSCRIPTS_RISK_MATRIX");
            StartPipeServer();
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _pipeCts?.Cancel();
        }

        // ══════════════════════════════════════════════
        //  NAMED PIPE SERVER (async)
        // ══════════════════════════════════════════════

        private void StartPipeServer()
        {
            _pipeCts = new CancellationTokenSource();
            var ct = _pipeCts.Token;

            Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        using var pipe = new NamedPipeServerStream(
                            "AUTOSCRIPTS_RISK_MATRIX",
                            PipeDirection.In,
                            NamedPipeServerStream.MaxAllowedServerInstances,
                            PipeTransmissionMode.Byte,
                            PipeOptions.Asynchronous);

                        UiInvoke(() => lblEngineValue.Text = "● LISTENING");

                        await pipe.WaitForConnectionAsync(ct);

                        UiInvoke(() => lblEngineValue.Text = "● ACTIVE");

                        using var reader = new StreamReader(pipe, Encoding.UTF8);
                        while (pipe.IsConnected && !ct.IsCancellationRequested)
                        {
                            var line = await reader.ReadLineAsync(ct);
                            if (line == null) break;
                            ProcessPipeMessage(line.Trim());
                        }
                    }
                    catch (OperationCanceledException) { break; }
                    catch (Exception ex)
                    {
                        UiInvoke(() => LogWarning($"Pipe error: {ex.Message}"));
                        await Task.Delay(1000, ct).ConfigureAwait(false);
                    }
                }
            }, ct);
        }

        private void ProcessPipeMessage(string message)
        {
            // Format: PRICES;SYMBOL;p1,p2,p3,...
            if (string.IsNullOrEmpty(message)) return;

            var parts = message.Split(';');
            if (parts.Length < 3 || !parts[0].Equals("PRICES", StringComparison.OrdinalIgnoreCase))
                return;

            var symbol = parts[1].Trim().ToUpperInvariant();
            var priceTokens = parts[2].Split(',', StringSplitOptions.RemoveEmptyEntries);

            var prices = new List<double>(priceTokens.Length);
            foreach (var tok in priceTokens)
            {
                if (double.TryParse(tok.Trim(), System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out double val))
                {
                    prices.Add(val);
                }
            }

            if (prices.Count < 2) return;

            var history = _assets.GetOrAdd(symbol, _ => new AssetPriceHistory());
            history.SetPrices(prices);

            UiInvoke(() => LogInfo($"Received {prices.Count} prices for {symbol}"));

            // Trigger recalculation on a background thread
            Task.Run(RecalculateAndRender);
        }

        // ══════════════════════════════════════════════
        //  PEARSON CORRELATION ENGINE
        // ══════════════════════════════════════════════

        private static double CalculatePearsonCorrelation(List<double> assetA, List<double> assetB)
        {
            int n = Math.Min(assetA.Count, assetB.Count);
            if (n < 2) return 0.0;

            double sumA = 0, sumB = 0;
            for (int i = 0; i < n; i++)
            {
                sumA += assetA[i];
                sumB += assetB[i];
            }

            double meanA = sumA / n;
            double meanB = sumB / n;

            double covariance = 0;
            double varA = 0;
            double varB = 0;

            for (int i = 0; i < n; i++)
            {
                double dA = assetA[i] - meanA;
                double dB = assetB[i] - meanB;
                covariance += dA * dB;
                varA += dA * dA;
                varB += dB * dB;
            }

            double denominator = Math.Sqrt(varA * varB);
            if (denominator < 1e-15) return 0.0;

            double r = covariance / denominator;

            // Clamp to [-1, +1] to guard against floating-point drift
            return Math.Clamp(r, -1.0, 1.0);
        }

        // ══════════════════════════════════════════════
        //  MATRIX RECALCULATION
        // ══════════════════════════════════════════════

        private int GetLookbackCount()
        {
            int lookback = 100;
            if (InvokeRequired)
            {
                Invoke(() => lookback = ParseLookback());
            }
            else
            {
                lookback = ParseLookback();
            }
            return lookback;
        }

        private int ParseLookback()
        {
            return cboLookback.SelectedIndex switch
            {
                0 => 50,
                2 => 300,
                _ => 100
            };
        }

        private double GetAlertThreshold()
        {
            double threshold = 0.80;
            if (InvokeRequired)
            {
                Invoke(() => threshold = ParseThreshold());
            }
            else
            {
                threshold = ParseThreshold();
            }
            return threshold;
        }

        private double ParseThreshold()
        {
            if (double.TryParse(txtThreshold.Text.Trim(),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double v))
            {
                // Support both 0.80 and 80 style input
                return v > 1.0 ? v / 100.0 : v;
            }
            return 0.80;
        }

        private void RecalculateAndRender()
        {
            lock (_calcLock)
            {
                var symbols = _assets.Keys.OrderBy(s => s).ToArray();
                int count = symbols.Length;
                if (count < 2) return;

                int lookback = GetLookbackCount();
                double threshold = GetAlertThreshold();

                // Snapshot all price histories
                var snapshots = new Dictionary<string, List<double>>(count);
                foreach (var sym in symbols)
                {
                    if (_assets.TryGetValue(sym, out var hist))
                        snapshots[sym] = hist.GetSnapshot(lookback);
                }

                // Build correlation matrix
                var matrix = new double[count, count];
                var alerts = new List<string>();

                for (int i = 0; i < count; i++)
                {
                    matrix[i, i] = 1.0; // self-correlation
                    for (int j = i + 1; j < count; j++)
                    {
                        double r = CalculatePearsonCorrelation(snapshots[symbols[i]], snapshots[symbols[j]]);
                        matrix[i, j] = r;
                        matrix[j, i] = r;

                        // Generate alerts
                        if (r >= threshold)
                        {
                            alerts.Add($"WARNING: {symbols[i]} and {symbols[j]} correlation is ultra-high ({r:F2}). " +
                                       $"Opening BUY positions on both increases systemic exposure!");
                        }
                        else if (r <= -threshold)
                        {
                            alerts.Add($"HEDGE DETECTED: {symbols[i]} and {symbols[j]} are inversely correlated ({r:F2}). " +
                                       $"Positions on both effectively cancel each other out.");
                        }
                    }
                }

                _correlationMatrix = matrix;
                _symbolOrder = symbols;

                // Push results to UI
                UiInvoke(() =>
                {
                    RenderMatrix(symbols, matrix);
                    foreach (var alert in alerts)
                        LogWarning(alert);

                    lblPortfoliosValue.Text = count.ToString();
                    lblGridCountValue.Text = $"{count} × {count}";
                });
            }
        }

        // ══════════════════════════════════════════════
        //  MATRIX RENDERING
        // ══════════════════════════════════════════════

        private void RenderMatrix(string[] symbols, double[,] matrix)
        {
            dgvMatrix.SuspendLayout();
            dgvMatrix.Columns.Clear();
            dgvMatrix.Rows.Clear();

            int n = symbols.Length;

            // Create columns
            for (int j = 0; j < n; j++)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    Name = symbols[j],
                    HeaderText = symbols[j],
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    MinimumWidth = 80
                };
                dgvMatrix.Columns.Add(col);
            }

            // Create rows
            for (int i = 0; i < n; i++)
            {
                var row = new DataGridViewRow();
                row.CreateCells(dgvMatrix);
                row.HeaderCell.Value = symbols[i];
                row.Height = 42;

                for (int j = 0; j < n; j++)
                {
                    double r = matrix[i, j];
                    row.Cells[j].Value = r.ToString("F2");
                    row.Cells[j].Tag = r; // store raw value for painter
                }

                dgvMatrix.Rows.Add(row);
            }

            dgvMatrix.ResumeLayout();
            dgvMatrix.Invalidate();
        }

        // ══════════════════════════════════════════════
        //  CELL PAINTING (color-coded risk heatmap)
        // ══════════════════════════════════════════════

        private void DgvMatrix_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var cell = dgvMatrix.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Tag is not double r) return;

            Color bgColor;
            Color fgColor;

            if (e.RowIndex == e.ColumnIndex)
            {
                // Diagonal: self-correlation
                bgColor = CDiagonal;
                fgColor = Color.FromArgb(80, 100, 140);
            }
            else if (r >= 0.80)
            {
                // High positive – danger red, intensity scales with value
                double intensity = (r - 0.80) / 0.20; // 0..1
                bgColor = BlendColor(Color.FromArgb(120, 25, 50), CHighPos, intensity);
                fgColor = Color.White;
            }
            else if (r <= -0.80)
            {
                // High negative – orange/yellow hedge warning
                double intensity = (Math.Abs(r) - 0.80) / 0.20;
                bgColor = BlendColor(Color.FromArgb(120, 80, 10), CHighNeg, intensity);
                fgColor = Color.Black;
            }
            else if (r >= 0.50)
            {
                // Moderate positive – warm tint
                double intensity = (r - 0.30) / 0.50;
                bgColor = BlendColor(CSlate, Color.FromArgb(100, 30, 50), intensity);
                fgColor = Color.FromArgb(255, 180, 180);
            }
            else if (r <= -0.50)
            {
                // Moderate negative – cool tint
                double intensity = (Math.Abs(r) - 0.30) / 0.50;
                bgColor = BlendColor(CSlate, Color.FromArgb(100, 80, 20), intensity);
                fgColor = Color.FromArgb(255, 220, 150);
            }
            else
            {
                // Low/no correlation – healthy diversification
                bgColor = CSlate;
                fgColor = Color.FromArgb(180, 195, 225);
            }

            if (e.CellStyle == null) return;
            e.CellStyle.BackColor = bgColor;
            e.CellStyle.ForeColor = fgColor;
            e.CellStyle.SelectionBackColor = bgColor;
            e.CellStyle.SelectionForeColor = fgColor;
        }

        private static Color BlendColor(Color from, Color to, double t)
        {
            t = Math.Clamp(t, 0, 1);
            return Color.FromArgb(
                (int)(from.R + (to.R - from.R) * t),
                (int)(from.G + (to.G - from.G) * t),
                (int)(from.B + (to.B - from.B) * t));
        }

        // ══════════════════════════════════════════════
        //  BUTTON HANDLERS
        // ══════════════════════════════════════════════

        private void BtnRecalculate_Click(object? sender, EventArgs e)
        {
            if (_assets.IsEmpty)
            {
                LogWarning("No asset data loaded. Send data via pipe or run simulation first.");
                return;
            }
            LogInfo("Manual recalculation triggered.");
            Task.Run(RecalculateAndRender);
        }

        private void BtnSimulate_Click(object? sender, EventArgs e)
        {
            LogInfo("═══ SIMULATING LIVE PORTFOLIO DISRUPTION ═══");

            var rng = new Random(42); // deterministic seed for reproducibility

            // Correlated base series (shared trend component)
            var baseSeries = GenerateRandomWalk(rng, 300, 1.3000, 0.0008);

            // EURUSD & GBPUSD: highly positively correlated (~0.90+)
            var eurusd = AddNoise(baseSeries, rng, 0.0002);
            var gbpusd = AddNoise(baseSeries.Select(p => p * 1.22).ToList(), rng, 0.0003);

            // USDCHF: inversely correlated to EUR/GBP
            var usdchf = baseSeries.Select(p => 2.60 - p + rng.NextDouble() * 0.0003).ToList();

            // AUDUSD: moderately correlated to base
            var audusd = GenerateRandomWalk(rng, 300, 0.6700, 0.0006);
            for (int i = 0; i < 300; i++)
                audusd[i] = audusd[i] * 0.5 + baseSeries[i] * 0.35 + 0.1;

            // XAUUSD: gold – mildly inverse to USD strength (moderate neg correlation)
            var xauusd = GenerateRandomWalk(rng, 300, 2350.0, 3.5);
            for (int i = 0; i < 300; i++)
                xauusd[i] = xauusd[i] - (baseSeries[i] - 1.3) * 800;

            // USDJPY: different dynamics, low correlation
            var usdjpy = GenerateRandomWalk(rng, 300, 154.50, 0.15);

            // NZDUSD: highly correlated with AUDUSD
            var nzdusd = audusd.Select(p => p * 0.92 + rng.NextDouble() * 0.0002).ToList();

            // USDCAD: moderate inverse to risk-on basket
            var usdcad = baseSeries.Select(p => 2.72 - p * 0.6 + rng.NextDouble() * 0.003).ToList();

            // Populate assets
            var simData = new Dictionary<string, List<double>>
            {
                ["EURUSD"] = eurusd,
                ["GBPUSD"] = gbpusd,
                ["USDCHF"] = usdchf,
                ["AUDUSD"] = audusd,
                ["XAUUSD"] = xauusd,
                ["USDJPY"] = usdjpy,
                ["NZDUSD"] = nzdusd,
                ["USDCAD"] = usdcad
            };

            foreach (var kvp in simData)
            {
                var hist = _assets.GetOrAdd(kvp.Key, _ => new AssetPriceHistory());
                hist.SetPrices(kvp.Value);
            }

            LogInfo($"Injected 8 synthetic instruments × 300 candles.");

            Task.Run(RecalculateAndRender);
        }

        // ══════════════════════════════════════════════
        //  SIMULATION HELPERS
        // ══════════════════════════════════════════════

        private static List<double> GenerateRandomWalk(Random rng, int count, double start, double volatility)
        {
            var prices = new List<double>(count) { start };
            for (int i = 1; i < count; i++)
            {
                double change = (rng.NextDouble() - 0.5) * 2.0 * volatility;
                prices.Add(prices[i - 1] + change);
            }
            return prices;
        }

        private static List<double> AddNoise(List<double> source, Random rng, double noiseLevel)
        {
            return source.Select(p => p + (rng.NextDouble() - 0.5) * 2.0 * noiseLevel).ToList();
        }

        // ══════════════════════════════════════════════
        //  LOGGING
        // ══════════════════════════════════════════════

        private void LogInfo(string message)
        {
            AppendLog(message, CCyan);
        }

        private void LogWarning(string message)
        {
            AppendLog(message, CHighPos);
        }

        private void AppendLog(string message, Color color)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string line = $"[{timestamp}] {message}\n";

            if (rtbLog.InvokeRequired)
            {
                UiInvoke(() => AppendLogInternal(line, color));
            }
            else
            {
                AppendLogInternal(line, color);
            }
        }

        private void AppendLogInternal(string line, Color color)
        {
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor = color;
            rtbLog.AppendText(line);
            rtbLog.ScrollToCaret();

            // Cap log size
            if (rtbLog.Lines.Length > 500)
            {
                rtbLog.SelectionStart = 0;
                rtbLog.SelectionLength = rtbLog.GetFirstCharIndexFromLine(100);
                rtbLog.SelectedText = "";
            }
        }

        // ══════════════════════════════════════════════
        //  THREAD-SAFE UI INVOKE HELPER
        // ══════════════════════════════════════════════

        private void UiInvoke(Action action)
        {
            if (IsDisposed || !IsHandleCreated) return;

            if (InvokeRequired)
            {
                try { BeginInvoke(action); }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
            }
            else
            {
                action();
            }
        }
    }
}
