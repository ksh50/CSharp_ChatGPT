#ChatGPT #VUnit #VHDL 

```
質問：
以下のような非常にシンプルなVUnitのテストベンチがあるとします。 質問ですが、VUnitから呼び出さずにこのテストベンチを単独でシミュレーションさせることは可能でしょうか？
--- 
library vunit_lib; 
context vunit_lib.vunit_context; 
entity tb_minimal is 
	generic( runner_cfg : string); 
end entity; 

architecture tb of tb_minimal is 
begin 
	test_runner : process 
	begin 
		test_runner_setup(runner, runner_cfg); 
		check_equal(to_string(17), "17"); 
		test_runner_cleanup(test_runner); 
	end process ; 
end architecture ;
```

はい、**VUnitを使わずにこのテストベンチを単独でシミュレーションすることは可能です**。  
ただし、以下の注意点と変更が必要です。

---

## 1. 問題点

提示されたテストベンチは以下の点で**VUnit依存**となっています:

- `vunit_lib.vunit_context` に含まれる手続き・型 (`test_runner_setup`, `test_runner_cleanup`, `check_equal`, `runner_cfg` など) を使用。
    
- `runner_cfg` はVUnitのPythonスクリプトから渡される**テストランナー設定用の文字列**。
    

これらのため、**そのままでは単独シミュレーションでエラー**になります。

---

## 2. 単独シミュレーションするための修正方法

### (A) `runner_cfg` を不要化

`runner_cfg` はVUnit実行時にPython側から設定されますが、単独シミュレーションでは固定値を与えることで解決できます。

```vhdl
entity tb_minimal is
  generic (runner_cfg : string := "");
end entity;
```

### (B) VUnit API呼び出し部分を削除または簡易化

`test_runner_setup`/`cleanup`はテスト管理のためのVUnit APIなので、単独実行時は不要です。  
以下のようにコメントアウトし、`check_equal`だけを残すか、VHDL標準の`assert`に置き換えます。

```vhdl
architecture tb of tb_minimal is
begin
  test_runner : process
  begin
    -- VUnit関連の初期化をスキップ
    -- test_runner_setup(runner, runner_cfg);

    -- VUnitのcheck_equalを使う場合
    -- check_equal(to_string(17),  "17");

    -- VHDL標準assertに置き換える場合
    assert to_string(17) = "17"
      report "Mismatch: expected 17"
      severity error;

    -- VUnitの終了処理もスキップ
    -- test_runner_cleanup(test_runner);

    wait; -- シミュレーション停止防止
  end process;
end architecture;
```

---

## 3. シミュレーションコマンド例

GHDLを用いる場合:

```bash
ghdl -a tb_minimal.vhd
ghdl -e tb_minimal
ghdl -r tb_minimal --vcd=wave.vcd
```

ModelSim/Questaの場合:

```tcl
vcom tb_minimal.vhd
vsim work.tb_minimal
run -all
```

---

## 4. まとめ

- VUnit依存部分 (`runner_cfg`, `test_runner_setup/cleanup`, `check_equal`) を削除・置換すれば、通常のVHDLテストベンチとして単独シミュレーション可能。
    
- 逆に、**VUnit機能をそのまま使う場合はVUnit Pythonランナー経由が必要**。
    

---

✅ **結論:**  
テストベンチからVUnit専用APIを外せば、通常のVHDLテストベンチとして単独シミュレーションできる。

---

必要なら「VUnit依存部分を残したまま、単独シミュレーション可能にする方法（ダミーrunner_cfg注入やcontext利用）」についても詳しく解説できますが、それも知りたいですか？


[[VUnit依存部残しながらVHDL単独シミュレーション手順]]

[[VUnitハイブリッドシミュレーションのメリット]]

