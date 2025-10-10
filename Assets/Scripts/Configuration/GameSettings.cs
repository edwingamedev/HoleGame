using EdwinGameDev.Levels;
using UnityEngine;

[CreateAssetMenu(menuName = "Edwin Game Dev/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    public LevelController[] Levels;
    /// <summary>
    /// This variable value is changed on purpose, I'm using it so we can fast prototype the levels.
    /// Another way of doing it would involve saving on player prefs, static class or even a whole system for being able
    /// to access and select the level anywhere. 
    /// </summary>
    public int selectedLevel;
}