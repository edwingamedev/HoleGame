using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EdwinGameDev.Utils
{
    public class SceneLoaderToButton : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(LoadScene);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(LoadScene);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
