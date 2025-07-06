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

    public int TotalExpToReachLevel(int level)
    {
        int baseLevel = level - 1;
        return baseLevel * (2 * BaseLevelExp + (baseLevel - 1) * NextLevelModifier) / 2;
    }

    // Upper bound binary search
    public int GetLevelFromTotalPoints(int totalPoints)
    {
        int low = 1;
        int high = MaxLevel;

        while (low < high)
        {
            int mid = (low + high + 1) / 2;
            int requiredExp = TotalExpToReachLevel(mid);

            if (totalPoints >= requiredExp)
            {
                low = mid;
            }
            else
            {
                high = mid - 1;
            }
        }

        return low;
    }
}