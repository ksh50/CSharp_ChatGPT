#ChatGPT #電気回路

```
質問：
Webベース回路図CADとして、「Upverter」というWebサービスがあることを知りました。
このサービスがどのようなものであるか、似たような競合サービスやアプリがあるかについて調査していただけますか？
```

### 1. Upverterとは？

- **クラウド完結の回路・基板CAD**  
    ブラウザ上で回路図作成、PCBレイアウト、オートルーティング、3Dプレビューまで行い、そのままガーバーデータやBSPを出力して製造発注に進める “モジュラー設計” プラットフォームです。([upverter.com](https://upverter.com/ "UPVERTER - Modular Design"))
    
- **ドラッグ＆ドロップ＋テンプレート**  
    IoTモジュールなど数百種類のテンプレートを呼び出し、ブロックを組み合わせる感覚で基板を構築できます。初心者でも部品配置や配線規則を気にせず形にしやすいのが特徴です。([upverter.com](https://upverter.com/ "UPVERTER - Modular Design"))
    
- **製造・物流までワンストップ**  
    完成したデータは Upverter 画面から直接 PCB 試作を依頼でき、量産フローやソフトウェア書き込みもサポート（Altium 系サービスと連携）。([upverter.com](https://upverter.com/ "UPVERTER - Modular Design"))
    
- **背景**  
    2017 年に Altium が買収し、Altium 365 との連携や教育版（Upverter Education）を展開しています。([BetaKit](https://betakit.com/altium-acquires-toronto-based-product-design-platform-upverter/?utm_source=chatgpt.com "Altium acquires Toronto-based product design platform Upverter"))
    

> **向いているユーザー像**  
> _IoT/プロトタイプを素早く形にしたいスタートアップや教育機関。伝統的 CAD の細かな DRC や高層多層設計を追い込みたいパワーユーザーには機能が物足りない場合があります。_

---

### 2. 主な競合Webサービス（機能別に整理）

|カテゴリ|サービス|概要／強み|注意点|
|---|---|---|---|
|**フル機能PCB設計 ＋ 製造連携**|**EasyEDA**|クラウド&デスクトップ両対応。500枚超のシート、10 万パッド規模まで設計可。LCSC部品とJLCPCB製造がワンクリックで連動。チーム管理・PLM連携も備える。([easyeda.com](https://easyeda.com/ "EasyEDA - Online PCB design & circuit simulator"))|中国系サービスへのデータ保管ポリシーを要確認。有料プランでないと非公開プロジェクト数が制限される。|
||**Autodesk Fusion Electronics**|3D CAD/CAM/CAE と統合された PCB 環境。機械設計チームと同一クラウド上で共同編集・バージョン管理が可能。個人向けは無料ライセンスあり。([オートデスク](https://www.autodesk.com/ca-en/products/fusion-360/personal "Compare Autodesk Fusion vs Autodesk Fusion for Personal Use \| Autodesk"))|本格利用には有料サブスクリプション。ブラウザ単体ではなくデスクトップアプリ＋クラウド同期型。|
||**Altium 365 Viewer**|ブラウザで Altium Designer データを閲覧・コメント可能。顧客／製造部門とのレビュー用に便利。([Altium](https://www.altium365.com/viewer/?utm_source=chatgpt.com "Online PCB Viewer to Visualize and Share Electronics Design Projects"))|編集は Altium Designer (デスクトップ) が必須。|
|**回路図＋シミュレーション主体**|**CircuitLab**|SPICE相当のアナログ/デジタル混在シミュレーションをワンクリックで実行。教育用テキストやQ&Aコミュニティも併設。([CircuitLab](https://www.circuitlab.com/ "Online circuit simulator & schematic editor - CircuitLab"))|PCBレイアウト機能はなく、製造データは出力不可。|
||**Tinkercad Circuits**|Arduino／micro:bit を含む回路をGUI-ブレッドボードで組み、コードを書いて動作を疑似体験できる学習向けツール。([Instructables](https://www.instructables.com/How-to-Teach-an-Online-Circuits-Class-With-Tinkerc/?utm_source=chatgpt.com "How to Teach an Online Circuits Class With Tinkercad - Instructables"))|部品ライブラリが教育用中心。実務レベルの回路図・基板出力はできない。|
|**回路図＋BOM／資料作成特化**|**Scheme-it (Digi-Key)**|電子部品カタログと連携した回路図/ブロック図作成ツール。KiCad用ライブラリやPDF/PNG/SVGにエクスポート可、BOMをそのままDigi-Keyで調達可能。([DigiKey](https://www.digikey.com/en/schemeit/project "Scheme-it \| Free Online Schematic and Diagramming Tool \| DigiKey"))|シミュレーション・PCBレイアウト機能なし。|

---

### 3. Upverterと他サービスの選定ポイント

|観点|Upverter|EasyEDA|Fusion Electronics|CircuitLab|Scheme-it|
|---|---|---|---|---|---|
|**設計範囲**|回路＋PCB＋製造|回路＋PCB＋製造|回路＋PCB＋機械設計|回路＋シミュレータ|回路図|
|**コラボレーション**|ブラウザ共有、フォーラム|クラウドPJ/チーム権限|クラウド版管・コメント|共有URL|リンク共有・BOM発注|
|**テンプレート/ライブラリ**|IoTモジュール多数|数百万部品＋自動作図|Fusion＋EAGLE資産|汎用SPICEモデル|Digi-Key部品|
|**学習コスト**|低（ドラッグ中心）|中|中〜高|低|低|
|**費用**|基本無料（製造時課金）|無料〜企業向け有料|個人無料／商用有料|有料会員で高機能|無料|

---

### 4. まとめと活用アドバイス

1. **プロトタイピングを素早く回して製造まで一気通貫したい** → Upverter か EasyEDA
    
2. **機械筐体との干渉チェックを同一モデルで行いたい** → Fusion Electronics
    
3. **回路理論の学習や事前動作確認をブラウザで済ませたい** → CircuitLab／Tinkercad Circuits
    
4. **購買部と連携しながらBOMを整備したい、資料作成が中心** → Scheme-it
    

「まずオンラインで始め、必要に応じてデスクトップEDAへ移行する」というハイブリッド運用も一般的です。プロジェクト規模・品質要件・チーム構成・データ管理ポリシー（特に国際取引や機密設計）を踏まえて選択すると良いでしょう。
