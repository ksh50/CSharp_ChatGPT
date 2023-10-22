#include <stdio.h>
#include <string.h> // for strtok
#include <stdlib.h> // for strtol

int main()
{
    char input[] = "d 01ab cdef"; // 解析する文字列
    char *endPtr;
    char *token;

    printf("Hello World!\n");

    token = strtok(input, " ");

    while (token != NULL)
    {
        long int number = strtol(token, &endPtr, 16);

        if (*endPtr == '\0')
        {
            printf("%s,%d,%X,%x\n", token, number, number, number);
        }
        else
        {
            printf("%s\n", token);
        }

        // 次のトークンに進む
        token = strtok(NULL, " ");
    }

    return 0;
}