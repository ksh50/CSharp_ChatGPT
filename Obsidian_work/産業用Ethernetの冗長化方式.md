#ChatGPT #制御 #通信

```
依頼：
各産業用Ethernetの冗長化方式を教えてください。
```

以下では 5 大産業用 Ethernet（EtherCAT / PROFINET / EtherNet IP / POWERLINK / CC‑Link IE TSN）の**冗長化方式**を  
「①配線（メディア）冗長」「②制御器（マスター／CPU）冗長」「③経路／フレーム冗長」の 3 観点で整理します。

|プロトコル|① 配線／リング冗長|② マスター／CPU冗長|③ 経路・フレーム冗長|典型復旧時間*|
|---|---|---|---|---|
|**EtherCAT**|二重ポートでリングを構成し**Cable Redundancy**。断線検知後 < 15 µs で逆方向通信へ切替え ([ethercat.org](https://www.ethercat.org/download/documents/etg_brochure_en.pdf?utm_source=chatgpt.com "[PDF] EtherCAT Brochure"), [acontis.com](https://www.acontis.com/en/fpcr.html?utm_source=chatgpt.com "EtherCAT Cable Redundancy - acontis"))|**Hot‑Standby Master** で片方が待機し即時引継ぎ ([ethercat.org](https://www.ethercat.org/en/technology.html?utm_source=chatgpt.com "the Ethernet Fieldbus - EtherCAT Technology Group"))|―（フレーム複製なし）|< 15 µs|
|**PROFINET RT/IRT**|**MRP**（Media Redundancy Protocol）リング（RT向け）／**MRPD**（Planned Duplication, IRT向け） ([us.profinet.com](https://us.profinet.com/profinet-media-redundancy/?utm_source=chatgpt.com "PROFINET Media Redundancy - PI North America"), [hms-networks.com](https://www.hms-networks.com/tech-blog/blogpost/hms-blog/2023/11/13/industrial-ethernet-rings--profinet--mrp-and-mrpd?utm_source=chatgpt.com "Industrial Ethernet Rings: PROFINet® MRP and MRPD"), [ウィキペディア](https://en.wikipedia.org/wiki/Media_Redundancy_Protocol?utm_source=chatgpt.com "Media Redundancy Protocol"))|**S2/S2+** 系統で2 CPUが同一デバイスを二重接続|MRPDは同フレームを双方方向へ同時送信（切替え 0 ms）|MRP ≤ 10 ms（14 SW以下時） ([Fiberroad Technology](https://fiberroad.com/resources/tech-notes/an-introduction-to-media-redundancy-protocol-mrp/?utm_source=chatgpt.com "An Introduction to Media Redundancy Protocol (MRP) - Fiberroad"))|
|**EtherNet /IP**|**DLR**（Device Level Ring）によりリング切替え < 3 ms。リングスーパバイザ1台＋バックアップ1台 ([Rockwell Automation](https://literature.rockwellautomation.com/idc/groups/literature/documents/at/enet-at007_-en-p.pdf?utm_source=chatgpt.com "[PDF] EtherNet/IP Device Level Ring Application Technique"), [Cisco](https://www.cisco.com/c/en/us/td/docs/switches/lan/cisco_ie3X00/software/17_4/b_redundancy_17-4_iot_switch_cg/m-device-level-ring.html?utm_source=chatgpt.com "Device Level Ring [Cisco Catalyst IE3200 Rugged Series]"))|ControlLogix／GuardLogix の**冗長ペアCPU**（Primary/Secondary）|**PRP／HSR**対応製品で二重ネットワーク・無瞬断切替え ([ウィキペディア](https://en.wikipedia.org/wiki/Parallel_Redundancy_Protocol?utm_source=chatgpt.com "Parallel Redundancy Protocol"))|DLR < 3 ms|
|**Ethernet POWERLINK**|**リング／二重ケーブル**。X20 BC8084 などで分岐・ケーブル冗長 ([appstoragepublicprod.s3-us-west-1.amazonaws.com](https://appstoragepublicprod.s3-us-west-1.amazonaws.com/PartAttachment/2124940/Redundancyforcontrolsystems.pdf?utm_source=chatgpt.com "[PDF] Redundancy for control systems - AWS"))|**Redundant MN（Managing Node）**：待機MNが1周期後に役割継承|openSAFETY上もデータ二重化可|通常1 サイクル未満（≒ ≤ 200 µs）|
|**CC‑Link IE TSN**|**冗長ループ / リング**：ケーブル断でも自動ループバックで通信継続 ([三菱電機](https://www.mitsubishielectric.com/fa/products/cnt/plcnet/pmerit/cclink_ie_tsn/features/connectivity.html?utm_source=chatgpt.com "Programmable Controllers MELSEC CC-Link IE TSN Features"), [mymitsubishisupport.co.za](https://www.mymitsubishisupport.co.za/cc-link-ie/?utm_source=chatgpt.com "CC-Link IE - My Mitsubishi Support"), [MITSUBISHI ELECTRIC UNITED STATES](https://us.mitsubishielectric.com/fa/en/products/cnt/programmable-controllers/network-related-products/cclink-ie/feature/improve-reliability/?utm_source=chatgpt.com "PLC Network CC-Link IE Redundant System - Mitsubishi Electric"))|**二重CPU＋二重ケーブル**による多段冗長構成（iQ‑R Safety 等） ([MITSUBISHI ELECTRIC UNITED STATES](https://us.mitsubishielectric.com/fa/en/products/cnt/programmable-controllers/network-related-products/cclink-ie/feature/improve-reliability/?utm_source=chatgpt.com "PLC Network CC-Link IE Redundant System - Mitsubishi Electric"))|TSNの**パス冗長（Frame Replication & Elimination）**をオプション採用|通信無停止（シームレス）|

*復旧時間は代表値。実際は機器数や設定で変動します。

---

### 解説ポイント

1. **配線（メディア）冗長**  
    _EtherCAT・POWERLINK・CC‑Link IE TSN_ はノードが2 ポート標準搭載のため追加スイッチ不要でリング化可能。  
    _PROFINET_ はスイッチ／デバイスが MRP 対応か確認が必須。  
    _EtherNet /IP_ の DLR は“リングスーパバイザ”役の機器が鍵。
    
2. **マスター／CPU冗長**  
    _EtherCAT_ Hot‑Standby、_POWERLINK_ MN 冗長、_ControlLogix GuardLogix_ などは制御周期をほぼ止めずに主従交替。  
    _PROFINET S2_ はデバイスが 1 台で 2 CPUへ同時接続できるため配線も簡潔。  
    _CC‑Link IE TSN_ は制御CPU自体を二重化し、TSN の時分割同期で状態を揃える。
    
3. **経路／フレーム冗長**  
    _MRPD（PROFINET IRT）_、_PRP/HSR_、_TSN FRER_ は**同一データを並列送信**し、通信断を“ゼロスイッチオーバ”で吸収。  
    逆に _EtherCAT_ はフレーム長が変化すると同期ずれになるため複製方式を取らず、ケーブル予備経路で対応。
    

---

### 選定のヒント

|求める可用性|推奨方式|
|---|---|
|**µs級サーボを止めずに断線検知したい**|EtherCAT Cable Redundancy|
|**ライン／PLC混在で10 ms以内復旧**|PROFINET MRP|
|**コントローラもネットも二重化し完全シームレス**|EtherNet /IP (PRP or DLR＋冗長CPU) / CC‑Link IE TSN|
|**Linuxベースで独自装置を組む**|POWERLINK 二重MN＋リング|

---

#### さらに詳しく知りたい場合

- 具体的な配線パターン例（スター＋サブリング併用など）
    
- 各冗長方式での**IPアドレス設計**や**診断タグ**
    
- 冗長化と**機能安全**の同時設計ノウハウ
    

――など、ご要望に応じて掘り下げも可能です。お気軽にお声かけください！


[[産業用Ethernet_冗長性と機能安全の同時設計]]