#ChatGPT #Obsidian 

```
質問：
Obsidian標準機能で、列幅が小さくなったり改行されたりします。標準機能で対策はありますか？
```

### 結論から

- **Obsidian 本体だけでは列幅をマウスで伸縮する UI は用意されていません**。行がギュッと折り返されたり、やたら細い列になるのは仕様です([Obsidian Forum](https://forum.obsidian.md/t/change-width-of-the-table-columns/73810 "Change width of the table columns - Feature archive - Obsidian Forum"))
    
- ただし “プラグインなし” でも **設定や CSS スニペット** を使ってかなり改善できます。
    

---

## 1. まず試したい最短手順

|やること|効果|手順|
|---|---|---|
|**Readable line length をオフ**|ペイン全幅を使い、列が広がる|**設定 → Editor → Readable line length** のチェックを外す※ホットキー「Toggle readable line length」を割り当てておくと便利([Obsidian Forum](https://forum.obsidian.md/t/how-to-get-a-lager-page-width-in-both-editing-mode-and-preview-mode/7555 "How to get a lager page width in both editing mode and preview mode? - Help - Obsidian Forum"))|
|**行内改行を防ぐ**|セル内で単語が折り返されない|後述の CSS スニペットを追加|

> これだけで「列幅が妙に狭い」「1 文字ごとに改行される」といったストレスはかなり緩和されます。

---

## 2. 標準機能だけで使える CSS スニペット例

Obsidian には **CSS Snippets**（外部プラグイン不要）という仕組みがあります。

1. `.obsidian/snippets/table-nowrap.css` というファイルを作成
    
2. 下記コードを貼り付け保存
    
3. **設定 → Appearance → CSS snippets** で有効化
    

```css
/* --- 表全体のレイアウト --- */
.markdown-source-view table,
.markdown-preview-view table {
  /* 行の内容に応じて幅を広げる */
  table-layout: auto;
}

/* --- 改行を抑制しつつ、最低幅を確保 --- */
.markdown-source-view th,
.markdown-source-view td,
.markdown-preview-view th,
.markdown-preview-view td {
  white-space: nowrap;          /* 改行禁止 */
  min-width: 8ch;               /* 8 文字分の幅を死守 */
}

/* オプション：列幅を固定したいとき */
:root{
  --table-column-min-width: 30ch;
  --table-column-max-width: 30ch;
} /* 1.5.0 以降の Obsidian/Electron では変数だけでも効く:contentReference[oaicite:2]{index=2} */
```

### ポイント

- `white-space: nowrap;` で「1 単語ごとの折り返し」を止める
    
- `min-width` や CSS 変数 `--table-column-min-width` を使うと _最小幅_ を保証できる
    
- `table-layout: auto;` を外すと固定幅 (`fixed`) になり、セル幅は `width` 指定分で切り揃えられます
    

---

## 3. ノート単位で効かせたい場合

フロントマターにクラスを付け、スニペット側でそのクラス限定にする方法が安全です。

```yaml
---
cssclass: wide-table
---
```

```css
/* wide-table クラスのときだけルールを適用 */
.wide-table table      { table-layout:auto; }
.wide-table th,
.wide-table td         { white-space:nowrap; }
```

---

## 4. インライン HTML で最速ハック

「CSS は触りたくない、でも 1 つの表だけ何とかしたい」場合は Markdown 内に素直に HTML を混ぜても OK です。

```html
<table style="table-layout:fixed;width:100%;">
  <tr>
    <th style="width:25%;white-space:nowrap;">項目</th>
    <th style="width:75%;">説明</th>
  </tr>
  <tr><td>サンプル</td><td>改行されません</td></tr>
</table>
```

---

## 5. それでも足りなければ…

1. **Advanced Tables** や **Markdown Table Editor** などのプラグインで GUI 操作を追加
    
2. テーマ独自の _style settings_（Minimal の “Table width” など）を利用
    
3. Dataview で“動的テーブル”を作り、CSS で `table.table-view-table` を調整
    

---

### まとめ

- UI レベルで列幅を変える機能は未実装（プラグイン要望が多数）
    
- **Readable line length の切替 + CSS Snippet** が “純正機能だけ” で出来る最も簡単な対策
    
- ノートごとの `cssclass` で局所適用、インライン HTML で単発対応も可
    

これで「列が狭くて読めない」「勝手に折り返される」といった悩みはかなり解消できるはずです。ぜひお試しください！