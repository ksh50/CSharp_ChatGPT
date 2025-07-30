#ChatGPT #VUnit #VHDL 

```
質問：
基本的にVUnitはシミュレーション結果を保持しないと思いますが、設定によりシミュレーション波形を保存できるようにできますか？

例えばですが、テストに失敗した場合だけシミュレーション波形を保存するようにできると嬉しいのですが・・・
```

はい、VUnitはデフォルトではシミュレーション波形（VCDやWLF）を保存しませんが、設定により波形を自動生成・保存することが可能です。さらに、「テストが失敗した場合のみ波形を保存」といった条件付きの波形保存も実現できます。

以下に詳細を解説します。

---

## 1. VUnitで波形を保存する方法

VUnitでは、シミュレーション実行時に波形ファイルを生成するために以下の手法を使います。

### Pythonスクリプト側で設定

`add_config` メソッドや `set_sim_option` を使用して波形生成を制御します。

例: GHDLでVCDファイルを出力

```python
from vunit import VUnit

vu = VUnit.from_argv()

lib = vu.add_library("lib")
lib.add_source_files("tb_minimal.vhd")

tb = lib.test_bench("tb_minimal")

# 波形ファイルを出力する設定
tb.set_sim_option("ghdl.gtkwave_script.gui", True)  # GTKWave起動用
tb.set_sim_option("ghdl.wave_file", "wave.ghw")     # GHDLの波形ファイル出力

vu.main()
```

ModelSim/Questaの場合:

```python
tb.set_sim_option("modelsim.vsim_flags", ["-voptargs=+acc"]) # 波形参照用に必要
tb.set_sim_option("modelsim.vsim_flags", ["-do", "log -r /*; add wave -r /*"])
```

---

## 2. 失敗時だけ波形を保存する方法

VUnitには**テスト結果に応じてシミュレーション波形を残す/残さない制御**を行う機能があります。

### 仕組み

- **テスト実行時**に一時ディレクトリでシミュレーションを行い、必要に応じて波形ファイルをコピー・保持します。
    
- `--keep` や `--keep-sim` オプション、もしくはPython APIの `keep` 引数を利用して制御可能。
    

---

### コマンドラインオプションで制御

- **常に波形保存**:
    
    ```bash
    python run.py --keep
    ```
    
    → 全テストで生成されたシミュレーション作業ディレクトリが削除されず残る。
    
- **失敗時だけ波形保存**:
    
    ```bash
    python run.py --keep-failed
    ```
    
    → **失敗したテストだけ**、シミュレーション作業ディレクトリ（波形ファイル含む）が保持される。
    

---

### Pythonコードで設定（条件付き）

```python
vu.main(keep='failed')  # 失敗時のみシミュレーション結果を保持
```

この場合、失敗したテストの作業ディレクトリ（`output/`配下）が残り、  
その中にGHDLなら`.ghw`、ModelSimなら`.wlf`等の波形ファイルが含まれます。

---

## 3. 実践的なワークフロー

1. **普段のテスト:**
    
    ```bash
    python run.py
    ```
    
    → 波形保存せずに高速実行。
    
2. **デバッグ時:**
    
    ```bash
    python run.py --keep-failed
    ```
    
    → 失敗したテストのみ波形残存。`output/lib.tb_minimal.test_xxx/`に波形が保存される。
    
3. **常時波形取得:**
    
    ```bash
    python run.py --keep
    ```
    
    → すべてのテストで波形確認可能。
    

---

## 4. まとめ

✅ **波形保存は可能**であり、次のように制御できます：

- `--keep` → 全テスト波形保存
    
- `--keep-failed` → 失敗テストのみ波形保存
    
- `set_sim_option` → 波形ファイル形式・保存場所を制御
    

---

これにより、普段は軽量に実行しつつ、失敗時のみデバッグ用波形を残す運用が可能になります。

---

次は 「失敗時に自動でGTKWaveやModelSim GUIを開く設定」も紹介しましょうか？


[[VUnitテスト失敗時にGUI開く設定]]