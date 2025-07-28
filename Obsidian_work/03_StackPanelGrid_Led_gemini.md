#Gemini #CSharp #プログラミング #WPF 

2025/07/22

03_StackPanelGrid_Led をベースにリファクタリング。

意図せず、コードビハインドでプログラムが書かれていたため、geminiの推奨でMVVMでリファクタリング。

分離が良くなった？（内容を完全には理解できず）

最初にMVVMスタイルで4つのファイルを追加してもらった。
この段階ではdotnet runで変化なし。
→実はMVVMスタイルのファイルを追加しただけで、エントリポイントからは参照されていなかった。

その後、MainWindow.xaml と MainWindow.xaml.cs を大幅に変更。
→この段階でビルド失敗
→質問したところ、UTF8で保存すべきところがUTF8 with BOMで保存されていたことに拠る、と知った。
→手動で各ファイルをUTF8で保存して再度dotnet runしたところ、ひとまず動くようになった。




以下、gemini のチャットのコピペ（Markdownで保存できないものか？）
[[20250722_gemini_chat]]

UTF8 with BOMの部分だけ切り出し
[[UTF8 with BOM]]

Geminiのリファクタリング後に不要になったファイルはどれかを聞いてみた。
[[03_StackPanelGrid_Led_geminiリファクタリング後の必要ファイル]]

コードビハインドとは？MVVMのメリットとは？という視点で追加調査
[[コードビハインドとMVVM]]
