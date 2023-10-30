#include <stdio.h>
#include <string.h>

#define HISTORY_COUNT 4
#define COMMAND_MAX_LENGTH 64

char command_buffer[COMMAND_MAX_LENGTH];
char history_buffer[HISTORY_COUNT][COMMAND_MAX_LENGTH];
int current_position = -1;    // 最新のコマンド位置
int oldest_position = 0;      // 最も古いコマンド位置
int history_size = 0;         // 履歴バッファにあるコマンド数
int navigation_position = -1; // ユーザーがナビゲートしている現在の位置

void add_to_history(const char *command);
void load_command_from_history(int index);

void main(void)
{
    add_to_history("cmd1");
    add_to_history("cmd2");
    add_to_history("cmd3");
    add_to_history("cmd4");
    add_to_history("cmd5");
    add_to_history("cmd6");
    add_to_history("cmd7");
    add_to_history("cmd8");
    add_to_history("cmd9");

    load_command_from_history(current_position);
}

void add_to_history(const char *command)
{
    if (history_size > 0 && strcmp(history_buffer[current_position], command) == 0)
    {
        // 前回のコマンドと同じなら無視
        return;
    }

    // 次の位置へ
    current_position = (current_position + 1) % HISTORY_COUNT;
    if (history_size == HISTORY_COUNT)
    {
        oldest_position = (oldest_position + 1) % HISTORY_COUNT; // 古いものから削除
    }
    else
    {
        history_size++;
    }

    // コマンドを履歴に追加
    strncpy(history_buffer[current_position], command, COMMAND_MAX_LENGTH - 1);
    history_buffer[current_position][COMMAND_MAX_LENGTH - 1] = '\0'; // 終端文字を確保
}

void load_command_from_history(int index)
{
    if (index >= 0 && index < history_size)
    { // indexが履歴バッファの範囲内であることを確認
        // 現在のコマンドバッファをクリア
        memset(command_buffer, 0, COMMAND_MAX_LENGTH);

        // 選択されたコマンドをコマンドバッファにコピー
        strcpy(command_buffer, history_buffer[index]);
    }
}
