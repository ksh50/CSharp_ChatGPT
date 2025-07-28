#NotebookLM #CSharp #WPF 

WPFアプリケーション開発において、MVVMパターンはUI（ビュー）とロジック（モデル）を効果的に分離するための中心的な役割を担っています。ViewModelは、このMVVMパターンの「VM」にあたる部分で、ビューとモデルの間の**仲介役**として機能します。

具体的には、ViewModelは以下の役割を担います:

- **モデルのデータをビューがバインドできるように準備する**。
- **ビューからのユーザー操作（イベント）を受け取り、それをバックエンドのコード（モデル）に伝達する**。
- **UIに依存しないロジックや、特定のビューでのみ利用する状態を保持する**。

これにより、**テストの容易性**（UIに依存しないテストが可能）、**保守性の向上**（関心事の分離により変更の影響範囲が限定される）、**チーム開発の効率化**（UI担当とロジック担当の分業が可能） といったメリットが得られます。

MVVMをWPFで実装する際、`INotifyPropertyChanged`インターフェース（プロパティ値の変更通知）や`ICommand`インターフェース（コマンド処理）の実装が定型コード（ボイラープレート）になりがちです。ここで**Community Toolkit MVVM**が非常に役立ちます。このツールキットは、ソースジェネレーターを活用することで、これらの定型コードを自動生成し、**開発者が本質的なロジックに集中できるようにします**。

以下に、Community Toolkit MVVMを使用した簡単なViewModelのコード例を示します。

---

### 簡単なViewModelのコード例（カウンターアプリケーション）

この例では、数値カウンターを表現するViewModelを作成し、その値を表示するプロパティと、値を増減させるコマンドを定義します。

**1. NuGetパッケージの追加** まず、プロジェクトに`CommunityToolkit.Mvvm` NuGetパッケージを追加します。

**2. ViewModelクラスの定義**

```CSharp

using CommunityToolkit.Mvvm.ComponentModel; // ObservableObject, ObservableProperty
using CommunityToolkit.Mvvm.Input;       // RelayCommand

// ViewModelクラスはpartialである必要があります。
// Source Generatorが追加のコードを生成するためです。
public partial class CounterViewModel : ObservableObject
{
    // [ObservableProperty]属性を使用することで、
    // プロパティとその変更通知ロジックが自動的に生成されます。
    // (例: 'Count'というpublicプロパティと 'OnCountChanged'などの部分メソッド)
    // private int count; というフィールド名を小文字にすると、
    // 生成されるプロパティ名は大文字始まりの 'Count' になります。
    [ObservableProperty]
    private int count;

    // [RelayCommand]属性を使用することで、
    // このメソッドに対応するICommand実装 ('IncrementCommand') が自動的に生成されます。
    [RelayCommand]
    private void Increment()
    {
        // プロパティのセッターを通じて値を変更すると、
        // [ObservableProperty]によって自動生成された変更通知が発火します。
        Count++;
    }

    // [RelayCommand]属性は非同期メソッドにも適用できます。
    [RelayCommand]
    private void Reset()
    {
        Count = 0;
    }

    // オプション: プロパティが変更されたときに特定のロジックを実行したい場合、
    // On[PropertyName]Changed という部分メソッドを定義できます。
    // これは[ObservableProperty]が生成するフックポイントです。
    partial void OnCountChanged(int value)
    {
        // 例: カウンターが特定の値に達したらデバッグメッセージを出力
        System.Diagnostics.Debug.WriteLine($"Count changed to: {value}");
    }
}
```

**コードの説明:**

- **`public partial class CounterViewModel : ObservableObject`**
    - `partial`キーワードは、このクラスの定義が複数のファイルに分割されていることを示します。Community Toolkit MVVMの**ソースジェネレーター**は、このキーワードを利用して、ユーザーが記述したViewModelクラスに追加のコード（プロパティやコマンドの実装など）を生成します。
    - `ObservableObject`は、Community Toolkit MVVMが提供する基底クラスで、**`INotifyPropertyChanged`インターフェースを実装しています**。これにより、`Count`プロパティの値が変更された際にUIに変更を通知する定型コードを自分で書く必要がなくなります。
- **`[ObservableProperty] private int count;`**
    - `[ObservableProperty]`属性をフィールドに付与するだけで、対応する**パブリックプロパティ（`Count`）が自動的に生成されます**。この生成されたプロパティは、セッターで値が変更された際に自動的に`INotifyPropertyChanged.PropertyChanged`イベントを発火させ、UIに変更を通知します。これにより、冗長なプロパティ定義と変更通知ロジック（`if (_count != value) { _count = value; OnPropertyChanged(nameof(Count)); }`のようなコード）が不要になります。
    - `OnCountChanged`のような部分メソッドは、`[ObservableProperty]`が付与されたプロパティが変更された前後に**カスタムロジックを注入するためのフックポイント**として自動生成されます。
- **`[RelayCommand] private void Increment()`**
    - `[RelayCommand]`属性をメソッドに付与するだけで、そのメソッドを呼び出す**`ICommand`の実装（`IncrementCommand`）が自動的に生成されます**。これにより、`ICommand`インターフェースを手動で実装する手間（`DelegateCommand`などのヘルパークラスを定義したり、インスタンス化したりする手間）がなくなります。
    - このコマンドはUIのボタンなどから直接バインドして呼び出すことができます。

---

### View (XAML) との連携例 (簡略版)

上記の`CounterViewModel`をWPFのXAMLから利用する場合、以下のようにバインドを設定します。

```XML

<Window x:Class="YourApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:YourApp"
        mc:Ignorable="d"
        Title="Counter App" Height="200" Width="300">

    <Window.DataContext>
        <!-- デザイン時データコンテキストを設定することで、XAMLデザイナーでIntelliSenseが機能します -->
        <local:CounterViewModel d:DesignInstance="{d:Type local:CounterViewModel}"/>
    </Window.DataContext>

    <StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
        <TextBlock Text="{Binding Count}" FontSize="40" HorizontalAlignment="Center"/>
        <StackPanel Orientation="Horizontal" Margin="0,10,0,0">
            <Button Content="Increment" Command="{Binding IncrementCommand}" Margin="5"/>
            <Button Content="Reset" Command="{Binding ResetCommand}" Margin="5"/>
        </StackPanel>
    </StackPanel>
</Window>
```

**XAMLの説明:**

- **`<Window.DataContext>`**
    - `DataContext`プロパティは、Viewがバインドするデータソース（この場合は`CounterViewModel`のインスタンス）を設定します。これにより、`{Binding Count}`のように、ViewModelのプロパティを直接参照できるようになります。
    - `d:DesignInstance`は、デザイン時にVisual StudioのXAMLデザイナーでデータバインディングのIntelliSenseを有効にするための設定です。実際のアプリケーションの実行には影響しません。
- **`Text="{Binding Count}"`**
    - `TextBlock`の`Text`プロパティが`CounterViewModel`の`Count`プロパティに**データバインド**されています。`Count`プロパティの値が変更されると、`TextBlock`の表示も自動的に更新されます。
- **`Command="{Binding IncrementCommand}"`**
    - `Button`の`Command`プロパティが、ViewModelで`[RelayCommand]`属性によって生成された`IncrementCommand`にバインドされています。ボタンがクリックされると、`IncrementCommand`が実行され、ViewModelの`Increment`メソッドが呼び出されます。

---

このように、Community Toolkit MVVMを利用することで、WPFアプリケーションのViewModelにおけるプロパティ変更通知やコマンド実装のボイラープレートコードが大幅に削減され、より簡潔で読みやすいコードを書くことができるようになります。