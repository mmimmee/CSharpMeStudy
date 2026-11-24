namespace WaveShooterGame;

public static class WaveManager
{
    public static int CurrentWave { get; private set; } = 1;
    public static int AliveMonsters { get; private set; }
    private static int _baseCount;

    public static void Initialize(int initialCount)
    {
        _baseCount = initialCount;
        CurrentWave = 1;
        AliveMonsters = initialCount;
    }

    public static void OnMonsterKilled()
    {
        if (AliveMonsters > 0)
        {
            AliveMonsters--;
        }
    }

    public static bool IsWaveCleared()
    {
        return AliveMonsters <= 0;
    }

    public static int AdvanceNextWave()
    {
        CurrentWave++;
        AliveMonsters = _baseCount + (CurrentWave - 1) * 2;
        return AliveMonsters;
    }
}
