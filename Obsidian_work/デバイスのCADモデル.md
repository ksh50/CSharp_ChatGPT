#ChatGPT #部品 #CAD

```
質問：
東芝デバイス＆ストレージのHPでTLP291(SEというフォトカプラについて調べていて、Ultra Librarian CADモデル、SamacSys CADモデルへのリンクを見つけました。

Ultra Librarian CADモデル、SamacSys CADモデルとは具体的にどのようなものですか？
文脈的に特定のフォーマットのCADデータモデルのように認識してます。

Ultra Librarian CADモデル、SamacSys CADモデルについて詳しく教えてください。

また、他にも似たような〇〇モデルがあればご紹介お願いします。
```

### そもそも「CADモデル」とは

部品メーカが公開する **CADモデル** とは、多くの場合つぎの 3 点セットを指します。

|種類|役割|主なファイル形式|
|---|---|---|
|**回路図記号（Schematic Symbol）**|回路図エディタ上で部品を配置する際のアイコン|_.schlib_, _.dra_ など|
|**フットプリント（PCB Footprint）**|プリント基板上でランドやシルクを定義するパターン|_.pcbdoc_, _.lbr_, _.kicad_mod_ など|
|**3D モデル**|実装高さや干渉チェック用の立体形状|STEP (*.stp / _.step_) が事実上の標準|

これらを“手書き”すると工数も信頼性も厳しいため、近年は専門サービスが IPC-7351 準拠データを大量に整備し、各 CAD ツール向けにワンクリックで供給する仕組みが主流になっています。  
TLP291(SE) のデータとして東芝が用意している **Ultra Librarian** と **SamacSys** もその代表格です。

---

## Ultra Librarian CADモデルとは

- **運営元:** 米 EMA Design Automation
    
- **形式:** 独自の “.bxl”（Binary XML）ファイル 1 つに記号・フットプリント・3D を封入し、サイト上で Altium、KiCad、Eagle など **20 以上の CAD フォーマット**へオンデマンド変換してダウンロード ([ultralibrarian.com](https://www.ultralibrarian.com/?utm_source=chatgpt.com "Ultra Librarian: Free Online PCB CAD Library"))
    
- **品質基準:** IPC-7351 に基づき、アセンブリ層・部品配置境界・テストプローブ領域など細かいメタデータを保持 ([ultralibrarian.com](https://www.ultralibrarian.com/about/standards?utm_source=chatgpt.com "Top PCB Component Library & Database | Ultra Librarian's Standards"))
    
- **付加価値:** 一部部品にはシミュレーションモデルやリファレンスデザインも添付 ([flowcad.com](https://www.flowcad.com/en/ultra-librarian.htm?utm_source=chatgpt.com "Ultra Librarian - FlowCAD"))
    
- **ワークフロー:**
    
    1. Ultra Librarian サイトかメーカ直リンクで _.bxl_ を取得
        
    2. 同サイトのオンライン・コンバータ、または無償の **Ultra Librarian Reader** を使って自分の EDA 形式に変換
        
    3. 生成されたライブラリ／STEP ファイルを設計に取り込む
        

> **ポイント**
> 
> - _.bxl_ さえ残しておけば CAD を乗り換えた際にも再エクスポートできる。
>     
> - 変換エンジンはクラウドなので、最新 EDA バージョンへの追従が比較的早い。
>     

---

## SamacSys CADモデルとは

- **運営元:** 英 SamacSys（2023 年に SupplyFrame 傘下）
    
- **形式:** モデル自体は各 CAD ネイティブ形式で提供し、ダウンロードには小さな常駐アプリ **Library Loader** を介す方式
    
- **機能:**
    
    - 検索ポータル **Component Search Engine** で **1500 万点超** の部品を検索し、記号・フットプリント・3D を一括取得 ([samacsys.com](https://www.samacsys.com/pcb-part-libraries/?utm_source=chatgpt.com "FREE Schematic Symbols & PCB Footprints - PCB Libraries - 3D"), [samacsys.com](https://www.samacsys.com/?utm_source=chatgpt.com "SamacSys: Home"))
        
    - Library Loader が CAD を自動検出し、ダウンロード後に **ワンクリックでライブラリ登録** や PCB への自動配置まで行う
        
    - KiCad では「モデルをDL → Place → Add Symbol」とシームレスに利用 ([samacsys.com](https://www.samacsys.com/kicad/?utm_source=chatgpt.com "KiCad Symbols | Footprints | 3D Models - SamacSys"))
        
- **品質基準:** こちらも IPC-7351 ベース。ユーザがリクエストした新部品は通常 24 時間以内に作成（有償リクエストも可）。
    

> **ポイント**
> 
> - 変換ステップが不要で“押すだけ”で設計に入るため、操作がよりエンドユーザ寄り。
>     
> - 社内ライブラリ運用の都合でネイティブファイルが欲しい場合に向く。
>     

---

## Ultra Librarian と SamacSys の主な違い

|観点|Ultra Librarian|SamacSys|
|---|---|---|
|**ダウンロード形態**|_.bxl_ を取得し後変換|直接ネイティブ形式を取得|
|**変換ツール**|クラウド or Reader|Library Loader（常駐）|
|**オフライン対応**|_.bxl_ を保持すれば可|初回はネット必須|
|**追加サービス**|TI 等と連携しシミュレーションモデルも配布|部品リクエストが早い／無料枠多い|
|**対応 CAD 数**|20 + フォーマット|15 + フォーマット（KiCad, Altium, Eagle…）|

どちらを選ぶかは、

- **社内の CAD ツール構成**
    
- **ネット接続可否やセキュリティポリシー**
    
- **将来の CAD 乗り換え可能性**  
    などで決めると良いでしょう。
    

---

## 似たような「〇〇モデル」サービス

|サービス|特徴|参考|
|---|---|---|
|**SnapEDA（SnapMagic Search）**|無償ダウンロード＋クラウド部品リクエスト。部品ページごとに 3D プレビュー。PLM 連携や価格情報も表示 ([snapeda.com](https://www.snapeda.com/?utm_source=chatgpt.com "SnapMagic Search \| Free PCB Footprints and Schematic Symbols"), [snapeda.com](https://www.snapeda.com/home?utm_source=chatgpt.com "Explore SnapMagic Search's Symbol & Footprint Libraries"))|部品点数：約 15 M|
|**PCB Libraries – Footprint Expert**|ローカルアプリで IPC 設定を細かくカスタマイズし、自社仕様のフットプリントを “自動生成” する有償ツール ([pcblibraries.com](https://www.pcblibraries.com/FootprintExpert/?utm_source=chatgpt.com "PCB Footprint Expert - PCBL"), [pcblibraries.com](https://www.pcblibraries.com/?utm_source=chatgpt.com "PCB Libraries"))|複数 CAD へ一括出力|
|**PartQuest (Siemens)**|Xpedition／PADS 向けの部品モデル＋部品情報連携。Mentor系ユーザ向け||
|**Digi-Key KiCad Library**|部品商社発の公式 KiCad ライブラリ。部品在庫と紐づき更新が早い||
|**Mouser ECAD Models**|Mouser サイトから Ultra Librarian 形式で取得できる||

---

### まとめ

- **Ultra Librarian CADモデル** は「*.bxl 1 つでマルチ CAD 出力が可能」という“汎用性重視”の配布形式。
    
- **SamacSys CADモデル** は **Library Loader** による“即時取り込み”体験が売りで、ネイティブ形式を直接得られる。
    
- いずれも **回路図記号・PCB フットプリント・3D STEP** を含み、IPC-7351 規格に沿った実装設計がすぐに開始できる。
    
- 他にも SnapEDA や PCB Footprint Expert など、用途・ワークフローに応じた選択肢が存在する。
    

設計フローや社内規定に合わせて、これらのライブラリサービスを使い分けることでライブラリ作成の手間を大幅に削減できます。


[[KiCADで使用可能なモデル]]