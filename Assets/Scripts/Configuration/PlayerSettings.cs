using UnityEngine;

[CreateAssetMenu(menuName = "Edwin Game Dev/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float MoveSpeed = 10;
    public float TurnSpeed = 10;
    public int NextLevelModifier = 7;
    public int BaseLevelExp = 10;
    public int MaxLevel = 20;
    
    public int ExpNeededOnLevel(int level) =>
        BaseLevelExp + NextLevelModifier * (level - 1);
        
    public int GetLevelFromTotalPoints(int totalPoints)
    {
        int level = 1;
        int remainingPoints = totalPoints;

        while (level < MaxLevel)
        {
            int xpToLevelUp = ExpNeededOnLevel(level);
            if (remainingPoints < xpToLevelUp)
            {
                break;
            }

            remainingPoints -= xpToLevelUp;
            level++;
        }

        return level;
    }
    
    public int TotalExpToReachLevel(int level)
    {
        int n = level - 1;
        return (n * (2 * BaseLevelExp + (n - 1) * NextLevelModifier)) / 2;
    }
}