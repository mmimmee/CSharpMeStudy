namespace WaveShooterGame;

public class Player
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public char Symbol => 'P';

    public Player(int startX, int startY)
    {
        X = startX;
        Y = startY;
    }

    public void Move(int dx, int dy, int maxWidth, int maxHeight)
    {
        X = Math.Clamp(X + dx, 1, maxWidth - 2);
        Y = Math.Clamp(Y + dy, 1, maxHeight - 2);
    }

    public Bullet Shoot()
    {
        return new Bullet(X, Y - 1);
    }
}

public class Bullet
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsActive { get; set; } = true;
    public char Symbol => '|';

    public Bullet(int startX, int startY)
    {
        X = startX;
        Y = startY;
    }

    public void Update()
    {
        Y--;
        if (Y <= 0)
        {
            IsActive = false;
        }
    }
}
