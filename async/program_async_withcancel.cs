using System;
using System.Threading.Tasks;
using System.Threading;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("プログラム開始");

        // CancellationTokenSourceを作成
        CancellationTokenSource cts = new CancellationTokenSource();

        // 他のバックグラウンドタスクを並行して動作させる（キャンセル可能）
        Task backgroundTask = RunBackgroundTaskAsync(cts.Token);

        // 3つの非同期タスクを順番に実行
        await RunTaskSequenceAsync();

        // すべてのタスクが完了したらバックグラウンドタスクをキャンセル
        cts.Cancel();

        try
        {
            await backgroundTask; // バックグラウンドタスクの完了を待つ
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("バックグラウンドタスクはキャンセルされました");
        }

        Console.WriteLine("すべてのタスクが完了しました");
    }

    /// <summary>
    /// タスクを順番に実行するメソッド
    /// </summary>
    /// <returns>Task</returns>
    static async Task RunTaskSequenceAsync()
    {
        await RunTaskAsync("タスク1", 2000);
        await RunTaskAsync("タスク2", 1500);
        await RunTaskAsync("タスク3", 1000);
    }

    /// <summary>
    /// 指定された時間だけ遅延してからメッセージを表示する非同期タスク
    /// </summary>
    /// <param name="taskName">タスクの名前</param>
    /// <param name="delayMilliseconds">遅延する時間（ミリ秒）</param>
    /// <returns>Task</returns>
    static async Task RunTaskAsync(string taskName, int delayMilliseconds)
    {
        Console.WriteLine($"{taskName} 開始");
        await Task.Delay(delayMilliseconds); // 指定時間の遅延
        Console.WriteLine($"{taskName} 完了 ({delayMilliseconds} ms 後)");
    }

    /// <summary>
    /// 他のバックグラウンドタスクを実行するメソッド
    /// </summary>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns>Task</returns>
    static async Task RunBackgroundTaskAsync(CancellationToken cancellationToken)
    {
        for (int i = 0; i < 5; i++)
        {
            cancellationToken.ThrowIfCancellationRequested(); // キャンセルが要求されていたら例外をスロー
            await Task.Delay(1000, cancellationToken); // 1秒ごとにメッセージを表示
            Console.WriteLine($"バックグラウンドタスク: {i + 1} 回目の処理が完了しました");
        }
    }
}
