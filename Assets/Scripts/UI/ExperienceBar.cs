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
        [SerializeField] private TMP_Text experienceText;
        [SerializeField] private Image imageFill;
        [SerializeField] private Hole hole;

        // Animation
        private readonly float fillSpeed = 4f;

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
            bool reachedMaxLevel = hole.ProjectedLevel >= playerSettings.MaxLevel;
            
            if (reachedMaxLevel)
            {
                imageFill.fillAmount = 1f;
                experienceText.SetText("MAX");
                return;
            }

            if (hole.ProjectedLevel != hole.CurrentLevel)
            {
                return;
            }

            float fillTarget = hole.ExpOnCurrentLevel / (float)hole.ExpToLevelUp;
            experienceText.SetText($"{hole.ExpOnCurrentLevel} / {hole.ExpToLevelUp}");

            StartCoroutine(SmoothFillRoutine(fillTarget, null));
        }
        
        private void ResetExpBar()
        {
            imageFill.fillAmount = 0f;
        }

        private void OnLevelUp(int newLevel)
        {
            if (newLevel >= playerSettings.MaxLevel)
            {
                imageFill.fillAmount = 1;
                currentLevel.SetText($"LVL {newLevel}");
                experienceText.SetText("MAX");
                return;
            }

            StartCoroutine(SmoothFillRoutine(1f, () =>
            {
                currentLevel.SetText($"LVL {newLevel}");
                experienceText.SetText($"{hole.ExpOnCurrentLevel} / {hole.ExpToLevelUp}");
                ResetExpBar();
                
                float fillTarget = hole.ExpOnCurrentLevel / (float)hole.ExpToLevelUp;
                StartCoroutine(SmoothFillRoutine(fillTarget, null));
            }));
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