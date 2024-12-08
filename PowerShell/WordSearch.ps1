Add-Type -AssemblyName System.IO.Compression.Filesystem
function Get-TextOfDocxDocumant {
    <#
        .EXAMPLE
        Get-TextOfDocxDocumant .\test.docx
    #>
    param (
        [string]$path
    )

    try {
        $fullPath = (Resolve-Path -Path $path).Path
        $compressed = [IO.Compression.Zipfile]::OpenRead($fullPath)

        $target = $compressed.Entries | Where-Object {$_.Fullname -eq "word/document.xml"}
        $stream = $target.Open()
        $reader = New-Object IO.StreamReader($stream)
        $content = $reader.ReadToEnd()

        $reader.Close()
        $stream.Close()
        $compressed.Dispose()

        $m = [regex]::Matches($content, "<w:p.*?>.*?</w:p>")
        return [PSCustomObject]@{
            Status = "OK"
            Lines = @($m.value -replace "<.+?>", "")
        }
    }
    catch {
        return [PSCustomObject]@{
            Status = "FILEOPENED"
            Lines = @()
        }
    }
}
