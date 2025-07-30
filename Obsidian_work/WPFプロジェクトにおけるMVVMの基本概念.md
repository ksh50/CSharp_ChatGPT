#ChatGPT #WPF 

```
依頼：
WPFプロジェクトでMVVMを使用する際に事前に知っておくべき概念を教えてください。
```

#### WPF × MVVM に入る前に押さえておきたい 15 のキーワード

> 「MVVM を書く」とは、**WPF が持つデータバインディング／ルーティング仕組み**を ViewModel へ流し込み、ロジックと UI を疎結合に保つこと。  
> 逆に言えば、ここに挙げる基礎を把握しておかないと _“なぜ分離できるのか”_ が腹落ちしづらくなります。

---

### 1. XAML の仕組みと名前空間

- **部分クラス**で裏側に `.xaml.cs` が生成される
    
- `xmlns:` による型解決／カスタム名前空間の追加方法
    
- ランタイムでは `Application.LoadComponent()` が XAML を膨らませて CLR オブジェクトを構築
    

### 2. Dependency Property (DP) システム

- `Register()`／`RegisterAttached()` による宣言
    
- **プロパティメタデータ**（既定値・PropertyChangedCallback）
    
- DP ≒ バインディング対象の“足場”
    

### 3. データバインディング基礎

- `DataContext` 伝播ルール
    
- バインディング式：`{Binding Path=..., Mode=..., UpdateSourceTrigger=...}`
    
- **OneWay / TwoWay / OneTime** 動作差
    
- `IValueConverter` / `IMultiValueConverter` で型変換・複合値
    

### 4. INotifyPropertyChanged & ObservableCollection

- ViewModel で UI へ即時通知する標準プロトコル
    
- コレクション変更イベント（`CollectionChanged`）が ItemsControl を更新
    

### 5. ICommand と RoutedCommand

- ボタン等の **CommandSource** が ICommand を呼ぶフック
    
- `CanExecute` が自動でボタンの Enable/Disable に反映
    
- 既製 `RoutedUICommand` vs. MVVM 向け `RelayCommand`／`DelegateCommand`
    

### 6. View / ViewModel / Model の責務分離

|層|主な役割|書くもの|
|---|---|---|
|**View (.xaml)**|画面レイアウト・アニメ・テーマ|XAML、Binding 式|
|**ViewModel**|表示状態・コマンド橋渡し|プロパティ＋非同期処理／バリデーション|
|**Model**|ドメインロジック・永続化|POCO, Repository|

### 7. リソース辞書・スタイル・テンプレート

- `StaticResource` / `DynamicResource` で再利用
    
- **DataTemplate** により _型⇔UI_ のマッピングを宣言的に定義
    
- `ControlTemplate` でコントロールの外観を丸ごと置換
    

### 8. Validation 機構

- `ValidationRule`（バインディング単位）
    
- `IDataErrorInfo` / `INotifyDataErrorInfo`（ViewModel 単位）
    
- `ValidatesOnNotifyDataErrors=True` の使い分け
    

### 9. スレッド & Dispatcher

- UI スレッド制約と `Dispatcher.Invoke/BeginInvoke`
    
- `async/await` で長時間処理を非同期化し **UI フリーズ** を防止
    

### 10. Event Aggregator / Messenger

- 画面間通信を **Publish–Subscribe** で疎結合化
    
- Prism の `IEventAggregator`、CommunityToolkit の `WeakReferenceMessenger` など
    

### 11. DI／IoC コンテナ

- ViewModel 生成と依存解決をフレームワークへ委譲
    
- **Prism.Unity**, **Microsoft.Extensions.DependencyInjection** など
    

### 12. Navigation と Composition

- `ContentControl`, `Frame`＋`Page` で画面遷移
    
- Prism の NavigationService、Caliburn.Micro の Conductor パターン
    

### 13. Design-time Data & Blend

- `d:DataContext`、`d:DesignInstance` を使った **デザイナ専用モック**
    
- Blend の VisualStateManager で状態遷移 UI を宣言的に
    

### 14. Source Generator / MVVM Toolkit

- **CommunityToolkit.Mvvm** の `[ObservableProperty]`, `[RelayCommand]`
    
- C# 9+ の **ソースジェネレータ**がボイラープレートを自動生成
    

### 15. テスト戦略

- ViewModel 単体テスト（xUnit + Moq）
    
- UI 自動テスト：WinAppDriver、WPF-UI、SpecFlow など
    
- **MVVM 分離**がテスト容易性を確保する核心
    

---

#### まとめ & 次の学習ステップ

1. **順序立てて習得**
    
    - ①WPF 基礎（DependencyProperty／Binding）→ ②MVVM 役割分担 → ③ツールキット応用
        
2. **実践で定着**
    
    - 小規模アプリでも MVVM で “通知・コマンド・バリデーション” を一周させる
        
3. **ツールを味方に**
    
    - CommunityToolkit.Mvvm や Prism を導入し、定型実装を減らしてロジックに集中
        

これら 15 項目を押さえておけば、WPF アプリを **MVVM で設計する動機と手順** がクリアになり、実装へスムーズに入れるはずです。