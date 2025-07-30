#ChatGPT #VUnit #VHDL 

```
依頼：
ハイブリッド型のメリットについて解説をお願いします。
```

はい、`runner_cfg` にデフォルト値を設定せずにVHDLテストベンチを単独実行すると、エラーになります。  
この理由と回避方法（GHDL・ModelSimのオプション）を以下に整理します。

---

## 1. エラーが発生する理由

`runner_cfg` は generic パラメータとして必須宣言されているため、  
**デフォルト値を持たない generic はシミュレーション実行時に必ず値を指定する必要**があります。

```vhdl
entity tb_minimal is
  generic (
    runner_cfg : string   -- デフォルト値なし
  );
end entity;
```

このまま `ghdl -r tb_minimal` や `vsim tb_minimal` を実行すると、

- **GHDL:**
    
    ```
    ghdl:error: cannot find value for generic runner_cfg
    ```
    
- **ModelSim/Questa:**
    
    ```
    # ** Error: (vsim-3053) Generic 'runner_cfg' has no default value.
    ```
    

というエラーになります。

---

## 2. 回避方法

### (A) 実行時に `runner_cfg` を指定する

GHDLやModelSimでは、**generic値を実行オプションで与える**ことで回避できます。

#### GHDL

```bash
ghdl -r tb_minimal --generic=runner_cfg="dummy"
```

#### ModelSim/Questa

```tcl
vsim -g/runner_cfg="dummy" work.tb_minimal
```

---

### (B) コンパイル時に `runner_cfg` を固定値にする

VHDL-2008準拠のシミュレータであれば、**コンパイルオプションでgeneric値を上書き**できます。

#### GHDL (分析時指定)

```bash
ghdl -a --std=08 --generic=runner_cfg="dummy" tb_minimal.vhd
ghdl -e tb_minimal
ghdl -r tb_minimal
```

※ ただしこの方法は1回のコンパイルで値が固定されるため、テストごとに変更する柔軟性はありません。

---

### (C) VUnitを利用せずに「空文字」指定する

`runner_cfg`のチェックは`test_runner_setup`で行われますが、**空文字""を渡しても通常の実行は可能**です。

```bash
ghdl -r tb_minimal --generic=runner_cfg=""
```

VUnit経由の複数テスト切替機能は無効になりますが、シミュレーションは実行されます。

---

## 3. 推奨方法

- **デバッグ用:**  
    → `--generic=runner_cfg="dummy"` をシミュレータ起動時に指定するのがシンプル
    
- **開発・CI共用:**  
    → entity宣言側に `runner_cfg : string := ""` を設定してハイブリッド型にするのが便利
    
- **VUnit互換重視:**  
    → デフォルト値なしにしておき、デバッグ時は`--generic`指定で渡す
    

---

## 4. まとめ

- **デフォルト値なしではエラーになる**（GHDLもModelSimも同様）
    
- **回避策は `--generic`/`-g` オプションで値を注入すること**
    
- 空文字でも動作するので、VUnit機能を使わないデバッグ用途では `"dummy"` や `""` を渡せばOK
    

---

次は、`runner_cfg`を省略可能にしつつ、指定されない場合だけ自動で空文字を使うようにするVHDL-2008の書き方についても解説しましょうか？


[[runner_cfgを省略可能なVHDL-2008書き方]]
