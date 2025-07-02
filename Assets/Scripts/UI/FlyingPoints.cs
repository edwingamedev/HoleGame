using TMPro;
using UnityEngine;

namespace EdwinGameDev.UI
{
    public class FlyingPoints : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void SetPoints(int points)
        {
            text.SetText($"+{points}");
        }
    }
}