#ChatGPT #FPGA #オープンソース

```
質問：
TinyFPGAのようなオープンソースのFPGAハードウェアはありますか？
```

はい、TinyFPGAのように**オープンソースハードウェア**として開発されているFPGAボードは他にもいくつか存在します。以下に代表的なものを紹介します。

---

## ✅ オープンソースFPGAボード一覧

### 1. **iCEBreaker (by 1BitSquared)**

- **FPGAチップ**: Lattice iCE40UP5K（Low-power, small FPGA）
    
- **特徴**:
    
    - 完全オープンソース（ハードウェア設計・ブートローダ）
        
    - Pmod互換拡張ポートあり（拡張ボード豊富）
        
    - USBで書き込み可（DFUブートローダ）
        
- **開発ツール**: Yosys + nextpnr + IceStorm（完全OSSツールチェーン）
    
- **公式サイト**: [https://icebreaker-fpga.org/](https://icebreaker-fpga.org/)
    

---

### 2. **Fomu**

- **FPGAチップ**: Lattice iCE40UP5K
    
- **特徴**:
    
    - USBコネクタに**直接挿せる超小型FPGA**（PCBサイズがUSB端子サイズ）
        
    - **Python（LiteX）やVerilog**で開発可能
        
    - 完全オープンソースハードウェア
        
- **開発ツール**:
    
    - Verilog: Yosys / IceStorm
        
    - Migen/LiteXベースの開発も可能（ソフトコアCPUなど）
        
- **公式サイト**: [https://www.fomu.im/](https://www.fomu.im/)
    

---

### 3. **ULX3S**

- **FPGAチップ**: Lattice ECP5（最大85K LUTsまでの構成あり）
    
- **特徴**:
    
    - Wi-Fi、microSD、オーディオ、USB、HDMIなど多数の周辺機能
        
    - オープンソースなPCBとファームウェア
        
    - LiteXやSymbiFlow対応
        
- **公式情報**: [https://github.com/emard/ulx3s-hw](https://github.com/emard/ulx3s-hw)
    

---

### 4. **OrangeCrab**

- **FPGAチップ**: Lattice ECP5（85K LUT）
    
- **形状**: Featherフォームファクタ（Adafruit Feather互換）
    
- **特徴**:
    
    - USB-C、RAM、QSPI Flash搭載
        
    - オープンソースPCB、開発ツール（LiteX, Yosys, nextpnr, etc.）
        
- **GitHub**: [https://github.com/gregdavill/OrangeCrab](https://github.com/gregdavill/OrangeCrab)
    

---

### 5. **ButterStick (by Greg Davill)**

- **FPGAチップ**: Lattice ECP5
    
- **特徴**:
    
    - 高速USB3、DDR3メモリ搭載
        
    - オープンソースハードウェア設計
        
    - 高性能なプロトタイピング向け
        
- **GitHub**: [https://github.com/gregdavill/butterstick-pcb](https://github.com/gregdavill/butterstick-pcb)
    

---

## 💡選ぶポイント

|ポイント|おすすめ|
|---|---|
|小型、手軽さ|Fomu, TinyFPGA BX|
|学習用途、拡張性|iCEBreaker|
|高性能、LiteX活用|ULX3S, OrangeCrab|
|ハードウェア自作志向|iCEBreaker, ULX3S（完全オープン設計）|

---

## 📦 オープンソースツールチェーン対応FPGAチップ

- Lattice iCE40（UP5K, LP8K） ➔ IceStorm対応
    
- Lattice ECP5 ➔ Project Trellis + nextpnr-ecp5
    

---

ご希望に応じて、それぞれのボードでの開発手順や入門向けチュートリアルもご紹介可能です。気になるボードがあればお知らせください。