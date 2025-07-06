using CandyCoded.HapticFeedback;
using UnityEngine;
using UnityEngine.UI;

namespace EdwinGameDev.Utils
{
    public class HapticFeebackToButton : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }
    
        private void OnEnable()
        {
            button.onClick.AddListener(Feedback);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(Feedback);
        }

        private void Feedback()
        {
            HapticFeedback.HeavyFeedback();
        }
    }
}
