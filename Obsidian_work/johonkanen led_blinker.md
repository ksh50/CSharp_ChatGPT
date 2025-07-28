#VHDL #VUnit #ChatGPT

```VHDL
LIBRARY ieee  ; 
    USE ieee.NUMERIC_STD.all  ; 
    USE ieee.std_logic_1164.all  ; 
    use ieee.math_real.all;

library work;
    use work.counter_pkg.all;

library vunit_lib;
    use vunit_lib.run_pkg.all;

entity tb_counter is
  generic (runner_cfg : string);
end;

architecture vunit_simulation of tb_counter is

    signal simulation_running : boolean;
    signal simulator_clock : std_logic;
    constant clock_per : time := 1 ns;
    constant clock_half_per : time := 0.5 ns;
    constant simtime_in_clocks : integer := 50;

    signal simulation_counter : natural := 0;
    -----------------------------------
    -- simulation specific signals ----

    signal counter : counter_object_record := init_counter;
    signal slow_counter : counter_object_record := init_counter;

begin

------------------------------------------------------------------------
    simtime : process
    begin
        test_runner_setup(runner, runner_cfg);
        simulation_running <= true;
        wait for simtime_in_clocks*clock_per;
        simulation_running <= false;
        test_runner_cleanup(runner); -- Simulation ends here
        wait;
    end process simtime;	

------------------------------------------------------------------------
    sim_clock_gen : process
    begin
        simulator_clock <= '0';
        wait for clock_half_per;
        while simulation_running loop
            wait for clock_half_per;
                simulator_clock <= not simulator_clock;
            end loop;
        wait;
    end process;
------------------------------------------------------------------------

    stimulus : process(simulator_clock)

    begin
        if rising_edge(simulator_clock) then
            simulation_counter <= simulation_counter + 1;

            create_timebase_from(counter, 3);

            if counter_is_ready(counter) then
                create_timebase_from(slow_counter, 5);
            end if;
                


        end if; -- rising_edge
    end process stimulus;	
------------------------------------------------------------------------
end vunit_simulation;
```

```
質問：
このファイルがVUnitのテストフレームワークに従ってどのように定義され、どのように実行されるか解説してください。
```

### VHDL テストベンチ `tb_counter.vhd` の構成と VUnit 流の書き方

| 行番号※                            | 主な記述                                                                                                                                                                       | VUnit での役割                                                                                                               |     |
| ------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------ | --- |
| 0–3                             | `library ieee; use ieee.numeric_std.all; …`                                                                                                                                | いつもの IEEE ライブラリ読み込み。                                                                                                     |     |
| 8–9                             | `library vunit_lib; use vunit_lib.run_pkg.all;`                                                                                                                            | **VUnit 固有**。`run_pkg` にはテストランナー（`test_runner_*` 一族）とアサーション補助が入っています。                                                    |     |
| 11–13                           | `entity tb_counter is generic (runner_cfg : string); end;`                                                                                                                 | **VUnit のお約束**：テストベンチ Entity には必ず `runner_cfg` 汎用定数を生やす。VUnit 本体（後述の Python スクリプト）がここに **テスト選択・ログ設定などをエンコードした文字列**を渡します。 |     |
| 15                              | `architecture vunit_simulation of tb_counter is`                                                                                                                           | VUnit ではアーキテクチャ名は自由ですが、慣例的に `vunit_～` とすることが多いです。                                                                        |     |
| 17–29                           | シミュレーション用信号宣言（クロック, DUT 用レコードなど）                                                                                                                                           | DUT（今回はカウンタ・オブジェクト）を動かすための内部信号たち。VUnit 固有でなく通常の VHDL。                                                                    |     |
| 33–41 **simtime process**       | 1. `test_runner_setup(runner, runner_cfg);` で **テスト実行を宣言・初期化**2. 指定クロック数後に `test_runner_cleanup(runner);` で **テスト終了を宣言**                                                   | ここが **VUnit がテスト結果を Pass/Fail 判定する境界**になります。                                                                             |     |
| 44–58 **sim_clock_gen process** | `simulator_clock` を 1 ns/0.5 ns で Toggle – DUT へ与えるクロック源。                                                                                                                  |                                                                                                                          |     |
| 59–71 **stimulus process**      | カウンタに対して `create_timebase_from`, `counter_is_ready` など (おそらく `counter_pkg` 由来) を呼び出し、DUT の振る舞いを検証。失敗時は内部で `assert … report … severity error;` のように書けば VUnit が Fail と認識します。 |                                                                                                                          |     |
| 73                              | `end vunit_simulation;`                                                                                                                                                    | テストベンチ完了。                                                                                                                |     |

※行番号は読みやすさのために概数です。

---

#### 実行フロー（Python ↔ VHDL の連携）

1. **Python の “テストスイート” 定義**
    
    ```python
    # run.py
    from pathlib import Path
    from vunit import VUnit
    
    ui = VUnit.from_argv()               # `python run.py -v` などで CLI 引数を解釈
    lib = ui.add_library("lib")          # 任意のライブラリ名で OK
    
    src_dir = Path(__file__).parent
    lib.add_source_files(src_dir / "*.vhd")  # counter_pkg.vhd, tb_counter.vhd 等を登録
    
    ui.main()                            # ----> 以降は VUnit が自動制御
    ```
    
2. **VUnit が行うこと**
    
    1. GHDL（または ModelSim/Questa 等）へコンパイルコマンドを発行。
        
    2. 各テストベンチ（ここでは `tb_counter` だけ）を起動するとき、
        
        - `--generic runner_cfg="…"`, `--assert-level`, `--output-path` などを付与。
            
    3. シミュレーションが `test_runner_setup`～`test_runner_cleanup` の区間で正常終了し、assert 失敗が無ければ **PASS** を返す。  
        失敗があれば **FAIL** として Python 側に収集。
        
    4. 結果をコンソール／JUnit XML／HTML レポートにまとめて終了。
        
3. **テストケースを増やす場合**
    
    - VHDL 内で
        
        ```vhdl
        if run("fast_counter") then
           -- ...
        elsif run("slow_counter") then
           -- ...
        end if;
        ```
        
    - Python 側は追加不要。`vunit run.py -t fast_counter` などで個別実行可能です。
        

---

### まとめ

- **VHDL テストベンチ側**
    
    - `library vunit_lib; use vunit_lib.run_pkg.all;` で VUnit API を呼び込む。
        
    - `generic (runner_cfg : string)` を必ず宣言。
        
    - `test_runner_setup` / `test_runner_cleanup` でテストの開始・終了を明示。
        
    - DUT への刺激・アサーションは通常の VHDL で記述。
        
- **Python スクリプト側**
    
    - ライブラリ登録・ソース登録・`ui.main()` の３ステップだけで OK。
        
    - テスト名の列挙・フィルタリング・再実行指示など CLI で柔軟に操作可能。
        

この分業により、**テストシナリオ（刺激 & アサーション）は VHDL、実行管理・ログ生成・CI 連携は Python (VUnit)** という責務分離が実現します。