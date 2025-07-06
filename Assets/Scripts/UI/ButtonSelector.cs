using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EdwinGameDev.UI
{
    public class ButtonSelector : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private  TMP_Text text;
        [SerializeField] private int levelToLoad;
        public event Action<int> OnClick;

        private void Start()
        {
            button.onClick.AddListener(Execute);
        }

        public void Setup(string label, int levelToLoad)
        {
            text.SetText(label);
            this.levelToLoad = levelToLoad;
        }

        private void Execute()
        {
            OnClick?.Invoke(levelToLoad);
        }
    }
}