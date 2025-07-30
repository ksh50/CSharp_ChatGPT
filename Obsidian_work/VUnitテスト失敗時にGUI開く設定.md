#ChatGPT #VUnit #VHDL 

```
依頼：
失敗時に自動でGTKWaveやModelSim GUIを開く設定の紹介をしていただけますか？
```

はい、VUnitではテスト失敗時に自動でGTKWaveやModelSimのGUIを開く設定が可能です。  
これにより、テスト失敗時に即座に波形を確認でき、デバッグ効率が大幅に向上します。

---

## 1. GHDL + GTKWaveの場合

### 設定方法

VUnitのPythonスクリプトで、`set_sim_option` を使用してGTKWaveを自動起動するオプションを有効化します。

```python
from vunit import VUnit

vu = VUnit.from_argv()
lib = vu.add_library("lib")
lib.add_source_files("tb_minimal.vhd")

tb = lib.test_bench("tb_minimal")

# GTKWave自動起動設定
tb.set_sim_option("ghdl.gtkwave_script.gui", True)

vu.main(keep='failed')  # 失敗時のみ波形保持
```

### 動作

- テスト失敗時、VUnitが `output/lib.tb_minimal.test_XXXX/` に波形ファイル（`.ghw`）を生成。
    
- 失敗直後にGTKWaveが自動起動し、波形ファイルをロード。
    

---

## 2. ModelSim/Questaの場合

ModelSimでは、`vsim` コマンドにGUIモード (`-gui`) を有効にすることで失敗時にGUIが自動起動できます。

### 設定例

```python
tb.set_sim_option("modelsim.init_files.after_load", ["wave.do"])  # 波形設定スクリプト
tb.set_sim_option("modelsim.vsim_flags", ["-gui"])  # GUIモードで起動
```

`wave.do`ファイルには波形表示設定を記述します：

```tcl
log -r /*
add wave -r /*
```

これにより、テスト失敗時にModelSim GUIが自動で開き、波形がロードされます。

---

## 3. 条件付きで「失敗時のみ」GUIを開く

GUI起動はテスト失敗時だけ行うのが効率的です。  
VUnitでは以下のように`post_check`フックを使って、失敗時のみGUIを呼び出せます。

### 実装例（GHDL + GTKWave）

```python
import subprocess
from vunit import VUnit

def open_wave_if_failed(output_path):
    # テスト失敗時にGTKWaveを起動
    subprocess.Popen(["gtkwave", str(output_path / "wave.ghw")])

vu = VUnit.from_argv()
lib = vu.add_library("lib")
lib.add_source_files("tb_minimal.vhd")

tb = lib.test_bench("tb_minimal")

# post_checkで失敗時に波形ビューアを起動
@tb.test("check_wave_on_fail")
def _(test):
    try:
        # テストの実行
        test.check_output("expected")
    except Exception:
        open_wave_if_failed(test.output_path)
        raise  # 例外を再送出して失敗を記録

vu.main(keep='failed')
```

---

## 4. 実用的な運用

- **普段:**  
    `python run.py` → 波形生成なし（高速）
    
- **デバッグ:**  
    `python run.py --keep-failed` → 失敗時に波形保持  
    ＋ `ghdl.gtkwave_script.gui=True` → GTKWave自動起動
    

---

## まとめ

✅ **VUnitで失敗時に波形GUIを自動起動するには:**

- GHDL: `tb.set_sim_option("ghdl.gtkwave_script.gui", True)`
    
- ModelSim: `tb.set_sim_option("modelsim.vsim_flags", ["-gui"])`
    
- さらに`post_check`で波形ビューアを条件付き起動可能。
    

---

次に、「複数テストがあるときに、失敗テストだけ自動でGUIを開き順番に確認する仕組み」についても解説しましょうか？

