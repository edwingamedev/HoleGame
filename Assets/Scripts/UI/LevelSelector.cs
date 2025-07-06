using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    public void SelectLevel(int level)
    {
        gameSettings.selectedLevel = level;
    }
}
