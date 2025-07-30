#ChatGPT #VUnit #VHDL 

```
依頼：
VUnit依存部分を残したまま、単独シミュレーション可能にする方法（ダミーrunner_cfg注入やcontext利用）について、詳しく解説をお願いします。
```

VUnit依存部分を残したまま単独シミュレーションを行うためには、以下の2つのポイントを解決する必要があります。

1. `runner_cfg` のダミー値を注入する
    
2. `vunit_context` に含まれるVUnit APIをそのまま利用できるようにする
    

以下に詳細な解説と具体例を示します。

---

## 1. `runner_cfg` のダミー値注入

`runner_cfg` は、VUnitがPythonスクリプト実行時に各テスト用の設定文字列を渡すための **genericパラメータ** です。  
単独シミュレーションでは、**ダミー文字列を直接指定**すればエラーを回避できます。

### 修正版エンティティ宣言

```vhdl
entity tb_minimal is
  generic (
    runner_cfg : string := "runner_cfg=disabled"  -- ダミー値
  );
end entity;
```

このようにデフォルト値を設定しておけば、VUnitから起動した場合も従来通り動作し、単独シミュレーション時もgeneric指定不要です。

---

## 2. `vunit_context` を使ったままシミュレーション

`library vunit_lib; context vunit_lib.vunit_context;` はそのまま利用可能です。  
ただし、**VUnitのライブラリ（`vunit_lib`）を事前にコンパイルしておく必要があります**。

### 事前準備（VUnitライブラリのコンパイル）

VUnitはVHDLパッケージ群を提供しています。Pythonを使わずに手動でライブラリをコンパイルできます。

例：GHDLの場合

```bash
# VUnitのライブラリファイルを取得（pipでインストール済ならこのディレクトリに存在）
cd ~/.local/lib/python3.12/site-packages/vunit/vhdl

# VUnitライブラリをGHDLでコンパイル
ghdl -a --work=vunit_lib vunit/vunit_context.vhd
ghdl -a --work=vunit_lib vunit/run_pkg.vhd
ghdl -a --work=vunit_lib vunit/runner_pkg.vhd
ghdl -a --work=vunit_lib vunit/check_pkg.vhd
...（必要に応じて他のパッケージも）
```

Questa/ModelSimの場合:

```tcl
vlib vunit_lib
vcom -work vunit_lib vunit/vunit_context.vhd
vcom -work vunit_lib vunit/run_pkg.vhd
vcom -work vunit_lib vunit/runner_pkg.vhd
vcom -work vunit_lib vunit/check_pkg.vhd
```

こうして `vunit_lib` を事前にシミュレータに登録しておくことで、  
**`context vunit_lib.vunit_context;` を残したままコンパイルできます**。

---

## 3. 実行フロー

これで次のように**単独シミュレーション**が可能です。

### GHDLの場合

```bash
ghdl -a tb_minimal.vhd
ghdl -e tb_minimal
ghdl -r tb_minimal --vcd=wave.vcd
```

### Questa/ModelSimの場合

```tcl
vcom tb_minimal.vhd
vsim work.tb_minimal
run -all
```

---

## 4. 動作のポイント

- `test_runner_setup` / `test_runner_cleanup` もそのまま動作しますが、`runner_cfg`がダミーなので「テスト自動分岐機能」は使われません。
    
- `check_equal` も `vunit_context` に依存しているため、そのまま動作します。
    
- つまり、**VUnitのテストランナー環境を「単一テストモード」で擬似的に再現**している状態になります。
    

---

## 5. 応用: `runner_cfg`をコマンドラインから渡す

もしもテストごとに異なる `runner_cfg` をシミュレーション時に指定したいなら、  
generic指定で渡すことも可能です。

例：GHDL

```bash
ghdl -r tb_minimal --generic=runner_cfg="runner_cfg=dummy"
```

例：Questa

```tcl
vsim -g/runner_cfg="runner_cfg=dummy" work.tb_minimal
```

---

## まとめ

✅ **VUnit依存部分を残したまま単独シミュレーションするには**:

1. `runner_cfg` にデフォルト値（ダミー）を設定。
    
2. VUnitのライブラリ（`vunit_lib`）を事前にコンパイル。
    
3. 通常のVHDLテストベンチと同様にシミュレーション可能。
    

---

こうすることで、VUnit経由でも単独シミュレーションでも動く「ハイブリッド型テストベンチ」を作れます。

---

次に、このハイブリッド型のメリット（CI用テストとデバッグ用シミュレーションを共存させる設計）についても解説しましょうか？