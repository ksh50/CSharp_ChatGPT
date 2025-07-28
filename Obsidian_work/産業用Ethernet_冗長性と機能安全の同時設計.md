#ChatGPT #制御 #通信 #機能安全

```
依頼：
産業用Ethernetの各プロトコルでの冗長化と機能安全の同時設計ノウハウについて教えてください。
```

以下では **冗長化（高可用性）と機能安全（SIL/PL）を“同時に”設計する際の実務ノウハウ** を 5 大産業用 Ethernet プロトコル別にまとめます。  
①ネットワーク／配線冗長、②コントローラ冗長、③機能安全プロトコル設定、④タイミング設計・検証――の4 視点で整理しました。

---

## 0. 全方式に共通する設計原則

1. **ウォッチドッグと復旧時間のマージンを確保**  
    _Safety Watchdog Time_ > _最悪の経路切替時間_（リング再収束、CPU切替）  
    例：PROFINET MRP 最長10 ms ⇒ PROFIsafe Watchdog 30 ms 以上に設定。
    
2. **安全パラメータを冗長経路で“同一”に保つ**  
    _Safety ID／SNN／F‑Address_ 等は経路・CPUが変わっても不変にすることで、切替後も接続を維持。
    
3. **診断を一本化**  
    ネットワーク冗長イベント（DLR Fault, MRP TopologyChange 等）と安全関連イベント（FSoE Error, PROFIsafe F‑Error など）を同じ SCADA/HMI に集約すると原因切り分けが迅速。
    
4. **同時テストを必ず実施**  
    ケーブル断／スイッチ断電／CPUフェイルオーバ試験を“安全機能有効のまま”実行し、
    
    - セーフステートに落ちないこと（許容される場合は即停止）
        
    - サイクルタイム／ジッタの逸脱が基準内  
        を確認する。
        

---

## 1. EtherCAT ＋ FSoE

|項目|推奨実装|
|---|---|
|ネット冗長|**Cable Redundancy**：2‑ポートスレーブでリング化。断線検知 ≤ 15 µs → 逆方向ループへ自動切替え ([EtherCAT](https://www.ethercat.org/en/technology.html?utm_source=chatgpt.com "the Ethernet Fieldbus - EtherCAT Technology Group"), [Beckhoff Automation](https://www.beckhoff.com/en-us/products/i-o/ethercat/?utm_source=chatgpt.com "EtherCAT \| Beckhoff USA"))|
|CPU冗長|**Hot‑Standby Master**：待機マスターが同一プロセスイメージを保持し、LINK LOSS 検知で 1 周期後に制御権を継承。|
|安全設定|_Safety ID_ を各ノードに固定。両マスターで **同一 SAL**（Safety Application List）を署名し TwinSAFE Loader でダウンロード。FSoE CRC ⊕ Distributed Clock は経路変化の影響を受けない。 ([Beckhoff USA Blog](https://www.blog.beckhoffus.com/post/safety-over-ethercat?utm_source=chatgpt.com "Just the Facts on Safety over EtherCAT - Beckhoff USA Blog"))|
|タイミング|ケーブル断でもフレームが折返すだけなのでサイクル延伸は≪1 µs。FSoE Watchdog 標準 100 ms（十分マージン）。|

---

## 2. PROFINET IRT ＋ PROFIsafe

|項目|推奨実装|
|---|---|
|ネット冗長|**MRP**（～10 ms復旧） or **MRPD**（フレーム二重化で0 ms）リング。IRT では MRPD を推奨。 ([PROFINET University](https://profinetuniversity.com/system-redundancy/profinet-redundancy-media-redundancy/?utm_source=chatgpt.com "PROFINET Media Redundancy + Media Redundancy"), [Siemens サポート](https://support.industry.siemens.com/cs/attachments/67364686/67364686_Media_and_System_Redundancy_V1_1_en.pdf?utm_source=chatgpt.com "[PDF] Which PROFINET devices support media redundancy and which ..."))|
|CPU冗長|**S2 System**：2 台のF‑CPUを1 デバイスへ二重接続。デバイス側は自動で Primary/Secondary を切替。 ([profibus.com](https://www.profibus.com/index.php?eID=dumpFile&f=207029&t=f&token=5ec409c02dbe77dedc621d2b7c7b6eee73c7c275&utm_source=chatgpt.com "[PDF] PROFINET System Description"))|
|安全設定|TIA Portal で **F_Source_Add / F_Dest_Add** を固定。両F‑CPUのハードウェア構成コピーでパラメータ完全一致。|
|タイミング|PROFIsafe Watchdog を MRP 復旧 + 送受信 jitter に対して 3× 以上に設定（例：30–100 ms）。IRT の帯域予約が MRPD 複製で崩れないよう OB‑Cycle を計算。|

---

## 3. EtherNet/IP ＋ CIP Safety

|項目|推奨実装|
|---|---|
|ネット冗長|標準は **DLR**（<3 ms切替）リング。シームレス要件なら **PRP/HSR** に対応したスイッチ or エンドノードを2 ポートで二重ネットへ接続。 ([Rockwell Automation](https://literature.rockwellautomation.com/idc/groups/literature/documents/at/enet-at007_-en-p.pdf?utm_source=chatgpt.com "[PDF] EtherNet/IP Device Level Ring Application Technique"), [ODVA](https://www.odva.org/news/ethernet-ip-network-specification-complete-for-ethernet-apl-physical-layer-for-process-automation/?utm_source=chatgpt.com "EtherNet/IP Network Specification Complete for Ethernet-APL ..."))|
|CPU冗長|**GuardLogix/ControlLogix Redundancy**：Primary/Secondary chassis + Coordinated System Time(PTP)。|
|安全設定|Studio5000 で **SNN（Safety Network Number）** を全ネットワークで統一。Safety Task を **Safety Signature** でロックし、両CPU間で自動同期。|
|タイミング|CIP Safety Connection Watchdog ≥ DLR故障検出（3 ms）＋PRP失敗時リトライ。PTP精度 < 100 ns を保てるよう Ring/Supervisor の負荷分散を設計。|

---

## 4. Ethernet POWERLINK ＋ openSAFETY

|項目|推奨実装|
|---|---|
|ネット冗長|**リング or 二重ケーブル**。待機ラインは 1 サイクルで自動切替（≤ 200 µs）。Managing Node（MN）を二重化して **Redundant MN** 構成。 ([bihl-wiedemann.de](https://www.bihl-wiedemann.de/us/company/technological-foundations/bus-systems/powerlink?utm_source=chatgpt.com "POWERLINK Fieldbus system - Bihl+Wiedemann GmbH"), [ResearchGate](https://www.researchgate.net/profile/Denis-Genon-Catalot/publication/308821718_Performance_analysis_of_Ethernet_Powerlink_protocol_Application_to_a_new_lift_system_generation/links/57d7d9c108ae601b39af36e7/Performance-analysis-of-Ethernet-Powerlink-protocol-Application-to-a-new-lift-system-generation.pdf?origin=scientificContributions&utm_source=chatgpt.com "[PDF] Performance analysis of Ethernet Powerlink protocol - ResearchGate"))|
|CPU冗長|待機 MN が Current Cycle Counter を監視し、欠落検知で 1 周期遅れで制御開始。|
|安全設定|openSAFETY SN‑ID を主従 MN で共有。SPDO に 2× CRC16 + TimeStamp を付与。SafeDESIGNER で安全ロジックを生成し、両MNへ同一プロジェクトを書込。|
|タイミング|openSAFETY default Tcycle ≈ ネットワークサイクル。冗長切替えが1 周期 → Watchdog = > 3 × Tcycle で設定。|

---

## 5. CC‑Link IE TSN ＋ Safety

|項目|推奨実装|
|---|---|
|ネット冗長|**冗長ループ**＋TSN **FRER (IEEE 802.1CB)** でフレームを複製し異経路へ送信（無瞬断）。 ([iebmedia.com](https://iebmedia.com/technology/industrial-ethernet/taking-the-proven-benefits-of-tsn-to-the-real-world/?utm_source=chatgpt.com "Taking the proven benefits of TSN to the real world"), [automationworld.com](https://www.automationworld.com/communication/article/33004575/cc-link-ies-embrace-of-tsn-and-gigabit-ethernet?utm_source=chatgpt.com "CC-Link IE's Embrace of TSN and Gigabit Ethernet \| Automation World"), [ウィキペディア](https://en.wikipedia.org/wiki/Time-Sensitive_Networking?utm_source=chatgpt.com "Time-Sensitive Networking"))|
|CPU冗長|iQ‑R Safety CPU を二重化し、TSN の時分割領域内で同期。二重ケーブル構成によりネットとCPU冗長を同時実現。|
|安全設定|GX Works3 で **Safety ID & CRC** を設定し、Primary/Secondary 両CPUへ同一 Safety Parameter List をダウンロード。|
|タイミング|FRER の複製による帯域増分を考慮し、Time‑Aware Shaper で優先度7スロットを確保。安全通信 Cycle ≥ 31.25 µs。Watchdog はループ復旧無停止のため通常設定（100 ms前後）で可。|

---

## 6. プロジェクト運用 Tips

|フェーズ|具体的アクション|
|---|---|
|**設計初期**|・SIL/PL とシステム可用性（MTTR, MTBF）の両方をターゲットにした _Safety Concept_ を作成・冗長トポロジと安全 PLC/IO のサプライヤ選定を同時に行う|
|**エンジニアリング**|・エンジニアリングツール（TwinCAT, TIA Portal, Studio5000, SafeDESIGNER, GX Works3）で **一元管理プロジェクト** を作り、パラメータの差分を禁止・アプリ改版時は「署名解除 → 変更 → 再署名 → 冗長系へ同時DL」の手順を徹底|
|**検証**|・ケーブル断、スイッチ電源 OFF、マスター CPU STOP を網羅した _Fault Insertion Test_ を自動化・安全・冗長イベントを Syslog / OPC UA / MQTT でクラウドへ集約し、FMEA を容易化|
|**認証**|・ETG/PI/ODVA/CLPA のコンフォーマンステスト＋TÜV など第三者の SIL 監査を “同じハード構成” で受験し、設計変更を最小化|

---

### まとめ

> **「途切れない通信」** と **「誤動作しない安全」** を同時に狙うには、
> 
> 1. 各プロトコル固有の冗長機構を“安全アドレス・署名が不変のまま”組み込む
>     
> 2. ウォッチドッグと復旧時間のバランスをとる
>     
> 3. テストと診断を一体化する  
>     ことがカギです。
>     

ご計画中の装置構成や目標 SIL／可用性値に合わせて、詳細なパラメータ設定例や試験手順書の雛形を作成することも可能です。お気軽にご相談ください！