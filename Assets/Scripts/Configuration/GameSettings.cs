using EdwinGameDev.Levels;
using UnityEngine;

[CreateAssetMenu(menuName = "Edwin Game Dev/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    public LevelController[] Levels;
    public int selectedLevel;
}