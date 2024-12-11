using System;
using System.Threading.Tasks;
using System.Threading;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("プログラム開始");

        // 他のバックグラウンドタスクを並行して動作させる
        Task backgroundTask = RunBackgroundTaskAsync();

        // 3つの非同期タスクを順番に実行
        await RunTaskSequenceAsync();

        await backgroundTask; // バックグラウンドタスクの完了を待つ

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
    /// <returns>Task</returns>
    static async Task RunBackgroundTaskAsync()
    {
        for (int i = 0; i < 5; i++)
        {
            await Task.Delay(1000); // 1秒ごとにメッセージを表示
            Console.WriteLine($"バックグラウンドタスク: {i + 1} 回目の処理が完了しました");
        }
    }
}
