#ChatGPT #Windows #時短

右クリックメニューの拡張機能に追加する機能で、テンプレートからのファイル作成を支援。時短効果。

```
依頼：
下記ページにNew+というPowerToysの機能が記載されています。
https://learn.microsoft.com/ja-jp/windows/powertoys/newplus

この機能の使い所が分からないのですが、どのような使い方が想定されるでしょうか？
どのようの機能なのかについて教えてください。
また、使い方の提案を紹介いただけないでしょうか？
```

### New+ とは何か

New+ は PowerToys v0.85 で追加された File Explorer の右クリックメニュー拡張です。  
「**新規作成 (New)」メニューの横に “New+” が現れ、そこに置いた **テンプレート**（ファイル／フォルダー／ショートカット）を 1 クリックで複製できます。わざわざアプリを開いて「名前を付けて保存」したり、既存フォルダーをコピー＆リネームしたりする手間をまとめて省くことが狙いです。([Microsoft Learn](https://learn.microsoft.com/ja-jp/windows/powertoys/newplus "PowerToys 用 New+Windows | Microsoft Learn"))

---

## 基本の使い方

|手順|操作|
|---|---|
|1|**PowerToys 設定 → New+** をオンにする|
|2|同じページの **「テンプレートを開く」** ボタンを押し、|
|‎`%LOCALAPPDATA%\Microsoft\PowerToys\NewPlus\Templates` フォルダーを開く||
|3|そこに自由にファイル／フォルダー／.lnk を配置・名前変更する|
|（サブフォルダーも可。並び順を 01_, 02_… などで制御可能）||
|4|エクスプローラーで目的の場所を右クリック → **New+** → テンプレートを選択|

> **ヒント**: テンプレートフォルダーを OneDrive 等と同期フォルダーに変更すれば PC 間で同じテンプレートを共有できます。([Microsoft Learn](https://learn.microsoft.com/ja-jp/windows/powertoys/newplus "PowerToys 用 New+Windows | Microsoft Learn"))

---

## ファイル名を自動で置き換える「変数」

テンプレート名に下記の変数を含めておくと、複製時に自動展開されます。  
例: `note_$YYYY-$MM-$DD.md` → `note_2025-05-30.md`([Microsoft Learn](https://learn.microsoft.com/ja-jp/windows/powertoys/newplus "PowerToys 用 New+Windows | Microsoft Learn"))

|種類|主な変数|展開例|
|---|---|---|
|日付|`$YYYY, $MM, $DD`|2025, 05, 30|
|時刻|`$hh, $mm, $ss`|07, 15, 02|
|特殊|`$PARENT_FOLDER_NAME`|親フォルダー名|
|環境|`%USERNAME%`, `%USERPROFILE%`|Windows の環境変数|

---

## 想定シナリオと活用アイデア

|シナリオ|使い方例|
|---|---|
|**① 日次議事録や日誌**|テンプレート: `Meeting_$YYYY-$MM-$DD.docx`。右クリック一発で「今日の日付入り議事録」を生成。|
|**② プロジェクトの雛形作成**|フォルダー構成 `src\`, `test\`, `docs\README.md` をテンプレート化。新しい案件ごとに即席で同じ骨格を展開。|
|**③ ブログ／技術メモ**|`YYYY-MM-DD-title.md`＋Front-Matter のひな形を置き、静的サイト生成 (Hugo など) の新規投稿を高速化。|
|**④ エンジニアリング用ファイル**|VHDL/Verilog 開発なら `tb_template.vhd`, `run.py` をまとめたサブフォルダーを登録。毎回のテストベンチ作成が 2 クリックで完了。|
|**⑤ 会社規定書式の迅速配布**|Excel/Word テンプレートを部署共通 OneDrive に置き、New+ から社員全員が同一フォーマットを使用。|
|**⑥ 自動リネーム＋変数連携**|画像整理で `IMG_$YYYY$MM$DD_$hh$mm$ss.jpg` をテンプレートに。スクリーンショットを即時日付命名で保存。|

---

## よくある質問

|Q|A|
|---|---|
|**テンプレートの拡張子や番号をメニューに表示したくない**|New+ 設定 > **表示オプション**で「拡張子を隠す」「先頭の数字/スペース/ドットを隠す」を切り替えます。([Microsoft Learn](https://learn.microsoft.com/ja-jp/windows/powertoys/newplus "PowerToys 用 New+Windows \| Microsoft Learn"))|
|**既存の “新規作成” と何が違う？**|標準メニューは固定の単一ファイル種のみ。New+ は**複数ファイル／フォルダーを含む任意のテンプレート**をユーザーが自由に追加・並べ替えできます。|
|**導入バージョンは？**|モジュールは v0.85.0 で登場し、その後の v0.85.1 以降で安定化パッチが配布されています。([GitHub](https://github.com/microsoft/powertoys/releases/ "Releases · microsoft/PowerToys · GitHub"))|

---

### まとめ

New+ は「**コピーしてリネームするだけの単純作業**」を右クリック 2 回で済ませる小さな自動化ツールです。テンプレートを工夫すれば、日々のドキュメント整備や新規プロジェクト立ち上げの初動が確実に短縮できます。まずは 1 つテンプレートを置いて、いつもの作業フローにどれだけ時短効果が出るか試してみてください。


[[WordでのPowerToysのNew+機能利用例]]