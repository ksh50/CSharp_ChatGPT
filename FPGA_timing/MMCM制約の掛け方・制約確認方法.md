## USER
AMD社製FPGAの制約ファイル記述について教えてください。

FPGA内蔵のMMCMを使う場合、FPGAのクロック入力ポートのみにcreate_clock指定することで、create_generated_clockを使うわないでも自動でMMCM出力に制約がかかるとの話を聞きました。

この情報に従うと、create_clockで入力クロック指定だけすれば入出力ポートのタイミング制約がかかる、との認識であってますか？

仮にcreate_clockで入力ポートに制約をかけ、追加でcreate_generated_clockで制約をかけた場合、自動でかかった制約には何等か悪影響が出ますか？

また、実際に掛けられたcreate_clock入力クロック制約、派生クロックにかかったcreate_generated_clock制約の内容を確認することはできますか？write_xdcで見れそうな気はしているのですが、よくわかってません。」

## ASSISTANT
AMD製FPGAにおけるタイミング制約（XDC記述）について、MMCMの扱いと`create_clock` / `create_generated_clock`の影響を整理して説明します。

---

### **1. `create_clock` のみで MMCM 出力クロックに制約が自動でかかるか？**
はい、Vivadoのタイミングエンジンは、MMCMやPLLを通じたクロックの関係を自動的に認識し、派生クロック (`generated clock`) を作成します。そのため、FPGAの外部から入力されるクロック信号 (`create_clock` で定義) のみを指定すれば、MMCMの出力クロックにも適切な制約が適用されます。

具体的には：
- `create_clock -period 10.000 [get_ports clk_in]`
  - これだけを指定すると、Vivadoは自動的にMMCMの出力クロックを認識し、内部に適切な`create_generated_clock`を作成します。

### **2. `create_generated_clock` を追加で指定するとどうなるか？**
基本的には悪影響はありませんが、**意図しない競合や制約の上書きが発生する可能性がある**ため、以下の点に注意が必要です。

- Vivadoが自動生成した制約 (`create_generated_clock`) と手動で指定した制約 (`create_generated_clock` で追加) の定義が異なる場合、Vivadoのタイミングエンジンがどの制約を優先するかによって、意図しないタイミング解析結果が出る可能性がある。
- `create_generated_clock` を手動で指定すると、クロックドメインやタイミング解析のレポートが明示的に設定されるため、タイミング解析の結果がより正確になることもある。
- ただし、MMCMの出力クロックが `get_pins` 経由で明示的に設定されると、Vivadoの自動生成と競合する場合があり、警告 (`WARNING`) やエラー (`ERROR`) が発生することがある。

したがって、**特に理由がない限りは、`create_generated_clock` を手動で追加する必要はない**と言えます。ただし、特定のクロック構造（例えば非整数分周など）で意図した動作になっているかを確認する場合、手動で追加するのが有効なケースもあります。

### **3. 実際に適用された制約を確認する方法**
適用された制約は **`write_xdc`** や **`report_clocks`** を使って確認できます。

#### **① `write_xdc` で確認**
`write_xdc` を実行すると、Vivadoが内部的に認識しているすべての制約を書き出すことができます。

```tcl
write_xdc -constraint <出力ファイル名>.xdc
```
このXDCファイル内に、Vivadoが自動生成した `create_generated_clock` のエントリがあるか確認できます。

#### **② `report_clocks` で確認**
以下のコマンドで、現在のクロック情報を表示できます。

```tcl
report_clocks
```
この出力には、`create_clock` で作成したクロックと、MMCMなどを通して自動的に派生した `generated clock` がリストされます。特に、`report_clocks` の出力には以下の情報が含まれます：
- クロック名
- 期間（周期）
- 生成元クロック
- 分周比
- デューティ比

これにより、**意図したクロック制約が適用されているかどうかを簡単に確認できます**。

---

### **まとめ**
1. **`create_clock` のみで MMCM 出力に派生クロック (`generated clock`) の制約が自動で適用される。**
2. **`create_generated_clock` を追加で手動設定すると競合や予期しない解析結果になる可能性があるため、基本的には不要。**
3. **適用されたクロック制約を確認するには、`write_xdc` でXDCファイルを書き出すか、`report_clocks` コマンドを使う。**

---

#### **補足**
- MMCMやPLLを通じたクロックに問題がある場合は、`report_timing_summary` も合わせて確認すると良いでしょう。
- `report_clocks -verbose` を実行すると、クロックの詳細情報が取得できます。

このあたりを試して、意図した動作になっているか確認してみてください！

## USER
MMCMを使用した際、FPGA入力ポートのクロックとMMCMの出力クロックの位相差は設定されているのでしょうか？

write_xdc で -sdcオプションで実行するとcreate_geberated_clockコマンドが見れるのですが下記のようになっており、MMCM入力ピンに対しての位相差は設定されているようですが、FPGAクロック入力ポート(信号名CPU_CLK)に対する位相は設定されていないように見えます。もしかしてクロック入力ポートからMMCM入力ピンまでの遅延は自動で計算されるのでしょうか？

create_generated_clock -namae clkfbout_clk_wiz_0 -source [get_pins MMCM/inst/mmcm_adv_inst/CLKIN1] -multiply_by 1 -add -master_clock [get_clocks CPU_CLK] [get_pins MMCM/inst/mmcm_adv_inst/CLKFBOUT]

set_propagated_clock [get_clocks clkfbout_clk_wiz_0]

## ASSISTANT
### **MMCM 使用時の FPGA 入力クロックと MMCM 出力クロックの位相差について**
MMCM を使用した場合、**FPGA の入力クロックと MMCM の出力クロックの間の位相差は自動的に考慮されます**。ただし、`write_xdc -sdc` で出力される `create_generated_clock` の定義を見る限り、**FPGA の入力ポート (CPU_CLK) に対する直接の位相設定は反映されていないように見える** ことがあります。

これは以下の仕組みによるものです。

---

## **1. MMCM 入力クロック (CLKIN1) と FPGA クロック入力ポート (CPU_CLK) の関係**
### **(a) MMCM の `create_generated_clock` が CPU_CLK を元クロックとして持っている**
- `create_generated_clock` の `-source` は、**MMCM の `CLKIN1` ピン**に指定されています。
- `-master_clock` は `CPU_CLK` になっているため、Vivado のタイミング解析エンジンは、**MMCM の入力クロック (`CLKIN1`) と `CPU_CLK` が同じクロックドメインであることを認識しています**。

#### **【重要】CPU_CLK から MMCM の CLKIN1 までの配線遅延**
Vivado は、FPGA の I/O バッファと内部 MMCM の `CLKIN1` までの配線遅延 (`clock insertion delay`) を**自動的に考慮**します。  
そのため、明示的な `create_generated_clock` での位相指定がなくても、**`CPU_CLK` のエッジと `CLKIN1` のエッジの間に発生する遅延は自動計算され、タイミング解析に反映されます**。

### **(b) MMCM 出力クロックの `create_generated_clock` には位相が含まれる**
- `create_generated_clock` で指定される `CLKFBOUT` は、MMCM の `CLKIN1` に対して設定されたフィードバッククロックです。
- この制約では `-add` が含まれており、`-source` で指定した `CLKIN1` に対する位相差が反映されるようになっています。
- `set_propagated_clock` が適用されているため、MMCM の出力クロックの伝播遅延も考慮されます。

#### **【重要】FPGA クロック入力 (CPU_CLK) に対する直接の位相設定がない理由**
Vivado の内部タイミングエンジンは **`CPU_CLK` → `CLKIN1` → `CLKFBOUT` → `CLKOUTx`** という経路を理解しているため、**明示的な `CPU_CLK` への位相差設定がなくても、内部的に適切に処理される** ようになっています。

---

## **2. MMCM の入力クロックと出力クロックの位相関係を確認する方法**
MMCM の出力クロックの `create_generated_clock` がどのような設定になっているかを確認する方法をいくつか紹介します。

### **(a) `report_clocks -verbose` で確認**
以下のコマンドで、Vivado が認識しているクロックの詳細情報を取得できます。

```tcl
report_clocks -verbose
```
これにより、`CPU_CLK` や `CLKIN1`、`CLKOUTx` の周波数・位相関係が一覧表示されます。

### **(b) `report_timing_summary` でタイミング情報を確認**
タイミング解析結果を確認するには、以下のコマンドを使用します。

```tcl
report_timing_summary
```
この結果を確認することで、MMCM の出力クロック (`CLKOUTx`) が `CPU_CLK` に対してどのような位相を持つか、セットアップ/ホールド時間が満たされているかを確認できます。

### **(c) `get_clocks` でクロック一覧を取得**
Vivado が認識しているクロック名を取得する場合は、以下のコマンドを使用します。

```tcl
get_clocks
```

---

## **3. 結論**
✅ **FPGA のクロック入力ポート (`CPU_CLK`) から MMCM の `CLKIN1` までの遅延は Vivado により自動計算されるため、明示的な位相設定は不要。**  
✅ **MMCM の `create_generated_clock` には `CLKIN1` に対する位相が設定されているため、タイミング解析時には適切に考慮される。**  
✅ **`report_clocks -verbose` や `report_timing_summary` を使って、MMCM の出力クロック (`CLKOUTx`) の位相や周波数を確認できる。**  
✅ **Vivado が自動で設定する値を手動で `create_generated_clock` で追加すると、意図しない影響を与える可能性があるため、特別な理由がない限り不要。**

---

### **補足**
もし、MMCM の `create_generated_clock` に特定の位相を追加したい場合は、Vivado の GUI で **Clocking Wizard** を開き、**出力クロックの位相を設定する** こともできます。  
その場合、`report_clocks` で確認すると、設定された位相が適用されていることが分かります。

この辺りを確認しながら、意図した動作になっているか試してみてください！

## USER
MMCMを使う場合には、特に指定がなくてもクロック入力ポートからMMCMのCLK1までの遅延が内部で自動計算される、とのことで承知しました。

次にクロック入力信号(CPU_CLK)に対する仮想クロックとの位相差を考えてみます。create_clockで-waveformオプションを指定せずに仮想クロックを設定し、create_clkでFPGA入力ポートのクロックも-waveformオプションを指定しない場合、CPU_CLKと仮想クロックには同相になるのでしょうか？

前提として、入力クロックCPU_CLKと仮想クロックとは同じ周波数を設定するとします。

## ASSISTANT
### **仮想クロック (`create_clock`) の `-waveform` オプションを指定しない場合の位相関係**

#### **結論**
✅ `create_clock` で `-waveform` を指定しない場合、仮想クロックと FPGA 入力クロック (`CPU_CLK`) は同じ周波数ならば **デフォルトでは同相** になります。

#### **理由**
Vivado における `create_clock` のデフォルト動作は以下の通りです：

1. **`-waveform` を省略した場合のデフォルト動作**
   - `create_clock` を使ってクロックを定義すると、デフォルトでは `0°` の位相で開始するクロックが生成されます。
   - `-waveform` を指定しない場合、デフォルトのエッジは **(0°, 50% duty cycle の場合は 50%の位相で) 自動計算** されます。

2. **仮想クロックと CPU_CLK の関係**
   - 仮想クロックと CPU_CLK を `create_clock` で設定し、`-waveform` をどちらにも指定しない場合、Vivado のデフォルト処理では両方とも **0° の位相で開始** すると解釈されるため、**同相となる**。

---

### **具体的な `create_clock` 記述例**
#### **(1) 仮想クロック**
```tcl
create_clock -period 10.000 [get_ports virt_clk]
```
- `-waveform` を指定しない → 0ns で立ち上がり、50% デューティ比なら 5ns で立ち下がる（デフォルト動作）
- 10ns 周期（100 MHz）

#### **(2) FPGA クロック入力**
```tcl
create_clock -period 10.000 [get_ports CPU_CLK]
```
- `-waveform` を指定しない → 同様に 0ns で立ち上がる
- 10ns 周期（100 MHz）

✅ **この場合、仮想クロック (`virt_clk`) と FPGA の入力クロック (`CPU_CLK`) は同じタイミングで立ち上がるため、同相（0° シフト）となる。**

---

### **仮想クロックと CPU_CLK の位相をずらす場合**
もし仮想クロックを `CPU_CLK` とは異なる位相で設定したい場合は、`-waveform` オプションを明示的に指定します。

#### **(1) `CPU_CLK` に 90° 位相ずれを適用**
```tcl
create_clock -period 10.000 -waveform {2.5 7.5} [get_ports CPU_CLK]
```
- 2.5ns で立ち上がる（90° の位相シフト）
- 7.5ns で立ち下がる（デューティ比 50%）

#### **(2) 仮想クロックはデフォルトのまま**
```tcl
create_clock -period 10.000 [get_ports virt_clk]
```
- 0ns で立ち上がる（0° の位相）

✅ **この場合、仮想クロック (`virt_clk`) と FPGA の `CPU_CLK` は 90° シフトすることになる。**

---

### **まとめ**
✅ `create_clock` で `-waveform` を指定しなければ、デフォルトで 0° の位相で開始し、仮想クロックと `CPU_CLK` は同相となる。  
✅ `-waveform` を使うことで、仮想クロックと FPGA 入力クロックの位相を明示的にずらすことができる。  
✅ `report_clocks -verbose` を使えば、設定されたクロックの波形や位相を確認できる。  

---

この仕様を踏まえて、設計意図に応じた仮想クロックの設定を試してみてください！

## USER
入力クロックCPU_CLKと仮想クロックは同相になるとのことで承知しました。

次に、set_input_delayで入力クロックCPU_CLK、及び仮想クロックに動悸したデータ入力信号に対してset_input_delay -min、-maxでsetup、holdの制限を掛けるとします。

通常、set_input_delayの -clockオプションでは仮想クロックを指定すると思います。

ここで-clockオプションで入力クロックCPU_CLKを指定する場合、仮想クロックと同相なので、同じ制約になると考えてよいでしょうか？

次に-clockオプションとして、MMCMの出力クロック（入力クロックCPU_CLK、仮想クロックに対して遅延を含むクロック）を指定した場合、制約は変化するでしょうか？

## ASSISTANT
### **`set_input_delay` の `-clock` オプションに指定するクロックによる影響**

#### **結論**
✅ **`-clock` オプションに `CPU_CLK` を指定した場合、仮想クロックと同相であれば、仮想クロックを指定するのと同じ制約が適用される。**  
✅ **`-clock` オプションに MMCM の出力クロックを指定した場合、`CPU_CLK` に対する MMCM の遅延（クロックインサーションディレイや MMCM の位相シフト）が考慮され、制約が変化する可能性がある。**

---

## **1. `-clock` に `CPU_CLK` を指定する場合**
```tcl
set_input_delay -min 1.0 -max 5.0 -clock [get_clocks CPU_CLK] [get_ports data_in]
```
- `CPU_CLK` が仮想クロックと同相であるなら、仮想クロック (`virt_clk`) を `-clock` に指定するのと **同じ結果** になる。
- `set_input_delay` の `-min` と `-max` は、指定したクロック (`-clock`) に対するデータ入力のタイミングマージンを定義するため、**クロックが同じであれば、制約も同じになる**。

✅ **この場合、仮想クロックを指定するのと `CPU_CLK` を指定するのは同等** であり、制約の適用結果に違いはない。

---

## **2. `-clock` に MMCM の出力クロックを指定する場合**
```tcl
set_input_delay -min 1.0 -max 5.0 -clock [get_clocks mmcm_clk_out] [get_ports data_in]
```
- `mmcm_clk_out` は MMCM の出力クロックであり、`CPU_CLK` に比べて遅延や位相変化が発生している可能性がある。
- **MMCM の遅延 (`clock insertion delay`) や、MMCM で設定された位相シフトの影響を受けるため、制約が変化する可能性がある。**
- 例えば、MMCM の `CLKOUTx` に対して `create_generated_clock` で `-add` や `-phase` を設定していた場合、それが反映される。

✅ **この場合、入力データの制約基準が MMCM の出力クロックになり、`CPU_CLK` とは異なるタイミング制約が適用される可能性がある。**

---

## **3. `CPU_CLK` でなく MMCM 出力クロックを `-clock` に指定するべきケース**
以下のようなケースでは、`-clock` に MMCM の出力クロックを指定するほうが適切：

1. **MMCM の `CLKOUT` を使用して I/O にデータがクロックされる場合**
   - 例えば、外部デバイスが **MMCM から出力されたクロックを基準にデータを駆動している** 場合、`set_input_delay` もそのクロック (`mmcm_clk_out`) に基づいて設定すべき。

2. **MMCM の位相が 90°/180° シフトしている場合**
   - MMCM で `CLKOUTx` の位相を 90° ずらしているなら、仮想クロック (`CPU_CLK`) を基準にした `set_input_delay` では正しくセットアップ/ホールド時間が計算されない。

3. **MMCM の周波数変換 (分周・倍周) が行われている場合**
   - MMCM で **2倍速・4倍速クロック** を生成している場合、元の `CPU_CLK` を基準に `set_input_delay` を適用すると、誤ったタイミング制約になる。

✅ **外部デバイスが MMCM の出力クロックに同期してデータを送ってくる場合、`set_input_delay` もその MMCM クロックを基準にする必要がある。**

---

## **4. どちらを指定するべきかの判断**
| `-clock` に指定するクロック | 制約の影響 | 使うべきケース |
|--------------------------|------------|--------------|
| `CPU_CLK` (または仮想クロック) | MMCM 遅延の影響なし (同相) | FPGA の I/O クロックが MMCM を通さず `CPU_CLK` で動作している場合 |
| MMCM の `CLKOUTx` | MMCM の位相・遅延が影響 | 外部デバイスが MMCM の出力クロック基準で動作する場合 |

✅ **MMCM の出力クロックが外部データの駆動クロックでないなら、仮想クロック (`CPU_CLK`) で問題なし。**  
✅ **外部データが MMCM の出力クロック基準で変化するなら、`set_input_delay` の `-clock` に MMCM の出力クロックを指定すべき。**

---

### **5. `set_input_delay` の結果を確認する方法**
指定した `set_input_delay` が意図通りに適用されているか確認するには、以下のコマンドを実行するとよい。

#### **(1) `report_timing` でタイミング制約を確認**
```tcl
report_timing -from [get_ports data_in] -to [get_ports some_output] -delay_type min
report_timing -from [get_ports data_in] -to [get_ports some_output] -delay_type max
```
この結果で、セットアップ (`-max`) / ホールド (`-min`) の制約がどのクロック基準で適用されているか確認できる。

#### **(2) `report_clocks -verbose` でクロック関係を確認**
```tcl
report_clocks -verbose
```
これで、`CPU_CLK` や `mmcm_clk_out` がどのように認識されているかをチェックできる。

#### **(3) `report_constraints` で `set_input_delay` の適用結果を確認**
```tcl
report_constraints -all_violators
```
制約が正しく適用されているか、違反が発生していないかを確認できる。

---

## **まとめ**
✅ **`CPU_CLK` と仮想クロックが同相なら、どちらを指定しても `set_input_delay` の結果は変わらない。**  
✅ **`-clock` に MMCM の出力クロックを指定すると、MMCM の遅延や位相シフトの影響を受けるため、制約が変化する可能性がある。**  
✅ **外部デバイスが MMCM の出力クロックを基準にデータを送るなら、`set_input_delay` の `-clock` も MMCM の出力クロックにするべき。**  
✅ **`report_timing`, `report_clocks -verbose`, `report_constraints` で適用結果を確認できる。**

---

この情報を元に、`set_input_delay` の `-clock` をどのクロックにするべきか、状況に応じて選択してください！