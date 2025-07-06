using UnityEngine;

namespace EdwinGameDev.UI
{
    public class LevelSelector : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private GameObject levelButtonPrefab;

        private void Start()
        {
            for (int i = 0; i < gameSettings.Levels.Length; i++)
            {
                GameObject levelButton = Instantiate(levelButtonPrefab, transform);
                ButtonSelector buttonSelector = levelButton.GetComponent<ButtonSelector>();
                buttonSelector.Setup($"Play Level {i+1}", i);
                buttonSelector.OnClick += SelectLevel;
                levelButton.SetActive(true);
            }
        }

        private void SelectLevel(int level)
        {
            gameSettings.selectedLevel = level; 
        }
    }
}