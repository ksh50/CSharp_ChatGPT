#ChatGPT #Microsoft

```
質問：
Microsoftの戦略概念として「one windows」があると聞きました。これは具体的にどのような概念ですか？
```

### 「One Windows」とは ── “Windows を 1 つのプラットフォームにまとめる” というマイクロソフトの統合コンセプト

|視点|要約|
|---|---|
|**狙い**|PC・タブレット・スマートフォン・Xbox・HoloLens・IoT など **あらゆるフォームファクタで同じ Windows コアとアプリ実行環境を共有** し、開発・運用・ユーザー体験を統一する。|
|**キーワード**|OneCore（共通カーネル）、UWP / Windows Apps SDK（共通アプリモデル）、One Store、共通ドライバー/更新基盤、Windows as a Service。|
|**時系列**|2013 年「One Microsoft」組織再編 → 2014 年 Build で“Universal Windows Apps”発表 → 2015 年 Windows 10 リリースで **One Windows** を初めて製品化 → 2017 年以降 Windows Core OS / CorePC へ発展（次世代 Windows 12 計画など）。|

---

## 1. 背景：組織横断の「One Microsoft」から OS 統合の「One Windows」へ

- **2013 年 7 月**──スティーブ・バルマー期に “One Microsoft” 再編を実施。従来の「Windows 部門」「Office 部門」といった縦割りを解消し、横串で製品を開発できる体制へ転換。([マイナビニュース](https://news.mynavi.jp/article/20130712-a068/?utm_source=chatgpt.com "「One Microsoft」へ、マイクロソフトが大規模な組織再編"))
    
- その技術面の中核が **「One Windows」**。社内に複数あった Windows（デスクトップ、Phone、Xbox、Embedded など）のコードベースを **OneCore** と呼ぶ単一カーネルへ統合し、「アプリもストアも 1 つ」という世界を目指した。
    

## 2. 主要コンポーネント

|コンポーネント|役割|
|---|---|
|**OneCore**|NT カーネル／ドライバー／セキュリティモデルを共通化。|
|**Universal Windows Platform（UWP） / Windows Apps SDK**|1 つの API セットでマルチデバイス対応アプリを開発。Build 2014 で初公表。([InfoWorld](https://www.infoworld.com/article/2199532/one-windows-all-devices-the-new-microsoft-app-strategy-unveiled.html "One Windows, all devices: The new Microsoft app strategy unveiled \| InfoWorld"))|
|**One Store**|PC から Xbox・HoloLens まで同一パッケージで配信。|
|**Windows as a Service**|半期／年次で機能更新し、全デバイスを同じビルド系列で保守。|
|**共通ライセンシング**|9 インチ未満デバイスへの OS 無償提供など、ビジネスモデルも統一。([Thurrott.com](https://www.thurrott.com/office/2321/microsofts-new-strategy-for-office-freemium "Microsoft's New Strategy for Office: Freemium - Thurrott.com"))|

## 3. 実装例（Windows 10 世代）

1. **Windows 10 (2015)**
    
    - 公式に「**1 つの Windows（One Windows）**」を掲げ、PC から Xbox・HoloLens までを同一 OS ファミリとして出荷。([ウィキペディア](https://zh.wikipedia.org/wiki/Windows_10 "Windows 10 - 维基百科，自由的百科全书"))
        
    - Continuum／Adaptive UI で画面サイズに応じた自動レイアウト。
        
    - ドライバーは Windows 10 Mobile と PC で共通 WHQL に移行。
        
2. **開発者メリット**
    
    - Visual Studio で 1 つのソリューションから Phone・PC・Xbox 向けを同時ビルド。
        
    - ストア登録もパッケージ共有でマーケットを横断。
        

## 4. 進化形：Windows Core OS／CorePC（モジュール化）

- UWP だけではカバーできなかった **軽量／モジュール化** を再設計したのが
    
    - **Windows Core OS（WCOS）**…10X, HoloLens 2, Surface Hub 2 S などで採用を目指したが正式版は見送り。
        
    - **CorePC**（開発コード名, 2024–2025）…既存デスクトップ互換を保ったまま “読み取り専用パーティション” 化・AI 機能統合などを進行中。([Windows Central](https://www.windowscentral.com/software-apps/windows-11/microsoft-windows-corepc-modern-platform-hudson-valley-2024 "Microsoft plans major platform upgrades for Windows that will modernize the OS with AI, faster updates, and better security | Windows Central"))
        
- これらは “**One Windows 2.0**” とも呼ばれ、_次期 Windows 12_ での採用が噂されている。
    

## 5. 成果と課題

|成果|課題／残った分断|
|---|---|
|• デバイス横断のカーネル統合は概ね完了し、ドライバー開発コストが低減。• UWP → Windows Apps SDK に進化し、Win32／.NET とのブリッジが改善。|• スマホ（Windows Phone）撤退で「完全に同一アプリが走る市場」が縮小。• 組込・教育向けの超軽量版需要に応えるため、CorePC など _新たな枝分かれ_ が再び発生。• Win32 資産が巨大なため、“真の One Windows” にはレガシー互換レイヤーが不可欠。|

---

## 6. 「One Windows」をどう捉えればよいか

1. **ブランド／エコシステムの一本化**
    
    - ユーザー視点：どのデバイスでも「同じ Windows 体験」。
        
    - 開発者視点：API・IDE・ストアを一本化して投資回収を容易に。
        
2. **クラウド＆AI 時代への布石**
    
    - Windows 365 (Cloud PC) など、“ローカルでもクラウドでも同じ Windows” へ拡張。
        
    - Copilot/AI PC に合わせた **CorePC** によって、セキュアかつ高速アップデートを実現する“モジュラー OS”へ発展中。
        

> **まとめ**  
> **「One Windows」は“Windows を複数製品の集合体から単一プラットフォームへ昇華させる”長期ビジョン**です。Windows 10 でハード面の統合をほぼ達成し、現在は CorePC／AI 連携で“クラウド・モジュール化”という次の段階へ進んでいます。開発者・IT 管理者にとっては、**API・更新・ライセンスの統一**が最大のメリット。今後の Windows 12 世代では、より軽量で安全な“次の One Windows”を体験できる見込みです。

