// static 쓰는 이유 - 정해진 속성을 쉽게 꺼내쓴다?
Enemy e1 = new Enemy("고블린");
Enemy e2 = new Enemy("고블린");
Enemy e3 = new Enemy("고블린");

e1 = e2;
e1.Name = "A";
Console.WriteLine(e1.Name);
Console.WriteLine(e2.Name);

Console.WriteLine($"몬스터가 총 몇마리 소환되었나요? {Enemy.Count}");

class Enemy
{
    public static int Count;    // 객체마다 따로 생긴다
    public string Name;        // 객체마다 하나

    public Enemy(string name)
    {
        Name = name;
        Count++;               // 모두 같은 Count
    }
}