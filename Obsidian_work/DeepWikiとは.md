#ChatGPT #AI #GitHub 

```
質問：
DeepWikiとはどのようなWebサービスですか？
```

### DeepWiki とは？

DeepWiki は **GitHub などのリポジトリを自動解析し、対話可能な Wiki 形式のドキュメントを生成してくれる AI ドキュメンテーションサービス** です。開発元は AI ソフトウェアエージェント「Devin」で知られる **Cognition 社**。公開リポジトリなら無料で利用でき、リポジトリ URL のドメインを  
`github.com ➔ deepwiki.com` に置き換えるだけで、その場でドキュメントが生成・閲覧できます。([Cognition](https://cognition.ai/blog/deepwiki?utm_source=chatgpt.com "DeepWiki: AI docs for any repo - Cognition"), [Qiita](https://qiita.com/ikarosu_9/items/25781584d9c3e8ad4c81?utm_source=chatgpt.com "【AI】自分のGithubリポジトリをDeepWiki化してみた感想 - Qiita"))

---

#### 主な特徴

|機能|内容|
|---|---|
|コード自動解析|ディレクトリ構造・依存関係をスキャンし、モジュールごとに概要を生成。|
|アーキテクチャ図の自動作成|クラス関係やコールグラフを視覚化し、構造をひと目で把握。([Devin Docs](https://docs.devin.ai/work-with-devin/deepwiki?utm_source=chatgpt.com "DeepWiki - Devin Docs"))|
|ソースへの直接リンク|各説明ブロックに該当コード行へのハイライト付きリンクを付加。|
|自然言語 Q&A|生成された Wiki を踏まえてチャット形式で「この関数はどこで呼ばれる？」などを質問可能。|
|Dev in 連携|Devin の “Ask Devin” が Wiki をコンテキストとして用いるため、コード検索／修正指示が精度向上。([Devin Docs](https://docs.devin.ai/work-with-devin/deepwiki?utm_source=chatgpt.com "DeepWiki - Devin Docs"))|

---

#### 使い方（パブリックリポジトリ）

1. **URL を変換**  
    例）`https://github.com/user/project` → `https://deepwiki.com/user/project`
    
2. ブラウザで開くと数秒～数十秒で Wiki が生成される。
    
3. 画面左ペインに章立てされたドキュメント、右にコードビューと図が表示。
    
4. 上部のチャットバーで自然言語検索・質問が可能。
    

---

#### 料金プラン（2025年7月時点）

|プラン|対象リポジトリ|料金|備考|
|---|---|---|---|
|Free|パブリック|無料|生成回数・閲覧無制限|
|Pro|プライベート|リポジトリ単位の従量課金|解析結果は非公開|

※ OSS 版 **DeepWiki-Open** （MIT License）も GitHub で公開されており、自前サーバーで同様の機能をホスト可能。([Qiita](https://qiita.com/segur/items/7217d9dae66ba84ef747?utm_source=chatgpt.com "DeepWiki-Openを使ってたらOSSに貢献できた話 - Qiita"), [GitHub](https://github.com/AsyncFuncAI/deepwiki-open?utm_source=chatgpt.com "AsyncFuncAI/deepwiki-open: Open Source DeepWiki: AI ... - GitHub"))

---

#### メリット

- **学習コスト削減**：初見の大規模コードでも数分で全体像を把握。
    
- **検索より速い**：grep や GitHub Search に比べ、関連部分を自然文で探せる。
    
- **ドキュメントの鮮度維持**：コード最新版を基に都度生成するため “ドキュメントが古い” 問題を回避。
    
- **証跡付き説明**：説明文の右側に “出典コード行” が明示されるため誤読を防止。([Qiita](https://qiita.com/ikarosu_9/items/25781584d9c3e8ad4c81?utm_source=chatgpt.com "【AI】自分のGithubリポジトリをDeepWiki化してみた感想 - Qiita"))
    

---

#### 留意点・制限

- **プライベート解析は有料**。CI 連携など本格利用には課金が必要。
    
- **巨大リポジトリは時間がかかる**：解析フェーズが数分以上になる場合がある。
    
- **サポート言語は主要言語中心**：マイナー言語や一部生成ファイルはスキップされることがある。
    
- **機密コードの扱い**：外部サービスに送信されるため、 NDA・社内ポリシー確認が必須。
    

---

### まとめ

DeepWiki は「**リードタイムを大幅に短縮する、AI ベースのコード理解プラットフォーム**」。公開リポジトリならリンクを書き換えるだけで利用でき、アーキテクチャ図・リンク付きドキュメント・対話型検索が一体となった開発者向けサービスです。プライベート案件では有料ですが、OSS 版やセルフホスト案もあるため、用途とセキュリティ要件に応じて選択できます。
