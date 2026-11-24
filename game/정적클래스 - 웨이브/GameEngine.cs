namespace WaveShooterGame;

public class GameEngine
{
    private readonly int _width;
    private readonly int _height;
    private readonly Player _player;
    private readonly List<Bullet> _bullets = new();
    private readonly List<Monster> _monsters = new();
    private readonly Random _random = new();
    private bool _isRunning = true;

    public GameEngine(int width, int height, int initialMonsters)
    {
        _width = width;
        _height = height;
        _player = new Player(width / 2, height - 2);

        WaveManager.Initialize(initialMonsters);
        SpawnMonsters(WaveManager.AliveMonsters);
    }

    public void Run()
    {
        Console.CursorVisible = false;

        while (_isRunning)
        {
            ProcessInput();
            Update();
            Render();
            Thread.Sleep(60);
        }

        Console.CursorVisible = true;
    }

    private void ProcessInput()
    {
        while (Console.KeyAvailable)
        {
            ConsoleKey key = Console.ReadKey(intercept: true).Key;

            switch (key)
            {
                case ConsoleKey.W:
                    _player.Move(0, -1, _width, _height);
                    break;
                case ConsoleKey.S:
                    _player.Move(0, 1, _width, _height);
                    break;
                case ConsoleKey.A:
                    _player.Move(-1, 0, _width, _height);
                    break;
                case ConsoleKey.D:
                    _player.Move(1, 0, _width, _height);
                    break;
                case ConsoleKey.Spacebar:
                    if (_player.Y > 1)
                    {
                        _bullets.Add(_player.Shoot());
                    }
                    break;
                case ConsoleKey.Escape:
                    _isRunning = false;
                    break;
            }
        }
    }

    private void Update()
    {
        foreach (var bullet in _bullets)
        {
            bullet.Update();

            foreach (var monster in _monsters)
            {
                if (monster.IsAlive && bullet.IsActive &&
                    bullet.X == monster.X && bullet.Y == monster.Y)
                {
                    monster.Kill();
                    bullet.IsActive = false;
                    break;
                }
            }
        }

        _bullets.RemoveAll(b => !b.IsActive);
        _monsters.RemoveAll(m => !m.IsAlive);

        if (WaveManager.IsWaveCleared())
        {
            int nextCount = WaveManager.AdvanceNextWave();
            SpawnMonsters(nextCount);
        }
    }

    private void Render()
    {
        char[,] buffer = new char[_height, _width];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                bool isBorder = y == 0 || y == _height - 1 || x == 0 || x == _width - 1;
                buffer[y, x] = isBorder ? '#' : ' ';
            }
        }

        foreach (var monster in _monsters)
        {
            if (monster.IsAlive)
            {
                buffer[monster.Y, monster.X] = monster.Symbol;
            }
        }

        foreach (var bullet in _bullets)
        {
            if (bullet.IsActive)
            {
                buffer[bullet.Y, bullet.X] = bullet.Symbol;
            }
        }

        buffer[_player.Y, _player.X] = _player.Symbol;

        Console.SetCursorPosition(0, 0);
        System.Text.StringBuilder sb = new();
        sb.AppendLine($"WAVE: {WaveManager.CurrentWave} | ALIVE MONSTERS: {WaveManager.AliveMonsters}   (WASD: Move, SPACE: Shoot, ESC: Quit)");

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                sb.Append(buffer[y, x]);
            }
            sb.AppendLine();
        }

        Console.Write(sb.ToString());
    }

    private void SpawnMonsters(int count)
    {
        _monsters.Clear();
        HashSet<(int, int)> occupied = new();

        while (_monsters.Count < count)
        {
            int x = _random.Next(1, _width - 1);
            int y = _random.Next(1, _height / 2);

            if (occupied.Add((x, y)))
            {
                _monsters.Add(new Monster(x, y));
            }
        }
    }
}
