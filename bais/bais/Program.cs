// 2026-11-18
// c# 프로그래밍 입문
// 학습 목표: 화면에 내가 원하는 글자가 나오게

Console.WriteLine("환영합니다");
Console.WriteLine("당신의 이름은 무엇인가요");

int hp = 100;
hp = hp - 100; //
float version = 1.01f;
bool gameClear = true;

Console.WriteLine(hp);
Console.WriteLine(version);
Console.WriteLine(gameClear);

string name = Console.ReadLine();
Console.WriteLine(name);

// 데이터 + 로직 >> 어떻게
// 변수
// 로직 : 어떻게 사용을 해야하는가

// 학습 목표 2 : 변수를 변형하여 원하는 흐름 얻기

int slimeHP = 100;
int p_Hp = 100;
int slimeAtk = 10;
int p_Atk = 20;

Console.WriteLine("(슬라임 턴)");
p_Hp = p_Hp - slimeAtk;
Console.WriteLine($"플레이어의 현재 체력은 {p_Hp} 다.");

if(gameClear)
{
    Console.WriteLine("이김");
}
