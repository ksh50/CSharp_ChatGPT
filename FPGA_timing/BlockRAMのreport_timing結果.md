## USER
VivadoでFPGA内部のDualPortRAMの遅延を出力するには、どのようなコマンドを使えばよいですか？

## ASSISTANT
VivadoでFPGA内部のDualPort RAM（例えば `block RAM`）の遅延を出力したい場合、`report_timing` コマンドを使って、RAMにアクセスする読み書きのパスのタイミングを確認できます。以下に基本的なアプローチを紹介します。

---

## 【1】DualPort RAM遅延の種類
DualPort RAM に関する遅延には、次のような種類があります：

- **書き込み遅延**（例：レジスタ → RAM入力 → 書き込み）
- **読み出し遅延**（例：RAM出力 → レジスタ）
- **アドレス・制御信号の遅延**（例：アドレス信号がRAMに届くまで）

---

## 【2】コマンドの基本形

以下のような `report_timing` コマンドを使って、DualPort RAM へのアクセス遅延を確認します：

```tcl
report_timing -from [get_pins -hier -filter {NAME =~ "*wr_data_reg*/Q"}] \
              -to   [get_pins -hier -filter {REF_PIN_NAME == "WE"}]
```

これは一例で、「レジスタからRAMのWE（Write Enable）ピンまでの遅延」を測るものです。

---

## 【3】RAMセルを直接指定したい場合

VivadoでDualPort RAMを推論（またはIP）で使っている場合、RAMセルは `RAMB36E2` や `RAMB18E2` といったプリミティブでインスタンス化されています。それらの **ポート名やピン名** を使って以下のように指定できます：

```tcl
report_timing -from [get_pins -hier -filter {NAME =~ "*wr_data_reg*/Q"}] \
              -to   [get_pins -hier -filter {NAME =~ "*RAMB36E2_inst*/DOUTB*"}]
```

RAMの出力（読み出し）に関しては、`DOUTA`, `DOUTB` などの出力ピンが使われます。

---

## 【4】RAMのポートを特定するには？

下記のようにすると、RAMのセルやピンを探索できます：

```tcl
get_cells -hier -filter {PRIMITIVE_TYPE == "RAMB36E2"}
get_pins -of_objects [get_cells -hier -filter {PRIMITIVE_TYPE == "RAMB36E2"}]
```

---

## 【5】レジスタからレジスタのように指定する例

書き込みパス（レジスタ → RAM）の遅延：
```tcl
report_timing -from [get_pins -hier -filter {NAME =~ "*wr_data_reg*/Q"}] \
              -to   [get_pins -hier -filter {REF_PIN_NAME == "DINB*"}]
```

読み出しパス（RAM → レジスタ）の遅延：
```tcl
report_timing -from [get_pins -hier -filter {REF_PIN_NAME == "DOUTA*"}] \
              -to   [get_pins -hier -filter {NAME =~ "*rd_data_reg*/D"}]
```

---

## 【補足】
- `-delay_type min` や `-delay_type max` オプションを追加すると最小または最大遅延を調べられます。
- RTLの抽象名ではなく、合成・配置後の具体的な名前（inst_〜やRAMB36E2インスタンス名）を使う必要があります。

---

もし具体的なRAMインスタンス名や構成、どのような遅延を知りたいかがわかれば、もっと正確なコマンドを提案できます。必要であれば、Netlist ViewerなどでRAM周辺の構造を可視化してから分析するのも良い手段です。