namespace PortfolioCorrelation
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pipeCts?.Cancel();
                _pipeCts?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ── Color Constants ──
            var cBackground = Color.FromArgb(10, 15, 29);      // #0A0F1D
            var cPanel = Color.FromArgb(19, 26, 48);            // #131A30
            var cCyan = Color.FromArgb(0, 210, 255);            // #00D2FF
            var cAccentText = Color.FromArgb(180, 195, 225);
            var cSlate = Color.FromArgb(30, 41, 75);            // #1E294B
            var cHeaderFont = new Font("Segoe UI", 11F, FontStyle.Bold);
            var cBodyFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            var cSmallFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);

            // ══════════════════════════════════════════════
            //  LEFT SIDEBAR
            // ══════════════════════════════════════════════
            pnlSidebar = new Panel();
            lblLogo = new Label();
            lblLogoSub = new Label();
            pnlEngineStatus = new Panel();
            lblEngineTitle = new Label();
            lblEngineValue = new Label();
            lblPortfoliosTitle = new Label();
            lblPortfoliosValue = new Label();
            lblGridCountTitle = new Label();
            lblGridCountValue = new Label();
            btnSimulate = new Button();

            // pnlSidebar
            pnlSidebar.BackColor = cPanel;
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Width = 260;
            pnlSidebar.Padding = new Padding(16, 20, 16, 20);

            // lblLogo
            lblLogo.Text = "AUTOSCRIPTS";
            lblLogo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblLogo.ForeColor = cCyan;
            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(20, 24);

            // lblLogoSub
            lblLogoSub.Text = "MATRIX";
            lblLogoSub.Font = new Font("Segoe UI", 14F, FontStyle.Regular);
            lblLogoSub.ForeColor = Color.FromArgb(100, 120, 160);
            lblLogoSub.AutoSize = true;
            lblLogoSub.Location = new Point(22, 56);

            // pnlEngineStatus
            pnlEngineStatus.BackColor = Color.FromArgb(14, 20, 38);
            pnlEngineStatus.Location = new Point(16, 100);
            pnlEngineStatus.Size = new Size(228, 220);

            // Engine labels
            lblEngineTitle.Text = "MATHEMATICAL ENGINE";
            lblEngineTitle.Font = cSmallFont;
            lblEngineTitle.ForeColor = Color.FromArgb(100, 120, 160);
            lblEngineTitle.Location = new Point(12, 12);
            lblEngineTitle.AutoSize = true;

            lblEngineValue.Text = "● ACTIVE";
            lblEngineValue.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblEngineValue.ForeColor = cCyan;
            lblEngineValue.Location = new Point(12, 32);
            lblEngineValue.AutoSize = true;

            lblPortfoliosTitle.Text = "TOTAL PORTFOLIOS MONITORED";
            lblPortfoliosTitle.Font = cSmallFont;
            lblPortfoliosTitle.ForeColor = Color.FromArgb(100, 120, 160);
            lblPortfoliosTitle.Location = new Point(12, 72);
            lblPortfoliosTitle.AutoSize = true;

            lblPortfoliosValue.Text = "0";
            lblPortfoliosValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblPortfoliosValue.ForeColor = Color.White;
            lblPortfoliosValue.Location = new Point(12, 94);
            lblPortfoliosValue.AutoSize = true;

            lblGridCountTitle.Text = "MATRIX GRID COUNT";
            lblGridCountTitle.Font = cSmallFont;
            lblGridCountTitle.ForeColor = Color.FromArgb(100, 120, 160);
            lblGridCountTitle.Location = new Point(12, 144);
            lblGridCountTitle.AutoSize = true;

            lblGridCountValue.Text = "0 × 0";
            lblGridCountValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblGridCountValue.ForeColor = Color.White;
            lblGridCountValue.Location = new Point(12, 166);
            lblGridCountValue.AutoSize = true;

            pnlEngineStatus.Controls.AddRange(new Control[] {
                lblEngineTitle, lblEngineValue,
                lblPortfoliosTitle, lblPortfoliosValue,
                lblGridCountTitle, lblGridCountValue
            });

            // btnSimulate
            btnSimulate.Text = "⚡ SIMULATE LIVE\nPORTFOLIO DISRUPTION";
            btnSimulate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSimulate.ForeColor = Color.White;
            btnSimulate.BackColor = Color.FromArgb(180, 40, 80);
            btnSimulate.FlatStyle = FlatStyle.Flat;
            btnSimulate.FlatAppearance.BorderSize = 0;
            btnSimulate.Location = new Point(16, 340);
            btnSimulate.Size = new Size(228, 60);
            btnSimulate.Cursor = Cursors.Hand;
            btnSimulate.Click += BtnSimulate_Click;

            pnlSidebar.Controls.AddRange(new Control[] {
                lblLogo, lblLogoSub, pnlEngineStatus, btnSimulate
            });

            // ══════════════════════════════════════════════
            //  TOP CONTROL BAR
            // ══════════════════════════════════════════════
            pnlTopBar = new Panel();
            lblLookback = new Label();
            cboLookback = new ComboBox();
            lblThreshold = new Label();
            txtThreshold = new TextBox();
            btnRecalculate = new Button();

            pnlTopBar.BackColor = cPanel;
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Height = 56;
            pnlTopBar.Padding = new Padding(16, 0, 16, 0);

            lblLookback.Text = "LOOKBACK PERIOD:";
            lblLookback.Font = cSmallFont;
            lblLookback.ForeColor = cAccentText;
            lblLookback.AutoSize = true;
            lblLookback.Location = new Point(20, 18);

            cboLookback.Font = cBodyFont;
            cboLookback.BackColor = cSlate;
            cboLookback.ForeColor = Color.White;
            cboLookback.FlatStyle = FlatStyle.Flat;
            cboLookback.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLookback.Items.AddRange(new object[] { "Last 50 Candles", "Last 100 Candles", "Last 300 Candles" });
            cboLookback.SelectedIndex = 1;
            cboLookback.Location = new Point(166, 14);
            cboLookback.Size = new Size(170, 28);

            lblThreshold.Text = "HIGH CORR. ALERT ≥";
            lblThreshold.Font = cSmallFont;
            lblThreshold.ForeColor = cAccentText;
            lblThreshold.AutoSize = true;
            lblThreshold.Location = new Point(370, 18);

            txtThreshold.Font = cBodyFont;
            txtThreshold.BackColor = cSlate;
            txtThreshold.ForeColor = cCyan;
            txtThreshold.BorderStyle = BorderStyle.FixedSingle;
            txtThreshold.Text = "0.80";
            txtThreshold.Location = new Point(528, 14);
            txtThreshold.Size = new Size(60, 28);
            txtThreshold.TextAlign = HorizontalAlignment.Center;

            btnRecalculate.Text = "⟳ RECALCULATE MATRIX";
            btnRecalculate.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRecalculate.ForeColor = Color.White;
            btnRecalculate.BackColor = Color.FromArgb(0, 140, 200);
            btnRecalculate.FlatStyle = FlatStyle.Flat;
            btnRecalculate.FlatAppearance.BorderSize = 0;
            btnRecalculate.Location = new Point(620, 10);
            btnRecalculate.Size = new Size(210, 36);
            btnRecalculate.Cursor = Cursors.Hand;
            btnRecalculate.Click += BtnRecalculate_Click;

            pnlTopBar.Controls.AddRange(new Control[] {
                lblLookback, cboLookback, lblThreshold, txtThreshold, btnRecalculate
            });

            // ══════════════════════════════════════════════
            //  CENTRAL MATRIX GRID
            // ══════════════════════════════════════════════
            dgvMatrix = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvMatrix).BeginInit();

            dgvMatrix.BackgroundColor = cBackground;
            dgvMatrix.GridColor = Color.FromArgb(25, 35, 65);
            dgvMatrix.BorderStyle = BorderStyle.None;
            dgvMatrix.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvMatrix.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvMatrix.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvMatrix.EnableHeadersVisualStyles = false;
            dgvMatrix.AllowUserToAddRows = false;
            dgvMatrix.AllowUserToDeleteRows = false;
            dgvMatrix.AllowUserToResizeRows = false;
            dgvMatrix.AllowUserToResizeColumns = false;
            dgvMatrix.ReadOnly = true;
            dgvMatrix.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvMatrix.MultiSelect = false;
            dgvMatrix.RowHeadersVisible = true;
            dgvMatrix.RowHeadersWidth = 110;
            dgvMatrix.DefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 50, 90);
            dgvMatrix.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvMatrix.Dock = DockStyle.Fill;
            dgvMatrix.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Column header style
            var colHeaderStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(16, 22, 42),
                ForeColor = cCyan,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };
            dgvMatrix.ColumnHeadersDefaultCellStyle = colHeaderStyle;
            dgvMatrix.ColumnHeadersHeight = 42;
            dgvMatrix.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Row header style
            var rowHeaderStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(16, 22, 42),
                ForeColor = cCyan,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            dgvMatrix.RowHeadersDefaultCellStyle = rowHeaderStyle;

            // Default cell style
            var defaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = cSlate,
                ForeColor = Color.FromArgb(180, 195, 225),
                Font = new Font("Consolas", 11F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };
            dgvMatrix.DefaultCellStyle = defaultCellStyle;

            dgvMatrix.CellPainting += DgvMatrix_CellPainting;

            ((System.ComponentModel.ISupportInitialize)dgvMatrix).EndInit();

            // ══════════════════════════════════════════════
            //  LOG CONSOLE
            // ══════════════════════════════════════════════
            pnlLogHeader = new Panel();
            lblLogTitle = new Label();
            rtbLog = new RichTextBox();

            pnlLogHeader.BackColor = cPanel;
            pnlLogHeader.Dock = DockStyle.Bottom;
            pnlLogHeader.Height = 26;

            lblLogTitle.Text = "  ▸ EXPOSURE ALERT CONSOLE";
            lblLogTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblLogTitle.ForeColor = Color.FromArgb(100, 120, 160);
            lblLogTitle.Dock = DockStyle.Fill;
            lblLogTitle.TextAlign = ContentAlignment.MiddleLeft;
            pnlLogHeader.Controls.Add(lblLogTitle);

            rtbLog.BackColor = Color.FromArgb(8, 12, 22);
            rtbLog.ForeColor = Color.FromArgb(140, 160, 200);
            rtbLog.Font = new Font("Consolas", 9F);
            rtbLog.ReadOnly = true;
            rtbLog.BorderStyle = BorderStyle.None;
            rtbLog.Dock = DockStyle.Bottom;
            rtbLog.Height = 140;
            rtbLog.ScrollBars = RichTextBoxScrollBars.Vertical;

            // ══════════════════════════════════════════════
            //  MAIN CONTENT PANEL (holds grid between top bar and log)
            // ══════════════════════════════════════════════
            pnlMain = new Panel();
            pnlMain.BackColor = cBackground;
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Padding = new Padding(8);
            pnlMain.Controls.Add(dgvMatrix);     // Fill
            pnlMain.Controls.Add(pnlTopBar);     // Top

            // ══════════════════════════════════════════════
            //  FORM
            // ══════════════════════════════════════════════
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = cBackground;
            ClientSize = new Size(1440, 860);
            MinimumSize = new Size(1100, 680);
            Text = "MT5 Portfolio Correlation & Risk Matrix Analyzer";
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9.5F);

            // Add controls in correct dock order (reverse for Fill to work)
            Controls.Add(pnlMain);         // Fill – added first
            Controls.Add(rtbLog);          // Bottom
            Controls.Add(pnlLogHeader);    // Bottom (above log)
            Controls.Add(pnlSidebar);      // Left
        }

        #endregion

        // ── Sidebar ──
        private Panel pnlSidebar;
        private Label lblLogo;
        private Label lblLogoSub;
        private Panel pnlEngineStatus;
        private Label lblEngineTitle;
        private Label lblEngineValue;
        private Label lblPortfoliosTitle;
        private Label lblPortfoliosValue;
        private Label lblGridCountTitle;
        private Label lblGridCountValue;
        private Button btnSimulate;

        // ── Top Bar ──
        private Panel pnlTopBar;
        private Label lblLookback;
        private ComboBox cboLookback;
        private Label lblThreshold;
        private TextBox txtThreshold;
        private Button btnRecalculate;

        // ── Matrix ──
        private DataGridView dgvMatrix;

        // ── Log ──
        private Panel pnlLogHeader;
        private Label lblLogTitle;
        private RichTextBox rtbLog;

        // ── Main ──
        private Panel pnlMain;
    }
}
