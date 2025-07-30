#ChatGPT #Windows #時短 

```
質問：
例えば、特定のWordテンプレートをベースに、「○○調査_yyyymmdd.docx」を新規作成することはできますか？できる場合には手順を提示いただきたいです。
```

はい、**New+** では Word テンプレートを 1 クリックで複製しながらファイル名に日付（yyyymmdd 形式）を自動付与できます。以下の手順でセットアップしてみてください。

---

## 手順 (初回だけ)

1. **PowerToys を有効化**
    
    - PowerToys を起動 → 左ペイン **New+** → スイッチを **オン**。
        
    - 同じ画面の **テンプレート ファイル名の変数を置き換える** も **オン**にします（デフォルトはオフ）。([Microsoft Learn](https://learn.microsoft.com/ja-jp/windows/powertoys/newplus "PowerToys 用 New+Windows | Microsoft Learn"))
        
2. **テンプレート フォルダーを開く**
    
    - New+ 設定画面の **[テンプレートを開く]** をクリック。エクスプローラーで  
        `%LOCALAPPDATA%\Microsoft\PowerToys\NewPlus\Templates`  
        が開きます。([Microsoft Learn](https://learn.microsoft.com/ja-jp/windows/powertoys/newplus "PowerToys 用 New+Windows | Microsoft Learn"))
        
3. **Word テンプレートを用意して配置**
    
    - Word でベース文章を作成 → 名前を付けて保存 (**.docx**)。
        
    - 上記フォルダーにコピーし、ファイル名を
        
        ```
        市場調査_$YYYY$MM$DD.docx
        ```
        
        のように変更します。
        
        - `$YYYY` ＝西暦4桁、`$MM` ＝月2桁、`$DD` ＝日2桁なので、連結すれば **yyyymmdd** 形式になります。([Microsoft Learn](https://learn.microsoft.com/en-us/windows/powertoys/newplus?utm_source=chatgpt.com "PowerToys New+ for Windows - Learn Microsoft"))
            
4. （任意）**メニュー順を整理**
    
    - 先頭に `01_` などを付けると表示順を制御できます。拡張子や先頭番号を隠すオプションは New+ の **表示オプション** で切替。([Microsoft Learn](https://learn.microsoft.com/ja-jp/windows/powertoys/newplus "PowerToys 用 New+Windows | Microsoft Learn"))
        

---

## 使い方 (毎回)

1. 目的のフォルダーで右クリック → **New+**。
    
2. 先ほど登録した **市場調査_$YYYY$MM$DD** を選択。
    
3. 即座に
    
    ```
    市場調査_20250530.docx   （← 当日の日付入りで生成）
    ```
    
    が作られ、そのまま Word で開いて編集できます。
    

---

## バリエーション／応用アイデア

|要件|具体的なファイル名テンプレート例|補足|
|---|---|---|
|調査種別ごとにテンプレートを分けたい|`競合調査_$YYYY$MM$DD.docx`, `市場調査_$YYYY$MM$DD.docx`|New+ は入力プロンプトを持たないため、種別ごとに別ファイルで用意|
|フォルダー名を先頭に付けたい|`$PARENT_FOLDER_NAME_$YYYY$MM$DD.docx`|「東京支店」フォルダー内で作成 → `東京支店_20250530.docx`に展開|
|毎日作る日報|`日報_$YYYY$MM$DD.docx`|作業ログや議事録にも応用可|
|複数ファイル構成の雛形|`01_調査報告_$YYYY$MM$DD\`（フォルダー）内に `report.docx`, `data.xlsx` などを置く|フォルダーごと複製できる|

---

### 注意点

- 変数は **大文字小文字を区別**します（`$yyyy` は無効）。
    
- 「○○」部分を都度入力する“置き換えフィールド”は現状ありません。必要ならテンプレートを複数作るか、`$PARENT_FOLDER_NAME` でフォルダー名から動的取得してください。
    
- テンプレートフォルダーを **OneDrive 等に移す**と複数 PC で共通利用できます。([Microsoft Learn](https://learn.microsoft.com/ja-jp/windows/powertoys/newplus "PowerToys 用 New+Windows | Microsoft Learn"))
    

これで、Word を開いて「名前を付けて保存…」をする手間が丸ごと省け、ファイル名の付け忘れ・付け間違いも防げます。ぜひ日次の定型ドキュメント作成に取り入れてみてください。