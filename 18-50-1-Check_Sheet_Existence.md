## USER
C#、closedxml、指定した文字列のシートの有無を確認するには？

## ASSISTANT
ClosedXMLを使用してExcelファイル内に特定の名前のシートが存在するかどうかを確認するには、`IXLWorkbook.Worksheets` コレクションを使います。以下に簡単なコード例を示します。

```csharp
using ClosedXML.Excel;
// ...

string filePath = "your_file_path.xlsx";
string sheetNameToCheck = "SheetToFind";

using (IXLWorkbook workbook = new XLWorkbook(filePath))
{
    bool sheetExists = workbook.Worksheets.Any(w => w.Name == sheetNameToCheck);
    
    if(sheetExists)
    {
        Console.WriteLine("シートが存在します。");
    }
    else
    {
        Console.WriteLine("シートが存在しません。");
    }
}
```

このコードは、`sheetNameToCheck` に指定した名前のシートがExcelファイル内に存在するかどうかを確認します。存在する場合は `true`、存在しない場合は `false` を返します。

## USER
指定したシートで最終行を数値で得るには？

## ASSISTANT
ClosedXMLを使用して特定のシートの最終行番号を取得するには、`IXLWorksheet.LastRowUsed()`メソッドを使用できます。以下はそのサンプルコードです：

```csharp
using ClosedXML.Excel;
// ...

string filePath = "your_file_path.xlsx";
string sheetName = "Sheet1"; // ここにシート名を指定

using (IXLWorkbook workbook = new XLWorkbook(filePath))
{
    IXLWorksheet worksheet = workbook.Worksheet(sheetName);
    int lastRow = worksheet.LastRowUsed().RowNumber();

    Console.WriteLine($"最終行は {lastRow} 行目です。");
}
```

このコードでは、`sheetName`で指定したシートにおける最後に使用されている行の行番号を取得します。`LastRowUsed()` メソッドは最後にデータが入っている行を返すので、この行番号を使って最終行として扱うことができます。

## USER
行にアウトラインを設定するには？

## ASSISTANT
ClosedXMLを使用してExcelシートの特定の行にアウトライン（グループ化）を設定することができます。具体的には、`IXLRangeRow.Group()` メソッドを使用します。以下はそのサンプルコードです：

```csharp
using ClosedXML.Excel;
// ...

string filePath = "your_file_path.xlsx";
string sheetName = "Sheet1"; // ここにシート名を指定

using (IXLWorkbook workbook = new XLWorkbook(filePath))
{
    IXLWorksheet worksheet = workbook.Worksheet(sheetName);

    // 2行目から4行目までをグループ化（アウトライン化）します
    worksheet.Rows(2, 4).Group();

    // 変更を保存
    workbook.Save();
}
```

このコードは、2行目から4行目までの行をグループ化（アウトライン化）します。Excelでファイルを開くと、これらの行の左側にアウトラインバーが表示され、折りたたみや展開が可能になります。

このようにして行にアウトラインを設定することができます。

## USER
折りたたみ、展開を操作するコードを提示していただけますか？

## ASSISTANT
ClosedXML では、行や列の折りたたみ（アウトライン）の状態を直接操作するAPIは提供されていないので、折りたたみや展開をプログラムから制御することはできません。

しかし、あらかじめシートにアウトラインを設定して保存することはできます。その後、ユーザーが手動でExcel内で折りたたみや展開を行う必要があります。

もし絶対にプログラムから折りたたみや展開を制御したい場面があれば、OpenXML SDK や ExcelのCOMオブジェクト（Excel Interop）を使用する必要があります。これらのライブラリはより低レベルな操作を提供するので、折りたたみや展開をプログラムから制御できます。ただし、これらの方法は一般的には複雑であり、特にCOMオブジェクトの場合はExcelがインストールされている環境でしか動作しません。

## USER
アウトラインのレベルは設定できますか？

## ASSISTANT
はい、ClosedXMLではアウトラインのレベルを設定することができます。`IXLRangeRow.Group()` メソッドにオプションでレベル数を指定できます。以下にコードの例を示します。

```csharp
using ClosedXML.Excel;
// ...

string filePath = "your_file_path.xlsx";
string sheetName = "Sheet1"; // シート名を指定

using (IXLWorkbook workbook = new XLWorkbook(filePath))
{
    IXLWorksheet worksheet = workbook.Worksheet(sheetName);

    // 2行目から4行目までをレベル1でグループ化
    worksheet.Rows(2, 4).Group(1);

    // 6行目から8行目までをレベル2でグループ化
    worksheet.Rows(6, 8).Group(2);

    // 変更を保存
    workbook.Save();
}
```

この例では、2行目から4行目までをレベル1で、6行目から8行目までをレベル2でグループ化（アウトライン化）しています。このようにして、異なるレベルで行をグループ化することができます。

## USER
アウトラインを削除する方法は？

## ASSISTANT
ClosedXMLを使用して行のアウトライン（グループ化）を削除するには、`IXLRangeRow.Ungroup()` メソッドを使用できます。以下はそのサンプルコードです：

```csharp
using ClosedXML.Excel;
// ...

string filePath = "your_file_path.xlsx";
string sheetName = "Sheet1"; // ここにシート名を指定

using (IXLWorkbook workbook = new XLWorkbook(filePath))
{
    IXLWorksheet worksheet = workbook.Worksheet(sheetName);

    // 2行目から4行目までのグループ化（アウトライン）を解除します
    worksheet.Rows(2, 4).Ungroup();

    // 変更を保存
    workbook.Save();
}
```

このコードは、2行目から4行目までの行のグループ化（アウトライン）を解除します。この操作後にExcelでファイルを開くと、指定した行範囲のアウトラインが削除されているはずです。

## USER
特定のシートをアクティブにするには？

## ASSISTANT
ClosedXMLでは直接的に「アクティブなシート」を設定する機能は提供されていません。このライブラリは主にExcelファイルのデータ操作に焦点を当てており、UI（ユーザーインターフェース）に関連する機能は限定的です。

一般に、アクティブなシートを設定することは、Excelが開かれたときにどのシートが最初に表示されるかを決定するUI操作です。このような操作は、通常はOpenXML SDKやExcelのCOMオブジェクト（Excel Interop）を使用して行われます。

もし絶対に特定のシートをアクティブにしたい場合は、より低レベルなAPIを使う必要があります。それは、一般的にはプログラミングが複雑になり、特にCOMオブジェクトの場合、Excelがインストールされている環境でしか動作しないという制限があります。

## USER
アウトラインのスタイルを設定するには？

## ASSISTANT
ClosedXMLでは、アウトライン（行や列のグループ化）に対するスタイル（線種、線の太さ、色など）の設定はサポートされていません。このライブラリは主にExcelファイルのデータ操作に焦点を当てており、UI（ユーザーインターフェース）に関連する細かいスタイル設定は限定的です。

もしアウトラインのスタイルを詳細に設定したい場合は、OpenXML SDKを使用するオプションがありますが、これはより低レベルなAPIであり、その使用は比較的複雑です。

別のオプションとして、ExcelのCOMオブジェクト（Excel Interop）を使用する方法もありますが、これにはExcelがインストールされている環境が必要です。

要するに、ClosedXMLではアウトラインのスタイル設定は限定的であり、より高度なスタイリングが必要な場合は他の方法を検討する必要があります。

## USER
アウトラインがコピーされないのも同じ理由？

## ASSISTANT
ClosedXMLにおいて、アウトライン（行や列のグループ化）がコピーされないのは、このライブラリの機能制限に起因する可能性が高いです。ClosedXMLは主にデータ操作と基本的なスタイリングに焦点を当てていますが、UIに関連する詳細な機能、特にアウトラインのような高度な設定は、必ずしも完全にサポートされているわけではありません。

もしアウトラインを含むような複雑なシートを別のシートにコピーする必要がある場合は、OpenXML SDKを使用するとより柔軟な操作が可能ですが、APIは低レベルであり使用は複雑です。

ExcelのCOMオブジェクト（Excel Interop）も同様の高度な操作が可能ですが、これにはExcelがインストールされている環境が必要です。

短い答えとしては、はい、アウトラインがコピーされないのもClosedXMLの機能制限による可能性が高いです。