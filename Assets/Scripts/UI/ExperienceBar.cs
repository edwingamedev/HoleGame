using System;
using System.Collections;
using System.Collections.Generic;
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

        private readonly Queue<Func<IEnumerator>> fillAnimationQueue = new();

        private readonly float fillSpeed = 4f;
        private int lastKnownLevel = 1;
        private bool isAnimating;
        
        private Coroutine fillRoutine;
        
        private void Awake()
        {
            imageFill.fillAmount = 0;
        }

        private void OnEnable()
        {
            hole.OnConsume += HoleOnConsume;
            hole.OnLevelUp += OnLevelUp;
        }

        private void OnDisable()
        {
            hole.OnConsume -= HoleOnConsume;
            hole.OnLevelUp -= OnLevelUp;
        }

        private void HoleOnConsume(int points)
        {
            bool reachedMaxLevel = hole.CurrentLevel >= playerSettings.MaxLevel;
        
            if (reachedMaxLevel)
            {
                imageFill.fillAmount = 1;
                return;
            }
        
            // Compare if we gained full level(s)
            int gainedLevels = hole.CurrentLevel - lastKnownLevel;
        
            if (gainedLevels > 0)
            {
                for (int i = 0; i < gainedLevels; i++)
                {
                    fillAnimationQueue.Enqueue(() => SmoothFillRoutine(1f, () =>
                    {
                        imageFill.fillAmount = 0f;
                    }));
                }
        
                // Fill bar to leftover exp after multiple level-ups
                int remainingPoints = hole.CurrentPoints % hole.PointsToLevelUpThreshold;
                float finalFill = remainingPoints / (float)hole.PointsToLevelUpThreshold;
        
                fillAnimationQueue.Enqueue(() => SmoothFillRoutine(finalFill, null));
            }
            else
            {
                int currentLevelThreshold = hole.PointsToLevelUpThreshold;
                float targetFill = hole.CurrentPoints % currentLevelThreshold / (float)currentLevelThreshold;
                fillAnimationQueue.Enqueue(() => SmoothFillRoutine(targetFill, null));
            }
        
            lastKnownLevel = hole.CurrentLevel;

            if (isAnimating)
            {
                return;
            }

            StartCoroutine(RunFillQueue());
        }
        
        private void OnLevelUp(int newLevel)
        {
            if (newLevel >= playerSettings.MaxLevel)
            {
                imageFill.fillAmount = 1;
                currentLevel.SetText($"LVL {newLevel}");
                return;
            }

            StartCoroutine(PlayLevelUpSequence(newLevel));
        }


        private IEnumerator PlayLevelUpSequence(int newLevel)
        {
            yield return StartCoroutine(SmoothFillRoutine(1f, null));

            currentLevel.SetText($"LVL {newLevel}");

            yield return new WaitForSeconds(0.1f);

            if (newLevel < playerSettings.MaxLevel)
            {
                yield return StartCoroutine(SmoothFillRoutine(0f, null));
            }
            else
            {
                imageFill.fillAmount = 1;
            }
        }

        
        private IEnumerator RunFillQueue()
        {
            isAnimating = true;
        
            while (fillAnimationQueue.Count > 0)
            {
                yield return StartCoroutine(fillAnimationQueue.Dequeue().Invoke());
                yield return new WaitForSeconds(0.1f);
            }
        
            isAnimating = false;
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