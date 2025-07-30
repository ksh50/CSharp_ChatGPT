#ChatGPT #部品 #電気回路 

```
質問：
SamacSys CADモデルはKiCADで使うこともできるのですか？
Ultra Librarian CADモデルはKiCADでは使うことができない？
```

はい、どちらのサービスも **KiCad** で利用できます。ただし取り込み手順と扱えるファイル形式に少し違いがありますので、ポイントを整理しておきます。

---

## 1. SamacSys CADモデル（Library Loader） × KiCad

|項目|内容|
|---|---|
|対応状況|公式に KiCad をサポート（KiCad 6 以降の _.kicad_sym_ 形式にも対応）([samacsys.com](https://www.samacsys.com/kicad/ "KiCad Symbols \| Footprints \| 3D Models"))|
|取得方法|① 部品検索ポータル（Component Search Engine など）で「Download ECAD Model」② ライブラリ変換・配置を行う常駐アプリ **Library Loader** が自動起動|
|KiCad への登録|Library Loader が * シンボル： **SamacSys_Parts.kicad_sym** * フットプリント： **SamacSys_Parts.pretty** * 3D： _.step_ を所定フォルダへ配置し、一度だけライブラリパスを KiCad の「グローバルライブラリ」に追加すれば OK|
|使い勝手|ダウンロード直後に KiCad 側で「配置 → ライブラリ選択」するだけで部品を呼び出せる“ワンクリック”体験が特徴|

---

## 2. Ultra Librarian CADモデル（.bxl） × KiCad

|項目|内容|
|---|---|
|対応状況|**.bxl** をオンライン／Free Reader で変換すると **KiCad 用ライブラリ**を出力可能（Ultra Librarian は 30 以上の CAD 形式にエクスポート対応）([ultralibrarian.com](https://www.ultralibrarian.com/products/free-reader/ "Free Reader - Free Online PCB CAD Library - Ultra Librarian"))|
|取得方法|① メーカサイトや Ultra Librarian で _.bxl_ を入手② Ultra Librarian の **Online Reader** あるいは無償 **Free Reader** を使って「KiCad」にエクスポート|
|KiCad への登録|出力物に * _.kicad_sym_（KiCad 6 以降）または _.lib_（従来形式） * _.kicad_mod_（フットプリント） * _.step_（3D）が含まれるので、これらを KiCad のシンボル／フットプリント／3D ライブラリに手動でパス登録|
|使い勝手|**.bxl** を保管しておけば、別 CAD へ乗り換えたときにも再エクスポートできる汎用性が利点|

> **補足:** 旧バージョンの Free Reader では KiCad 変換がうまく動かない事例も報告されていましたが（KiCad 5 前後）現在のオンライン版／最新 Reader では解消されています。

---

### まとめ ― どちらを選ぶ？

|シーン|おすすめ|
|---|---|
|とにかく早く KiCad に配置したい|SamacSys（Library Loader が自動登録）|
|社内で複数 CAD を併用／将来乗り換えの可能性あり|Ultra Librarian（.bxl から再エクスポート可能）|
|ネット接続が制限されている環境|Ultra Librarian（.bxl＋ローカル Reader）|

いずれも KiCad で使用可能 ですので、

- ご自身のワークフロー（オンライン／オフライン、CAD混在、ライブラリ運用ポリシー）
    
- 必要な部品点数や更新頻度
    

に合わせて使い分けると良いでしょう。