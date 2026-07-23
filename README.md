<div align="center">

Topics: metatrader5, mql5, correlation-matrix, expert-advisor, mql4, metatrader, forex-trading, risk-management, data-visualization, portfolio-management, currency-strength, correlation-analysis, portfolio-risk, mt4, mt5, trading-bot, mt5-correlation-matrix, currency-strength-meter, fx-portfolio-heatmap

# Information

**Portfolio correlation and multi-symbol risk monitor for MT5 / MT4 traders. The project measures live correlations between open positions, currency baskets, and watchlist symbols, exposing hidden exposure, redundant trades, and unintended hedges across an entire forex portfolio.**

# 🔗 Portfolio Correlation MT5/MT4

**Real-time correlation matrix, currency strength meter, and basket exposure heatmap, built to keep multi-pair traders out of accidental triple positions.**

<br>

[![Stars](https://img.shields.io/github/stars/torvalds/linux?style=for-the-badge&color=00D4AA&label=Stars)](https://github.com/your-username/volume-profile-mt5/stargazers)
[![Forks](https://img.shields.io/github/forks/torvalds/linux?style=for-the-badge&color=4D9FFF&label=Forks)](https://github.com/your-username/volume-profile-mt5/network)
[![Issues](https://img.shields.io/github/issues/torvalds/linux?style=for-the-badge&color=FF4D6A&label=Issues)](https://github.com/your-username/volume-profile-mt5/issues)
[![Platform](https://img.shields.io/badge/MT5%20%2F%20MT4-Compatible-00D4AA?style=for-the-badge)](https://www.metatrader5.com)
[![License](https://img.shields.io/badge/License-MIT-4D9FFF?style=for-the-badge)](LICENSE)

</div>

<p align="center">
    <img src="https://minkxx-spotify-readme.vercel.app/api?theme=dark&rainbow=true&scan=true&spin=True" alt="Preview">
</p>

---

## 📸 Screenshot

<div align="center">

<p align="center">
  <img src="https://i.ibb.co/Qs7790c/5.png" alt="Correlation matrix UI" width="820">
</p>

</div>

---

## 🎬 Demo

<div align="center">

<img src="https://i.imgur.com/jPi5r6b.gif" alt="Demo">

</div>


---

## Why Correlation?

Opening three "different" trades that all move together is just one big trade at triple risk.

This project gives you:
- A live correlation matrix across watched pairs  
- Currency-level strength scoring  
- Aggregated portfolio beta and risk exposure  

---

## What It Does

**Portfolio Correlation MT5/MT4** computes statistical relationships across symbols and projects them as actionable risk metrics.

| Module | Description |
|---|---|
| Correlation Engine | Rolling Pearson on N-bar returns |
| Strength Meter | Per-currency relative strength |
| Exposure Mapper | Net long / short per currency |
| Basket Builder | Custom multi-symbol baskets |
| Heatmap Renderer | Color-coded matrix view |
| Risk Aggregator | Combined risk across open trades |

---

## Features

| Feature | Description |
|---|---|
| Live Matrix | N x N correlation heatmap |
| Strength Bars | USD, EUR, GBP, JPY, etc. |
| Open Position Overlay | Highlights traded pairs |
| Hedge Detector | Flags negatively correlated pairs |
| Window Selector | 20 / 50 / 100 / 200 bars |
| MT4 / MT5 Support | Platform selection system |
| Symbol Sets | FX majors, minors, metals, indices |
| Alert Engine | Threshold-based correlation alerts |
| Export | CSV matrix snapshot |
| Multi-Account Mode | Aggregates across accounts |

---

## System Behavior

- Recomputes on every closed bar
- Smooth rolling window, no lag spikes
- Color scale fixed for visual stability
- Lightweight, runs alongside trading platform

---

## Quick Start

**Requirements:**
- Windows 10 / 11  
- .NET 8+  
- Visual Studio 2022  

```bash
git clone https://github.com/your-username/portfolio-correlation.git
```

Open solution → Press **F5**

---

## How to Use

1. Launch app  
2. Select MT4 / MT5  
3. Enter login  
4. Click **CONNECT**  
5. Pick symbol set & timeframe  
6. Set correlation window  
7. Click **START MATRIX**  
8. Watch overlap and exposure live  

---

## Interface Logic

```
        EURUSD  GBPUSD  USDJPY  XAUUSD
EURUSD   1.00    0.87   -0.74    0.42
GBPUSD   0.87    1.00   -0.62    0.31
USDJPY  -0.74   -0.62    1.00   -0.55
XAUUSD   0.42    0.31   -0.55    1.00
```

- Deep red = strong positive  
- Deep blue = strong negative  
- White = near-zero  
- Border = currently open pair  

---

## Roadmap

- [x] Correlation engine  
- [x] Strength meter  
- [x] Heatmap UI  
- [ ] Real MT5 portfolio bridge  
- [ ] Beta-to-DXY mode  
- [ ] Rolling factor exposure  
- [ ] Multi-broker aggregation  

---

## Contributing

```
1. Fork
2. git checkout -b feature/new-feature
3. git commit -m "Add feature"
4. git push
5. Open PR
```

---

## License

MIT

---

<div align="center">

Portfolio Correlation MT5/MT4 · v1.0

</div>
