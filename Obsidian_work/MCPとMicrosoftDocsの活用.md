#ChatGPT #AI #Microsoft 

```
質問：
下記ページを見るとMicrosoft Docs内容を検索・要約できるように見えました。
もともとMCPとはどのようなもので、そのうちのどのような特性を使って上記機能を実現しているのか、詳細を知りたいです。

https://zenn.dev/thdy/articles/microsoft-docs-mcp
https://qiita.com/fukakimono/items/46c368c61603d98baadd
```

### そもそも MCP（Model Context Protocol）とは？

|観点|内容|
|---|---|
|**目的**|LLM／AI エージェントが外部のデータソースや API、ツールを**安全かつ標準化された方法で呼び出す**ためのオープンプロトコル。USB-C が機器接続を統一したように、MCP は AI への“接続ポート”を統一することを目指す([modelcontextprotocol.io](https://modelcontextprotocol.io/introduction "Introduction - Model Context Protocol"))|
|**通信方式**|JSON-RPC 2.0 をベースにした**クライアント-サーバ型**。・**Host** (IDE や Copilot など)・**Client** (Host 内の通信ライブラリ)・**Server** (データ／ツールを公開するプロセス) という 3 役に分かれる([modelcontextprotocol.io](https://modelcontextprotocol.io/docs/concepts/architecture "Core architecture - Model Context Protocol"))|
|**輸送層**|標準入出力 (stdio) または **Streamable HTTP**。後者は HTTPS POST＋Server-Sent Events でストリーミング応答を返せる([modelcontextprotocol.io](https://modelcontextprotocol.io/docs/concepts/architecture "Core architecture - Model Context Protocol"))|
|**プロトコル・メッセージ**|`initialize` ➜ `initialized` でハンドシェイク後、• Request/Result • Notification • Error をやり取りし、双方向にツール呼び出しやリソース取得を行う([modelcontextprotocol.io](https://modelcontextprotocol.io/docs/concepts/architecture "Core architecture - Model Context Protocol"))|
|**中核プリミティブ**|1. **Resources** : LLM が「読み込む内容」(ファイル、API 応答、記事など) を URI 付きで提供する仕組み([modelcontextprotocol.io](https://modelcontextprotocol.io/docs/concepts/resources "Resources - Model Context Protocol"))2. **Tools** : LLM が「実行する機能」を JSON Schema で型定義し、`tools/call` で呼び出せる仕組み([modelcontextprotocol.io](https://modelcontextprotocol.io/docs/concepts/tools "Tools - Model Context Protocol"))3. **Prompts** : 定型プロンプトのテンプレート共有 (Microsoft Docs MCP では未使用)|

---

### Microsoft Docs MCP Server が利用している MCP の特性

|MCP の機能|具体的な使い方|
|---|---|
|**Streamable HTTP transport**|VS Code/Copilot 等のクライアントから `https://learn.microsoft.com/api/mcp` へ接続。ブラウザから直接叩くと 405 が返るのはこのため([github.com](https://github.com/MicrosoftDocs/mcp "GitHub - MicrosoftDocs/mcp"))|
|**Tools**|サーバーが **`microsoft_docs_search`** という 1 つのツールを公開。入力: `query` 文字列のみ。LLM がこのツールを呼ぶと内部でベクトル検索を実行し、最適なドキュメントを取得([github.com](https://github.com/MicrosoftDocs/mcp "GitHub - MicrosoftDocs/mcp"))|
|**Resources**|検索結果は **最大 10 件、各 500 token 以下**のチャンク (タイトル・URL・抜粋テキスト) として Resources にパッケージされ、LLM 側が `resources/read` で読み取って回答の根拠に利用([github.com](https://github.com/MicrosoftDocs/mcp "GitHub - MicrosoftDocs/mcp"))|
|**リアルタイム更新**|Microsoft Learn 側で新しい記事が公開されるとインデクスが更新され、直ちに検索対象に含まれる (サーバー側の自動クローラ＋ベクトル再計算)([github.com](https://github.com/MicrosoftDocs/mcp "GitHub - MicrosoftDocs/mcp"))|
|**最小限の設定**|VS Code なら `settings.json` に`"mcp.servers.microsoft.docs.mcp": {"type":"http","url":"https://learn.microsoft.com/api/mcp"}` を書くだけで Host 側に登録される([zenn.dev](https://zenn.dev/thdy/articles/microsoft-docs-mcp "Microsoft Docs 公式 MCP サーバーを使って Microsoft のドキュメントを読みやすくする"))|

> **実際の流れ（Copilot 例）**
> 
> 1. ユーザーが「`docs_search: Azure App Service の価格プラン P1V3 の特徴を教えて`」とチャットで依頼
>     
> 2. Copilot は `tools/list` で `microsoft_docs_search` を発見
>     
> 3. `tools/call` で ①のクエリを送信
>     
> 4. サーバーが最適な記事チャンク (App Service の料金ページなど) を Resources として返却
>     
> 5. Copilot が `resources/read` で本文を取得し、要約＋URL を生成して回答
>     

---

### なぜ MCP が「検索＋要約」に向いているのか

1. **LLM が自律的にツール選択**  
    `microsoft_docs_search` という 1 つの Tool だけを公開しておけば、LLM は「検索すべき」と判断したときだけ呼び出す。
    
2. **型安全な引数 & 出力**  
    JSON Schema で `query` が必須文字列であることを宣言でき、LLM に誤使用を抑制させる([modelcontextprotocol.io](https://modelcontextprotocol.io/docs/concepts/tools "Tools - Model Context Protocol"))。
    
3. **根拠リンクを自動で添付**  
    Resource に URL フィールドを含める設計により、LLM は回答と同時に引用リンクを提示できる (Qiita 記事でもこの点が評価) ([qiita.com](https://qiita.com/fukakimono/items/46c368c61603d98baadd "〖超簡単〗MicrosoftDocsのMCPを使ってみる #Azure - Qiita"))。
    
4. **接続先を簡単に差し替え**  
    Host 側の設定を 1 行変えるだけで、Docs 以外の MCP Server (社内 Wiki など) にも切り替え可能。
    

---

### まとめ

- **MCP は「LLM ↔ 外部世界」の USB-C**。JSON-RPC 上で _Resources_ と _Tools_ を標準化し、ホスト／クライアント／サーバーをゆるく結合するプロトコル。
    
- **Microsoft Docs MCP Server** はその実装例で、
    
    - _Tools_ で「ドキュメント検索」機能を公開し、
        
    - _Resources_ で検索結果チャンクを提供。  
        これにより Copilot や Cursor が **公式ドキュメントを根拠付きで検索・要約**できる。
        
- 設定は URL を 1 行追加するだけ。社内文書や別 API への応用も同じ枠組みで拡張できるため、今後の **企業内ナレッジ統合**にも有望です。