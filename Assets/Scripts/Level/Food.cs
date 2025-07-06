using System.Collections;
using EdwinGameDev.Levels;
using UnityEngine;
using CandyCoded.HapticFeedback;

namespace EdwinGameDev.Gameplay
{
    public class Food : MonoBehaviour
    {
        public int points;
        public Hole ConsumerHole { get; set; }
    
        private bool pointsGiven;
        private ILevel levelController;
        
        private void Awake()
        {
            levelController = GetComponentInParent<ILevel>();
            RegisterToStage();
        }

        private void RegisterToStage()
        {
            levelController?.RegisterFood(this);
        }
        
        private void UnregisterFromStage()
        {
            levelController?.UnregisterFood(this);
        }

        public void Consume()
        {
            if (pointsGiven || ConsumerHole == null)
            {
                return;
            }

            ConsumerHole.AddPoints(this);
            pointsGiven = true;
            
            HapticFeedback.HeavyFeedback();
            
            StartCoroutine(ShrinkAndDisable());
        }

        private IEnumerator ShrinkAndDisable()
        {
            float duration = 0.4f;
            float time = 0f;
            Vector3 originalScale = transform.localScale;

            while (time < duration)
            {
                transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            
            gameObject.SetActive(false);
            UnregisterFromStage();
        }
    }
}