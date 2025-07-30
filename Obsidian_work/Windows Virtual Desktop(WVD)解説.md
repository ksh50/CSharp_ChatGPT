#ChatGPT #Windows 

```
質問：
Windows Virtual Desktop、Azure Virtual Desktopについて解説してください。
```

### 1. Windows Virtual Desktop とは？

- **Windows Virtual Desktop (WVD)** は、2019 年に登場した _Azure 上の仮想デスクトップ／アプリ配信サービス_ です。
    
- Microsoft がマネージドで提供する **接続ブローカー・ゲートウェイ・診断** などの制御プレーンに、ユーザー企業が用意した **Azure VM（セッション ホスト）** を登録して使います。
    
- 最大の特長は **Windows 10/11 Enterprise マルチセッション** – 1 台のクライアント OS VM を複数ユーザーで共有でき、従来の VDI よりコストを抑えられました。([Microsoft Learn](https://learn.microsoft.com/en-us/azure/virtual-desktop/windows-multisession-faq?utm_source=chatgpt.com "Windows Enterprise multi-session FAQ - Azure | Microsoft Learn"))
    

### 2. Azure Virtual Desktop への改称と位置づけ

- 2021 年 6 月、Microsoft は **サービス名を Windows Virtual Desktop から Azure Virtual Desktop (AVD) に変更** し、アプリ ストリーミングやハイブリッドワークなどより広いユースケースを掲げました。([Microsoft Azure](https://azure.microsoft.com/en-us/blog/azure-virtual-desktop-the-desktop-and-app-virtualization-platform-for-the-hybrid-workplace/?utm_source=chatgpt.com "Azure Virtual Desktop: The flexible cloud VDI platform for the hybrid ..."))
    
- 以後も Teams の AV リダイレクト、**Windows Server 2025 セッションホスト対応**、Intune からのマルチセッション管理など機能拡張が継続しています。([Microsoft Learn](https://learn.microsoft.com/en-us/azure/virtual-desktop/whats-new?utm_source=chatgpt.com "What's new in Azure Virtual Desktop? - Learn Microsoft"), [Microsoft Learn](https://learn.microsoft.com/en-us/intune/intune-service/fundamentals/azure-virtual-desktop-multi-session?utm_source=chatgpt.com "Using Azure Virtual Desktop multi-session with Microsoft Intune"))
    

### 3. アーキテクチャの要素

|役割|概要|補足|
|---|---|---|
|**セッションホスト (VM)**|ユーザーが接続する Windows 11/10 (E) または Windows Server|GPU VM やカスタムイメージも可|
|**ホストプール**|同一構成の VM 群を論理的に束ねる単位|「個人」/「プール」割当が選択可|
|**接続ブローカー & ゲートウェイ**|マイクロソフト管理の PaaS。接続調停と RDP over HTTPS トンネリングを提供|リージョン冗長で SLA 99.9 %|
|([Microsoft Learn](https://learn.microsoft.com/en-us/azure/virtual-desktop/service-architecture-resilience?utm_source=chatgpt.com "Azure Virtual Desktop service architecture and resilience"), [Microsoft Learn](https://learn.microsoft.com/en-us/azure/architecture/example-scenario/azure-virtual-desktop/azure-virtual-desktop?utm_source=chatgpt.com "Azure Virtual Desktop for the enterprise - Learn Microsoft"))|||

### 4. 主な機能ハイライト（2025 年時点）

- **Windows 11 Enterprise マルチセッション** に正式対応。
    
- **Azure AD Join / Entra ID** によるパスワードレス & 条件付きアクセス。
    
- **Intune によるマルチセッション端末管理**（ポリシー、アプリ配布、アップデート）。([Microsoft Learn](https://learn.microsoft.com/en-us/intune/intune-service/fundamentals/azure-virtual-desktop-multi-session?utm_source=chatgpt.com "Using Azure Virtual Desktop multi-session with Microsoft Intune"))
    
- **FSLogix** でユーザープロファイルをコンテナ化し高速 Roaming。
    
- **Autoscale** でアイドル VM の自動停止、コスト最適化。
    
- **Teams、Zoom、WebRTC のメディア最適化**（クライアント側リダイレクト）。
    

### 5. ライセンス & 料金モデル

|シナリオ|必要ライセンス|Azure 従量課金|
|---|---|---|
|**社内利用 (内部ユーザー)**|Microsoft 365 E3/E5/Business Premium などクライアント OS アクセス権を含むサブスク|VM・ストレージ・ネットワーク使用量|
|**外部商用配信 (SaaS/App Streaming)**|**AVD Per‑User Access Pricing** に登録（1 ユーザー月額）|同左|
|([Microsoft Learn](https://learn.microsoft.com/en-us/azure/virtual-desktop/licensing?utm_source=chatgpt.com "Licensing Azure Virtual Desktop \| Microsoft Learn"))|||

### 6. Windows 365 との違い

|項目|**Azure Virtual Desktop**|**Windows 365 (Cloud PC)**|
|---|---|---|
|インフラ管理|VM サイズ・イメージ・スケールを **管理者が自由設計**|Microsoft 完全管理（サイズ変更はプラン変更）|
|課金|Azure VM 従量 + ライセンス　→　**使った分だけ**|**ユーザー単価固定** 月額|
|マルチセッション|あり (Win 11/10 E multi‑session)|なし（常に 1 ユーザー:1 VM）|
|カスタマイズ/拡張|ネットワーク・GPU・自動化スクリプト等 **高自由度**|制約あり・簡便性重視|
|Windows 365 は “PC as a Service” に近く、AVD は **フルコントロール型 VDI** に位置づけられます。([Nerdio](https://getnerdio.com/blog/windows-365-vs-azure-virtual-desktop-avd-comparing-two-daas-products/?utm_source=chatgpt.com "Windows 365 vs. Azure (Windows) Virtual Desktop - Nerdio"))|||

### 7. 代表的な導入ステップ

1. **前提確認** – Azure サブスクリプション、Entra ID、ネットワーク帯域、ライセンスを準備。([Microsoft Learn](https://learn.microsoft.com/en-us/azure/virtual-desktop/prerequisites?utm_source=chatgpt.com "Prerequisites for Azure Virtual Desktop | Microsoft Learn"))
    
2. **ホストプール作成** – 目的別（個人 / マルチセッション）に VM イメージとサイズを選択。
    
3. **FSLogix プロファイル共有** – Azure Files / NetApp Files を構築。
    
4. **アプリ & ポリシー配信** – Intune / Group Policy / Configuration Manager。
    
5. **接続 & エクスペリエンス最適化** – Teams 最適化、RDP Shortpath、条件付きアクセス。
    
6. **運用** – Azure Monitor + Log Analytics でパフォーマンス & コスト監視、Autoscale 設定。
    

---

💡 **まとめ**

- **Windows Virtual Desktop** は Azure PaaS で提供される **本格 VDI 基盤**として始まり、現在は **Azure Virtual Desktop** として機能拡張を続けています。
    
- **マルチセッション Windows、PaaS 制御プレーン、従量課金** が最大の強み。
    
- 構成自由度が高い反面、スケール設計や運用監視はユーザー側の責任。
    
- 手離れの良い固定料金型を求める場合は **Windows 365**、カスタム要件やコスト最適化を追求する場合は **AVD** が適しています。


[[Windows365とは]]