using System.Collections;
using EdwinGameDev.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace EdwinGameDev.UI
{
    public class ExperienceBar : MonoBehaviour
    {
        public Image imageFill;
        private Hole hole;
        private float fillSpeed = 4f;
        private int experience;
        private Coroutine fillRoutine;

        public void SetHole(Hole hole)
        {
            this.hole = hole;
            hole.OnConsume += HoleOnOnConsume;
        }

        private void HoleOnOnConsume(int points)
        {
            experience += points;

            float progress = (experience % hole.SizeThreshold) / (float)hole.SizeThreshold;

            if (fillRoutine != null)
                StopCoroutine(fillRoutine);

            fillRoutine = StartCoroutine(SmoothFill(progress));
        }

        private IEnumerator SmoothFill(float target)
        {
            float start = imageFill.fillAmount;

            float time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime * fillSpeed;
                imageFill.fillAmount = Mathf.Lerp(start, target, time);
                yield return null;
            }

            imageFill.fillAmount = target;
        }
        
    }
}
