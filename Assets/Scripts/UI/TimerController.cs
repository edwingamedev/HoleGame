using EdwinGameDev.EventSystem;
using TMPro;
using UnityEngine;

namespace EdwinGameDev.UI
{
    public class TimerController : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;

        private void OnEnable()
        {
            GlobalEventDispatcher.AddSubscriber<Events.OnMatchTimerUpdated>(DisplayTime);
        }

        private void OnDisable()
        {
            GlobalEventDispatcher.RemoveSubscriber<Events.OnMatchTimerUpdated>(DisplayTime);
        }

        private void DisplayTime(Events.OnMatchTimerUpdated eventData)
        {
            int minutes = Mathf.FloorToInt(eventData.Time / 60f);
            int seconds = Mathf.FloorToInt(eventData.Time % 60f);
            SetTimer($"{minutes:00}:{seconds:00}");
        }
        
        private void SetTimer(string timerValue)
        {
            timerText.SetText(timerValue);
        }
    }
}