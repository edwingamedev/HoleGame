using UnityEngine;

[CreateAssetMenu(menuName = "Edwin Game Dev/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float MoveSpeed = 10;
    public float TurnSpeed = 10;
    public int PointsToLevelUpThreshold = 10;
}