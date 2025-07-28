#ChatGPT #制御 #通信 

```
依頼：
産業Ethernetの各プロトコルの詳細比較をお願いします。
```

以下では、日本でよく使われる主要 5 種の産業用 Ethernetプロトコルを、リアルタイム性能・しくみ・物理層・用途など多角的に比較しました。  
まずは要点を押さえられる対比表、そのあと特長と選定時の視点を解説します。

---

### 1. 主要プロトコル比較表

| 項目          | EtherCAT                                                                                      | PROFINET IRT                                                                                                                                                                | EtherNet/IP (CIP Sync/ Motion)                                                                                                                                                               | Ethernet POWERLINK                                                                                                | CC‑Link IE TSN                                                                                                                                                                                                                                                         |
| ----------- | --------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 開発母体        | Beckhoff／ETG                                                                                  | PI (Profibus & PROFINET International)                                                                                                                                      | ODVA                                                                                                                                                                                         | B&R（EPSG→B&R管理）                                                                                                   | CC‑Link協会                                                                                                                                                                                                                                                              |
| 最大通信速度      | 100 Mbps（100BASE‑TX/FX）、2024年から1 Gbps拡張（EtherCAT G）                                           | 100 Mbps（Class C）※TSN版は1 Gbps                                                                                                                                               | 10 M/100 M/1 G（標準Ethernetに依存）                                                                                                                                                                | 100 M／1 Gbps                                                                                                      | 1 Gbps (1000BASE‑T/ F)※Field Basicは100 M                                                                                                                                                                                                                               |
| 最短サイクル／ ジッタ | ≤ 100 µs、ジッタ≤ 1 µs ([ウィキペディア](https://en.wikipedia.org/wiki/EtherCAT "EtherCAT - Wikipedia")) | 31.25 µs（専用ASIC）～250 µs、ジッタ≈ 1 µs ([us.profinet.com](https://us.profinet.com/profinet-irt-isochronous-real-time/ "PROFINET IRT- Isochronous Real Time – PI North America")) | 同期精度 < 100 ns (IEEE 1588)；アプリ毎にサイクル設計 ([odva.org](https://www.odva.org/technology-standards/distinct-cip-services/cip-sync/ "CIP Sync™ \| Common Industrial Protocol \| ODVA Technologies")) | < 200 µs、ジッタ< 1 µs ([ウィキペディア](https://en.wikipedia.org/wiki/Ethernet_Powerlink "Ethernet Powerlink - Wikipedia")) | サイクル31.25 µs、同期± 1 µs ([MITSUBISHI ELECTRIC UNITED STATES](https://us.mitsubishielectric.com/fa/en/products/cnt/programmable-controllers/network-related-products/cclink-ie-tsn/features/ "PLC Network CC-Link IE TSN Performance \| Mitsubishi Electric Automation")) |
| 決定性のしくみ     | **フレームを各ノードが“通過中に”処理**（オンザフライ）                                                                | **時分割+帯域予約**（スケジューラ）                                                                                                                                                        | **PTP時刻基準でタイムスタンプ駆動**                                                                                                                                                                        | **ポーリング＋タイムスライス**（MNがSoC→Preq/Pres）                                                                               | **TSN時間スロット**＋優先度分離                                                                                                                                                                                                                                                    |
| 追加ハード要件     | スレーブ用ASIC必須、マスターは汎用NIC可                                                                       | IRT対応スイッチ/ASICが必須                                                                                                                                                           | PTP対応NIC/スイッチ推奨                                                                                                                                                                              | ハブ／MN実装が制御                                                                                                        | TSN対応スイッチ必須（Class B）                                                                                                                                                                                                                                                   |
| 拡張機能        | Distributed Clock、FSoE安全                                                                      | PROFIsafe、MRP冗長                                                                                                                                                             | CIP Safety/Security、Ethernet‑APL                                                                                                                                                             | openSAFETY                                                                                                        | Safety、IT/OT混在、256軸同期                                                                                                                                                                                                                                                  |
| 代表用途        | 半導体装置、精密モーション                                                                                 | 汎用PLC＋サーボ、工作機械                                                                                                                                                              | 大規模ライン、ロボット、プロセス制御                                                                                                                                                                           | 包装機、ロボット                                                                                                          | 日本・アジアのFA、高速モーション                                                                                                                                                                                                                                                      |
| ライセンス       | 仕様公開、ETG会員無償                                                                                  | オープン（IEC 61784‑2）                                                                                                                                                           | オープン（ODVA仕様）                                                                                                                                                                                 | オープンソース実装あり                                                                                                       | 仕様公開、ロイヤリティ無                                                                                                                                                                                                                                                           |

---

### 2. プロトコル別ポイント解説

#### ■ EtherCAT

- **オンザフライ処理**でヘッダーをほぼスキップし、1 フレームで最大6.5 万台を巡回。
    
- 100 µs以下の更新周期・µs級ジッタでサーボ同期が容易。
    
- スレーブはESC（EtherCAT Slave Controller）搭載が必須だが、マスターは汎用NICで実装可能。 ([ウィキペディア](https://en.wikipedia.org/wiki/EtherCAT "EtherCAT - Wikipedia"))
    

#### ■ PROFINET IRT

- RT（1–10 ms）とIRT（≤31.25 µs）の2層リアルタイム。
    
- IRTは**帯域予約＋カットスルースイッチ**で衝突を排除、ジッタ1 µs。
    
- CC‑C機器とIRT対応スイッチが必要だが、RT機器と混在も可能。 ([us.profinet.com](https://us.profinet.com/profinet-irt-isochronous-real-time/ "PROFINET IRT- Isochronous Real Time – PI North America"))
    

#### ■ EtherNet/IP ＋ CIP Sync／Motion

- 標準Ethernetをそのまま使用し、**IEEE 1588 PTP**で<100 nsの高精度同期。
    
- サイクルはアプリ設計依存（数百 µs～ms台）だが、多軸モーションも可能。
    
- TCP/UDP共存が容易で、IT統合や大規模製造ラインに強い。 ([odva.org](https://www.odva.org/technology-standards/distinct-cip-services/cip-sync/ "CIP Sync™ | Common Industrial Protocol | ODVA Technologies"))
    

#### ■ Ethernet POWERLINK

- 100 M／1 G環境でも**MN（Managing Node）のポーリング**で決定性を確保。
    
- 最新実装は200 µs未満のサイクル、ジッタ<1 µs。
    
- オープンソーススタックがあり、Linuxリアルタイム拡張と組み合わせ例も多い。 ([ウィキペディア](https://en.wikipedia.org/wiki/Ethernet_Powerlink "Ethernet Powerlink - Wikipedia"))
    

#### ■ CC‑Link IE TSN

- ギガビットEthernetを前提に**TSN(time‑aware shaper)**を活用、最小31.25 µs周期。
    
- TCP/IPトラフィックとリアルタイム通信を同線で混在可能。
    
- モーション同期±1 µs／256軸、アジア圏での装置間リンクに強み。 ([MITSUBISHI ELECTRIC UNITED STATES](https://us.mitsubishielectric.com/fa/en/products/cnt/programmable-controllers/network-related-products/cclink-ie-tsn/features/ "PLC Network CC-Link IE TSN Performance | Mitsubishi Electric Automation"))
    

---

### 3. 選定時の着眼点

|観点|重視するケース|推奨候補|
|---|---|---|
|**µs級の最短周期／配線シンプル**|単一ラインで超高速モーション|EtherCAT|
|**既存Siemens PLCとの親和性**|ドライブ連携・ライン制御|PROFINET IRT|
|**IT/OT統合・大規模ネットワーク**|ロボット島＋MES連携|EtherNet/IP|
|**オープンソース・Linuxリアルタイム**|独自制御アプリ開発|POWERLINK|
|**日本国内装置間の高速同期**|半導体/FPD装置、複数軸協調|CC‑Link IE TSN|

---

### 4. 今後のトレンド

- **TSN化**: PROFINET CC‑D、EtherNet/IP ＋ PTP/TSN、EtherCAT G(1 Gbps) ＋ TSNブリッジなど、各陣営ともIEEE 802.1 TSNへの対応を進行。
    
- **Ethernet‑APL**: 2‑wire 10 Mbps本線でプロセス計装 (PROFINET APL, EtherNet/IP APL 等) が2024年から普及期。
    
- **セキュリティ／統合**: CIP Security、PROFINET Security Class 3、FSoE/PROFIsafe/OpenSAFETYなど、OTレベルでの暗号化・機能安全が標準装備化。
    

---

#### まとめ

> 同じ「産業用Ethernet」でも、**決定性を担保するアプローチとリアルタイム性能のレンジが大きく異なる**ため、  
> ‑ 必要な制御周期、  
> ‑ 既存PLC/サーボとの互換性、  
> ‑ ネットワーク規模・IT統合度、  
> ‑ ハードウェア／ライセンスコスト  
> を軸に選定すると失敗しにくくなります。

追加で深掘りしたいポイント（例：冗長化方式、機能安全実装手順、スレーブスタック実装例など）があれば気軽にお知らせください！