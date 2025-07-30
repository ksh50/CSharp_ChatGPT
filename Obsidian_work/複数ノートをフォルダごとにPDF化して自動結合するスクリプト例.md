#ChatGPT #Obsidian 

```
依頼：
複数ノートをフォルダごとにPDF化して自動結合するスクリプト例を提示していただけますか？
```

以下は、**Obsidian Vault内の複数ノートをフォルダ単位でPDF化し、自動的に結合するスクリプト例（Windows PowerShell版）**です。

PandocでMarkdown→PDF変換し、**PDFtk**を使って結合する構成になっています。

---

## ✅ 必要な前提

1. **Pandoc** をインストール  
    → [https://pandoc.org/installing.html](https://pandoc.org/installing.html)
    
2. **LaTeXエンジン（PDF出力用）**（TinyTeX / MikTeX など）
    
3. **PDFtk（PDF結合ツール）** をインストール  
    → [https://www.pdflabs.com/tools/pdftk-server/](https://www.pdflabs.com/tools/pdftk-server/)
    

---

## 💻 PowerShellスクリプト例

```powershell
# ========= 設定 =========
# Obsidian Vault内のMarkdownフォルダ
$sourceFolder = "C:\Users\YourName\ObsidianVault\Notes"
# PDF出力先
$outputFolder = "C:\Users\YourName\ObsidianVault\PDFs"
# 結合後PDFのファイル名
$mergedPDF = Join-Path $outputFolder "MergedNotes.pdf"

# ========= 準備 =========
# 出力フォルダを作成
if (!(Test-Path $outputFolder)) {
    New-Item -ItemType Directory -Path $outputFolder | Out-Null
}

# ========= PDF変換 =========
# フォルダ内のMarkdownをPDF化
$mdFiles = Get-ChildItem -Path $sourceFolder -Recurse -Filter *.md

foreach ($file in $mdFiles) {
    $pdfName = ($file.BaseName + ".pdf")
    $pdfPath = Join-Path $outputFolder $pdfName

    Write-Host "変換中: $($file.Name) → $pdfName"
    pandoc $file.FullName `
        -o $pdfPath `
        --pdf-engine=xelatex `
        -V mainfont="IPAexGothic" `
        -V geometry:a4paper `
        -V fontsize=11pt
}

# ========= PDF結合 =========
Write-Host "PDF結合中..."
$pdfFiles = Get-ChildItem -Path $outputFolder -Filter *.pdf | Sort-Object Name
$pdfList = $pdfFiles | ForEach-Object { $_.FullName } -join " "

# PDFtkで結合
pdftk $pdfList cat output $mergedPDF

Write-Host "完了: $mergedPDF"
```

---

## 🔑 このスクリプトの動作

1. 指定フォルダ内の `.md` ファイルを再帰的に取得
    
2. Pandocで順次PDFに変換（日本語フォント設定済み）
    
3. PDFtkで生成したPDFを名前順に結合し、1つのPDFにまとめる
    

---

## ✅ カスタマイズ例

- **タグ別出力**  
    → `Get-ChildItem` でタグ抽出用に正規表現を追加
    
- **Canvas画像もPDF化して結合**  
    → `Canvas PNG → ImageMagickでPDF化 → 結合`
    
- **日付フォルダを自動作成して日次エクスポート**
    
    ```powershell
    $dateFolder = Join-Path $outputFolder (Get-Date -Format "yyyyMMdd")
    New-Item -ItemType Directory -Path $dateFolder
    ```
    

---

## 🔥 拡張：CanvasのPNGも自動でPDF化して統合する版も作れますが、追加しますか？