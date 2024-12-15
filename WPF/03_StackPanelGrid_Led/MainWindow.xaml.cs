using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace LEDDisplay
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _resetTimer; // 1秒後に元の状態に戻すためのタイマー
        private readonly Brush _defaultStrokeColor = Brushes.DimGray; // 枠線の色 (デフォルト)
        private readonly Brush _highlightStrokeColor = Brushes.Yellow; // ハイライト枠線の色
        private readonly Brush _defaultFillColor = Brushes.Red; // LED点灯時の色
        private readonly Brush _offFillColor = Brushes.Gray; // LED消灯時の色
        private const double _defaultStrokeThickness = 2; // 枠線の太さ (通常)
        private const double _highlightStrokeThickness = 4; // ハイライト時の枠線の太さ
        private List<bool> _previousLedStates; // 前回のLED状態 (true=点灯, false=消灯)
        private List<bool> _currentLedStates; // 現在のLED状態 (true=点灯, false=消灯)
        private Random _random; // 乱数生成器

        public MainWindow()
        {
            InitializeComponent();

            // LEDの状態リスト (3x3 の9個分、すべてtrueで初期化)
            _previousLedStates = new List<bool> 
            {
                true, true, true, 
                true, true, true, 
                true, true, true
            };

            _currentLedStates = new List<bool> 
            {
                true, true, true, 
                true, true, true, 
                true, true, true
            };

            _random = new Random();

            // タイマーの初期化
            _resetTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.3)
            };
            _resetTimer.Tick += ResetAllLEDs;

            // 初期状態のLEDを更新
            UpdateAllLEDs();
        }

        /// <summary>
        /// ボタンクリックでLEDの状態をランダムに変更する
        /// </summary>
        private void ToggleLEDButton_Click(object sender, RoutedEventArgs e)
        {
            // すべてのLEDの状態をランダムに変更
            for (int i = 0; i < _currentLedStates.Count; i++)
            {
                _currentLedStates[i] = _random.Next(2) == 1; // 0または1をランダムに生成してtrue/falseに変換
            }

            // すべてのLEDの状態を更新し、変更があった場合はハイライト
            UpdateAllLEDs();

            // 1秒後に枠の変化をリセットするためのタイマーをスタート
            _resetTimer.Stop(); // 既存のタイマーが動作中なら停止
            _resetTimer.Start();
        }

        /// <summary>
        /// すべてのLEDの枠線と色を現在の状態に基づいて更新する
        /// </summary>
        private void UpdateAllLEDs()
        {
            UpdateLED(led_0_0, _previousLedStates[0], _currentLedStates[0]);
            UpdateLED(led_0_1, _previousLedStates[1], _currentLedStates[1]);
            UpdateLED(led_0_2, _previousLedStates[2], _currentLedStates[2]);
            UpdateLED(led_1_0, _previousLedStates[3], _currentLedStates[3]);
            UpdateLED(led_1_1, _previousLedStates[4], _currentLedStates[4]);
            UpdateLED(led_1_2, _previousLedStates[5], _currentLedStates[5]);
            UpdateLED(led_2_0, _previousLedStates[6], _currentLedStates[6]);
            UpdateLED(led_2_1, _previousLedStates[7], _currentLedStates[7]);
            UpdateLED(led_2_2, _previousLedStates[8], _currentLedStates[8]);

            // 現在の状態を次回の"前回の状態"として保存
            _previousLedStates = new List<bool>(_currentLedStates);
        }

        /// <summary>
        /// 1つのLEDの枠線と色を現在の状態に基づいて更新する
        /// </summary>
        private void UpdateLED(System.Windows.Shapes.Ellipse led, bool previousState, bool currentState)
        {
            led.Fill = currentState ? _defaultFillColor : _offFillColor; // Onなら赤、Offなら灰色

            // 状態が変化していたら枠線をハイライト
            if (previousState != currentState)
            {
                led.Stroke = _highlightStrokeColor; // ハイライト枠線の色
                led.StrokeThickness = _highlightStrokeThickness; // 枠線の太さを4pxにする
            }
            else
            {
                ResetLED(led);
            }
        }

        /// <summary>
        /// すべてのLEDの枠線を元に戻す (1秒後にタイマーから呼び出される)
        /// </summary>
        private void ResetAllLEDs(object? sender, EventArgs e)
        {
            ResetLED(led_0_0);
            ResetLED(led_0_1);
            ResetLED(led_0_2);
            ResetLED(led_1_0);
            ResetLED(led_1_1);
            ResetLED(led_1_2);
            ResetLED(led_2_0);
            ResetLED(led_2_1);
            ResetLED(led_2_2);

            _resetTimer.Stop();
        }

        /// <summary>
        /// 指定したLEDの枠線の色と太さを元に戻す
        /// </summary>
        private void ResetLED(System.Windows.Shapes.Ellipse led)
        {
            led.Stroke = _defaultStrokeColor; // 枠線の色を黒に戻す
            led.StrokeThickness = _defaultStrokeThickness; // 枠線の太さを2pxに戻す
        }
    }
}
