using EdwinGameDev.Levels;
using UnityEngine;

[CreateAssetMenu(menuName = "Edwin Game Dev/GameSettings")]
public class GameSettings : ScriptableObject
{
    public float MatchDuration = 60;
    public LevelController[] Stages;
    public int selectedLevel;
}