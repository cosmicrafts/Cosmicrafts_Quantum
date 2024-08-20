/*
 * Here we save and manage the player progression
 */
public class UserProgress
{
    int Level { get; set; }

    int Xp { get; set; }

    int XpForLeveUp { get; set; }

    int BattlePoints { get; set; }

    public void InitValues(Progress progress)
    {
        Level = progress.Level;
        Xp = progress.Xp;
        BattlePoints = progress.BattlePoints;
        XpForLeveUp = GetNextXpGoal();
    }

    public int GetNextXpGoal()
    {
        return 100 * Level;
    }

    public int GetXp()
    {
        return Xp;
    }

    public int GetLevel()
    {
        return Level;
    }

    public int GetBattlePoints()
    {
        return BattlePoints;
    }

    public void AddBattlePoints(int plus)
    {
        BattlePoints += plus;
    }

    public void AddXp(int plus)
    {
        int required = XpForLeveUp - Xp;
        int bucket = plus;

        while (bucket >= required)
        {
            bucket -= required;
            LevelUp();
            required = XpForLeveUp;
        }

        Xp += bucket;
    }

    public void LevelUp()
    {
        Level++;
        Xp = 0;
        XpForLeveUp = GetNextXpGoal();
    }
}
