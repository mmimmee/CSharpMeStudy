using System;
using System.Collections.Generic;
using System.Text;

// 배열
// 왜 사용 하는가

int[] numbers = new int[10];
for (int i =0; i < 10; i++)
{
    numbers[i] = i;
    Console.WriteLine($"현재 값은 : {numbers[i]}");
}
Console.WriteLine(numbers[0.]);

// 갯수를 정하면 시작부터 끝까지 데이터를 저장하고 언제든지 꺼내서 쓸 수 있다.

const int ITEM_MAX_COUNT = 10;

int[] prices = new int[ITEM_MAX_COUNT];
for (int i = 0; i < ITEM_MAX_COUNT; i++)
{
    prices[i] = 10 + 10 * i;
    // ======= 아이템 테이블 표를 만드는 서식으로 변경하여라
}
Console.WriteLine("4번째 아이템의 가격");
Console.WriteLine($" {prices[3]} 입니다");

Console.WriteLine();