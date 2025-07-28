VUnitにおいてPythonとVHDLテストベンチを仲介する重要な文字列
#ChatGPT #VHDL #VUnit #Python #2025年 

```
質問
VUnitにおいて、PythonとVHDLテストベンチを仲介する重要なものとして runner_cfg という文字列が使われると聞きました。この runner_cfg とはどのようなもので、どのように使われるか教えてください。
```

### runner_cfg とは？

- **VUnit が “これはテストベンチだ” と認識するための目印**  
    VUnit は VHDL の場合、トップレベル entity に **`generic (runner_cfg : string)`** が存在するかどうかで、ファイルをテストベンチとしてスキャン・実行するかを判断します。generic 名は必ず `runner_cfg` である必要があります。存在しない場合は警告が出て、そのテストベンチはスキップされます。([Sigasi](https://www.sigasi.com/knowledge/how_tos/vunit-integration/?utm_source=chatgpt.com "VUnit projects in Sigasi Visual HDL"))
    
- **Python ランナーから VHDL へ渡される設定パケット**  
    Python 側で `vu.main()` が各シミュレーションを起動する際、この generic に **Base64 でエンコードされた “キー:値” 形式の文字列** を自動的に注入します。文字列には
    
    - 実行するテストケースのリスト
        
    - ログや結果を出力するディレクトリ
        
    - シード値・カラー設定などのオプション  
        が含まれ、VHDL 側で `test_runner_setup` がデコードして内部ランナーを初期化します。([vunit.github.io](https://vunit.github.io/run/user_guide.html?utm_source=chatgpt.com "Run Library User Guide - VUnit documentation"), [vunit.github.io](https://vunit.github.io/blog/2023_04_01_vunit_phases.html?utm_source=chatgpt.com "VUnit Phases - VUnit documentation"))
        
- **スタンドアロン実行への配慮**  
    テストベンチを手動シミュレーションしたい場合や、IDE が VUnit を呼ばない場合に備えて
    
    ```vhdl
    generic (runner_cfg : string := runner_cfg_default);
    ```
    
    とデフォルト値 `runner_cfg_default`（VUnit が提供）を設定しておけば、空設定でビルドできます。([vunit.github.io](https://vunit.github.io/run/user_guide.html?utm_source=chatgpt.com "Run Library User Guide - VUnit documentation"))
    

---

### 典型的な使い方（VHDL 側）

```vhdl
library vunit_lib;
context vunit_lib.vunit_context;  -- 重要

entity tb_example is
  generic (
    runner_cfg : string := runner_cfg_default   -- ★ここ
  );
end entity;

architecture tb of tb_example is
  -- テスト対象 DUT を宣言
begin
  main : process
    variable runner : test_runner_t;
  begin
    -- 1) ランナー初期化（runner_cfg を渡す）
    test_runner_setup(runner, runner_cfg);

    -- 2) テストケース
    if run("simple_add") then
      -- 期待値と比較 …
    elsif run("overflow_check") then
      -- 別のシナリオ …
    end if;

    -- 3) 後始末
    test_runner_cleanup(runner);
  end process;
end architecture;
```

- **`test_runner_setup`** が `runner_cfg` を解析し、どのテストケースを実行するか決定します。
    
- **ネストしたテストスイート**を生成するときは、子エンティティでは generic 名を `nested_runner_cfg` に変える慣例があります（親と区別するため）。([vunit.github.io](https://vunit.github.io/py/vunit.html?utm_source=chatgpt.com "vunit.ui - VUnit documentation"))
    

---

### 典型的な使い方（Python 側）

```python
from vunit import VUnit

vu = VUnit.from_argv()
lib = vu.add_library("lib")
lib.add_source_files("tb_example.vhd")

vu.main()     # ここで自動的に --generic=tb_example.runner_cfg="..." を付与
```

- Python スクリプトで **`runner_cfg` を明示的にセットする必要はありません**。
    
- 逆に、自作でシミュレータを呼び出すスクリプトを書く場合は同じ generic を付与すれば VUnit 機能が有効になります（例：GHDL なら `--generic=runner_cfg=...`）。
    

---

### よくある質問

|質問|回答|
|---|---|
|**長い文字列だけどシミュレータは大丈夫？**|近年の GHDL/ModelSim/Riviera-PRO などは数 kB の generic 文字列を扱えますが、古いバージョンだと制限に当たることがあります。シミュレータのリリースノートを確認してください。([GitHub](https://github.com/VUnit/vunit/issues/864?utm_source=chatgpt.com "The max. length of runner_cfg is defined by runner_cfg_default ..."))|
|**`runner_cfg` を変更して自分用のパラメータを追加したい**|追加キーを Python 側で設定しテストベンチで読み出すこともできますが、公式には VUnit API が現状公開しているキーのみを想定しています。独自パラメータは別 generic を設け、Python で `tb.set_generic("my_cfg", "value")` する方が安全です。|
|**`runner_cfg` が無い小規模テストを VUnit に取り込みたい**|entity 名を `tb_` で始めるか `_tb` で終わるようにし、上記の generic を 1 行追加するだけで最小限の移植ができます。既存の `assert false report "End of simulation"` などの終了コードを削除し `test_runner_cleanup` に置き換えてください。([vunit.github.io](https://vunit.github.io/user_guide.html?utm_source=chatgpt.com "User Guide - VUnit documentation"))|

---

#### まとめ

- `runner_cfg` は **Python ランナーから VHDL テストベンチへ設定情報を渡すための文字列 generic**。
    
- この generic の存在で VUnit はテストベンチを自動検出・制御する。
    
- VHDL テストベンチ側では `test_runner_setup` で解析し、`run` マクロで個別テストを実行、`test_runner_cleanup` で終了する。
    
- Python 側は何も意識せずに `vu.main()` を呼ぶだけで OK。
    
- スタンドアロン動作や独自拡張をしたい場合はデフォルト値や別 generic を活用。
    

これで `runner_cfg` の役割と使い方の全体像がつかめるはずです。
