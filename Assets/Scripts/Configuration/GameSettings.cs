using UnityEngine;

[CreateAssetMenu(menuName = "Edwin Game Dev/GameSettings")]
public class GameSettings : ScriptableObject
{
    public float MatchDuration = 60;
    public GameObject[] Levels; 
}