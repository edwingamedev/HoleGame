using EdwinGameDev.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EdwinGameDev.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private FloatingPointsController pointsController;
        [SerializeField] private ExperienceBar experienceBar;
        
        public void SetupHole(Hole hole)
        {
            pointsController.SetHole(hole);
            experienceBar.SetHole(hole);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}