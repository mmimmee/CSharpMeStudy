namespace WaveShooterGame;

public class Monster
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsAlive { get; private set; } = true;
    public char Symbol => 'M';

    public Monster(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Kill()
    {
        if (!IsAlive) return;
        IsAlive = false;
        WaveManager.OnMonsterKilled();
    }
}
