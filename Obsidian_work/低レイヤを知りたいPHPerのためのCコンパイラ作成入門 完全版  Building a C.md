#C言語 #コンパイラ

…というSpeakerDeck記事を見つけた。
[低レイヤを知りたいPHPerのためのCコンパイラ作成入門 完全版 / Building a C...](https://speakerdeck.com/tomzoh/building-a-c-compiler-for-phpers-who-want-to-dive-into-low-level-programming-expanded?slide=324)

通常Cコンパイルはアセンブラ生成とオブジェクトファイル生成の2段階がある。
今回の話はアセンブラ生成部分。
アセンブラ生成部分は下記3段階で構成できる。
- トークナイザ
- パーサ
- コードジェネレータ

簡単にCコンパイラのサブセットを作る手順を詳細に紹介してくれている。

EBNFベースのパーサ実装は自分でもある程度理解できた。

下記の紹介も入っていた。
[低レイヤを知りたい人のためのCコンパイラ作成入門](https://www.sigbus.info/compilerbook)
