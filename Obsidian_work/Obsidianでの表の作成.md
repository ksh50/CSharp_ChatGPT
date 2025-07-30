#ChatGPT #Obsidian 

```
依頼：
Obsidianで表（テーブル）を使う方法を教えてください。
```
## 1. Markdown だけで作る基本テーブル

```md
| 日付 | タスク          | 進捗 |
| ---- | --------------- | ---: |
| 7/30 | テーブル調査    | ✅  |
| 7/31 | 記事執筆        | ⏳  |
```

- **縦線 `|`** で列を区切り、**ハイフン `-`** でヘッダ̲を作成
    
- `:--` = 左寄せ、`:--:` = 中央、`--:` = 右寄せ
    
- ヘッダ行は _必ず_ 2 本以上のハイフンが必要
    
- Live Preview では **右クリック → Insert → Table** でもテンプレが挿入できます([Obsidian Help](https://help.obsidian.md/advanced-syntax?utm_source=chatgpt.com "Advanced formatting syntax - Obsidian Help"))
    

---

## 2. Obsidian 標準機能での編集ポイント

|操作|デフォルトキー|動作|
|:--|:--|:--|
|次セルへ移動|Tab|セル間を水平移動|
|前セルへ戻る|Shift+Tab|〃 逆方向|
|次行へ移動|Enter|新規行を自動生成|
|コンテキストメニュー|右クリック|行列の挿入／削除・並べ替え|

- Live Preview 上で列幅や行・列の追加／削除、並べ替えが GUI から可能
- **Command Palette → Insert Table** で 3×2 の空表を即作成
    

---

## 3. よりリッチにする代表的プラグイン

|プラグイン|代表機能|いつ使う？|
|---|---|---|
|**Advanced Tables**|_Tab/Enter_ でセル移動、オートフォーマット、列並べ替え、数式計算、CSV 書き出し など|Obsidian 内で “Excel らしい” 操作感が欲しい時|
|**Markdown Table Editor**|CSV/Excel/Google Sheets を貼り付けて即 Markdown 化、行列のドラッグ操作|外部表データを頻繁に貼り付ける／編集する時|

### インストール手順（共通）

1. `設定 → Community plugins` で _Safe mode_ を **オフ**
2. Browse を開き、プラグイン名で検索
3. Install → Enable で有効化
4. 必要なら `設定 → プラグイン設定` でショートカットや動作を調整
    

> **Advanced Tables の例**
> 
> - オートフォーマットの有効化／無効化
> - `Ctrl+Shift+D` でテーブルサイドバーを開く
> - バージョン 0.22.1（2024-10-23 時点）で Obsidian 1.x に対応([GitHub](https://github.com/tgrosinger/advanced-tables-obsidian "GitHub - tgrosinger/advanced-tables-obsidian: Improved table navigation, formatting, and manipulation in Obsidian.md"))
>     

> **Markdown Table Editor の例**
> 
> - CSV・.xlsx をクリップボードにコピー → ノートで **貼り付け** すると自動で Markdown テーブル化
> - 行列の並べ替えやソート、ヘッダ整形が GUI で可能([obsidianstats.com](https://www.obsidianstats.com/plugins/markdown-table-editor "Markdown Table Editor - An Obsidian plugin to provide an editor for Markdown tables. It can open CSV data and data from Microsoft Excel, Google Sheets, Apple Numbers and LibreOffice Calc as Markdown tables from Obsidian Markdown editor."))
>     

---

## 4. Dataview で “動的テーブル”

```dataview
table file.name as "ページ", status, due
from "Project"
where status != "Done"
sort due asc
```

- 他ノートの YAML フロントマターや行内メタデータを集計
    
- フィルタ／並べ替え／計算列が書けるので “Notion 的” ビューワを作成可能
    
- DataviewJS を使えばセル内で JS による加工も OK
    

---

## 5. スプレッドシート ↔ Markdown 相互変換

|方向|方法|
|:--|:--|
|**Excel → Markdown**|(1) 範囲をコピー → (2) **Markdown Table Editor** でペースト／**Advanced Tables β** の _Paste Spreadsheet_ コマンド|
|**Markdown → CSV**|**Advanced Tables → Export to CSV** でダウンロード|

---

## 6. ちょっとした TIPS

1. **セル内改行**: `<br>` を書くか、`Shift+Enter` で行を分ける
2. **パイプ `|` をセルに入れたい**: `\|` とバックスラッシュでエスケープ([Obsidian Help](https://help.obsidian.md/advanced-syntax?utm_source=chatgpt.com "Advanced formatting syntax - Obsidian Help"))
3. **画像埋め込み**: `![[image.png|100]]` のようにリサイズも可
4. **数式**: `=SUM(A2:A10)` など、Advanced Tables の式を使う（列名は A, B, C…）
5. **Git で差分が見づらい**場合は Advanced Tables の _auto-format off_ で一行セルを避ける
    

---

### まとめ

- **小規模**なら標準 Markdown＋Live Preview の右クリック編集で十分
- **大量編集や計算**が欲しければ **Advanced Tables**
- **外部データ取込み**が中心なら **Markdown Table Editor**
- **データベース的利用**は **Dataview** を組み合わせる
    

これで Obsidian 内でのテーブル利用と拡張の選択肢が揃います。ぜひワークフローに合わせて組み合わせてみてください！


[[Obsidianでの表の列幅調整]]