using EdwinGameDev.EventSystem;
using UnityEngine;

namespace EdwinGameDev.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas menuCanvas;

        private void OnEnable()
        {
            GlobalEventDispatcher.AddSubscriber<Events.GameStarted>(GameStarted);
            GlobalEventDispatcher.AddSubscriber<Events.GameEnded>(GameEnded);
        }

        private void OnDisable()
        {
            GlobalEventDispatcher.RemoveSubscriber<Events.GameStarted>(GameStarted);
            GlobalEventDispatcher.RemoveSubscriber<Events.GameEnded>(GameEnded);
        }

        private void GameStarted(Events.GameStarted _)
        {
            menuCanvas.enabled = false;
        }

        private void GameEnded(Events.GameEnded _)
        {
            menuCanvas.enabled = true;
        }
    }
}