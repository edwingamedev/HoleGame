using System;
using System.Collections;
using EdwinGameDev.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EdwinGameDev.UI
{
    public class ExperienceBar : MonoBehaviour
    {
        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private TMP_Text currentLevel;
        [SerializeField] private Image imageFill;
        [SerializeField] private Hole hole;
        private readonly float fillSpeed = 4f;
        private int experience;
        private Coroutine fillRoutine;
        
        private void Awake()
        {
            imageFill.fillAmount = 0;
        }

        private void OnEnable()
        {
            hole.OnConsume += HoleOnOnConsume;
            hole.OnIncreaseHoleSize += LevelUp;
        }

        private void OnDisable()
        {
            hole.OnConsume -= HoleOnOnConsume;
            hole.OnIncreaseHoleSize -= LevelUp;
        }
        
        private void HoleOnOnConsume(int points)
        {
            experience += points;

            if (hole.CurrentLevel >= playerSettings.MaxLevel)
            {
                imageFill.fillAmount = 1;
                
                return;
            }
            
            if (experience >= hole.PointsToLevelUpThreshold)
            {
                experience = 0;
                AnimateFillTo(
                    targetFill: 1f,
                    onComplete: () =>
                    {
                        AnimateFillTo(0f);
                    });
                return;
            }

            float targetFill = experience / (float)hole.PointsToLevelUpThreshold;
            AnimateFillTo(targetFill);
        }

        private void LevelUp()
        {
            currentLevel.SetText($"LVL {hole.CurrentLevel}");
        }

        private void AnimateFillTo(float targetFill, Action onComplete = null)
        {
            if (fillRoutine != null)
            {
                StopCoroutine(fillRoutine);
            }

            fillRoutine = StartCoroutine(SmoothFillRoutine(targetFill, onComplete));
        }

        private IEnumerator SmoothFillRoutine(float target, Action onComplete)
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
            onComplete?.Invoke();
        }
    }
}