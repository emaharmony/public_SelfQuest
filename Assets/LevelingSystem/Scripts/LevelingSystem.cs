namespace SelfQuest;

public virtual class LevelingSystem
{
    
    int _exp = 0; 
    public int overallLvl;
    public int currExp { get { return _exp; }  set { _exp = 0; AddEXP(value); } }
    public int nextLvlEXP; // cumulative XP required to reach the next level

    // RuneScape-like leveling constants
    const int MAX_LEVEL = 99;
    const int MAX_XP = 13034431; // RS max XP to reach level 99


    public void InitalizeLevels()
    {
        // Normalize level into valid bounds
        if (overallLvl < 1) overallLvl = 1;
        if (overallLvl > MAX_LEVEL) overallLvl = MAX_LEVEL;

        // Initialize next level threshold using RuneScape XP curve
        nextLvlEXP = GetNextLevelThreshold(overallLvl);
    }
    
    public void AddEXP(int exp)
    {
        // Clamp total EXP to MAX_XP
        long newExp = (long)_exp + exp;
        if (newExp > MAX_XP) newExp = MAX_XP;
        if (newExp < 0) newExp = 0; // safety
        _exp = (int)newExp;

        if (_exp >= nextLvlEXP && overallLvl < MAX_LEVEL)
            StartCoroutine(LevelUp());
    }
    
    public IEnumerator LevelUp()
    {
        while (overallLvl < MAX_LEVEL && currExp >= nextLvlEXP)
        {
            overallLvl++;
            if (overallLvl >= MAX_LEVEL)
            {
                // At max level, the threshold stays at MAX_XP
                nextLvlEXP = MAX_XP;
                break;
            }
            nextLvlEXP = GetNextLevelThreshold(overallLvl);
            yield return null;
        }

        yield return null;
    }
    
    // Returns the cumulative XP required to reach the NEXT level after currentLevel,
    // capped to MAX_XP. For example, if currentLevel == 1, returns XP needed for level 2 (83).
    int GetNextLevelThreshold(int currentLevel)
    {
        if (currentLevel >= MAX_LEVEL) return MAX_XP;
        int nextLevel = currentLevel + 1;
        return TotalXPForLevel(nextLevel);
    }

    // RuneScape total XP required to reach a specific level (1..99)
    // Level 1 returns 0 XP. Level 99 returns 13,034,431.
    static int TotalXPForLevel(int level)
    {
        if (level <= 1) return 0;
        if (level >= MAX_LEVEL) return MAX_XP;

        double points = 0;
        for (int lvl = 1; lvl < level; lvl++)
        {
            points += System.Math.Floor(lvl + 300.0 * System.Math.Pow(2.0, lvl / 7.0));
        }
        return (int)System.Math.Floor(points / 4.0);
    }
}