#ChatGPT #VUnit 

```
runner_cfg についてさらに教えてください。run.py のコードをPythonインタープリタが解釈し、run.py内のの各種設定・コマンドで作られたVHDLテストベンチへのテスト実行指示内容を、vu.main()メソッドが実行された時点でrunner_cfgという文字列にエンコードして渡すということでしょうか？
```

#### 仕組みの全体像

1. **Python 側（run.py → `vu.main()` 実行時）**
    
    - `run.py` でライブラリ追加・テストベンチ登録・オプション設定などを行ったあと `vu.main()` を呼ぶと、VUnit は
        
        1. すべてのテストベンチをスキャンし、走らせる **テストケースの集合** を決定
            
        2. テストごとに _Python 内部の辞書_ `runner_cfg = { ... }` を生成
            
            ```python
            runner_cfg = {
                "enabled_test_cases": ",".join(selected_names),
                "use_color": self._simulator_if.use_color,
                "output path": output_path.replace("\\", "/") + "/",
                "active python runner": True,
                "tb path": tb_path.replace("\\", "/") + "/",
            }
            config.generics["runner_cfg"] = encode_dict(runner_cfg)
            ```
            
            （VUnit 本体側の実装より）([GitHub](https://github.com/VUnit/vunit/issues/579?utm_source=chatgpt.com "Get run CLI args from Results · Issue #579 · VUnit/vunit · GitHub"))
            
        3. `encode_dict` で **「キー : 値, キー : 値 …」** というプレーン文字列に変換し、必要なら zlib 圧縮＋Base64 化（※古いバージョンのみ）
            
        4. シミュレータ起動コマンドに  
            `-g/tb_example/runner_cfg="active python runner : true,enabled_test_cases : …"`  
            の形で **generic 実引数** を追加して起動する。([GitHub](https://github.com/VUnit/vunit/issues/565?utm_source=chatgpt.com "Problem starting simulation run with ModelSim Microsemi 10.2c #565"))
            
2. **VHDL 側（シミュレーション開始直後）**
    
    - トップ Entity に必須 generic `runner_cfg : string` があることを VUnit スキャナが確認。([vunit.github.io](https://vunit.github.io/run/user_guide.html "Run Library User Guide - VUnit documentation"))
        
    - `test_runner_setup(runner, runner_cfg);` が呼ばれると、VHDL パッケージ `run_pkg` 内で
        
        - 文字列を辞書にパース（`has_key`, `get`）
            
        - `enabled_test_cases` リストを作成
            
        - 乱数シード、カラー設定、`output_path()`/`tb_path()` などを保存
            
        - 以後の `run("名前")` マクロで「実行対象かどうか」を判定する。([GitHub](https://raw.githubusercontent.com/VUnit/vunit/master/vunit/vhdl/run/src/run.vhd "raw.githubusercontent.com"))
            
3. **情報の内容（主要キー）**
    
    |キー|役割|典型的な値例|参照関数/場所|
    |---|---|---|---|
    |`active python runner`|Python 連携中かどうか|`true/false`|`active_python_runner(runner_cfg)`|
    |`enabled_test_cases`|実行するテスト名（カンマ区切り）|`__all__` / `TestA,TestB`|`run_pkg` 内で解析|
    |`output path`|個別テスト用アウトプットフォルダ|`…/vunit_out/test_output/…/`|`output_path(runner_cfg)`([vunit.github.io](https://vunit.github.io/run/user_guide.html?utm_source=chatgpt.com "Run Library User Guide - VUnit documentation"))|
    |`tb path`|テストベンチソースの場所|`…/src/`|`tb_path(runner_cfg)`|
    |`seed`|ベース乱数シード (hex)|`8c5c0ea80b58a8ee`|`get_seed()`|
    |`use_color`|ANSI カラー出力可否|`true/false`|`ansi_pkg.enable_colors` 呼び出し|
    
4. **タイミング**
    
    - **生成タイミング**: _必ず_ `vu.main()` 内で「シミュレーションを起動する直前」。
        
        - `run.py` を Python インタプリタが評価した時点では、まだ文字列は存在しない。
            
    - **作用範囲**: 1 テスト実行 = 1 runner_cfg。パラメータの異なるコンフィグを Python 側で複製すると、それぞれ別々の文字列が渡る。
        
5. **互換性メモ**
    
    - **Base64 圧縮の有無**: 2023 年以降の公式リリースは「そのまま ASCII」で渡す方式に統一。古いバージョンや文字数制限回避パッチでは `encode_dict → zlib → base64` が使われる。
        
    - **長さ制限**: いくつかのシミュレータは generic 文字列長に上限があり、`runner_cfg_default` 定数サイズで切り詰められる既知バグがあった（Issue #864）。最新版では緩和済み。([GitHub](https://github.com/VUnit/vunit/issues/864?utm_source=chatgpt.com "The max. length of runner_cfg is defined by runner_cfg_default ..."))
        
6. **カスタマイズしたい場合**
    
    - テストベンチ固有パラメータを渡したいときは、**独自 generic** を追加し、Python 側で `tb.set_generic("my_cfg", "value")` を使うのが推奨（`runner_cfg` 自体を手で編集しない）。
        
    - Python から内容確認をしたい場合は `--export-json` でテストメタデータをファイル化する方法が用意されている。
        

---

##### まとめ

- `runner_cfg` は **vu.main() が生成 → generic 経由で VHDL へ注入** される“設定パケット”文字列。
    
- 文字列化は `encode_dict` が行い、キー:値の辞書（enabled_test_cases, seed, output_path…）が渡る。
    
- VHDL 側では `test_runner_setup` がただちにデコードし、`run(...)` マクロや `output_path(runner_cfg)` などで活用される。
    
- ユーザは通常 Python スクリプトで意識する必要はなく、_runner_cfg を直接書き換える必要もない_。
