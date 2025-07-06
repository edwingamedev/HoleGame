using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.Gameplay
{
    public class Hole : MonoBehaviour
    {
        // Settings
        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private Material lightMaterial;

        private Collider groundCollider;
        private MeshCollider generatedMeshCollider;

        // Leveling
        private int TotalPoints { get; set; }
        public int CurrentLevel { get; private set; } = 1;
        public int ProjectedLevel => CurrentLevel + levelUpQueue.Count;
        public int ExpOnCurrentLevel => TotalPoints - playerSettings.TotalExpToReachLevel(CurrentLevel);
        public int ExpToLevelUp => playerSettings.ExpNeededOnLevel(CurrentLevel);

        private readonly Queue<int> levelUpQueue = new();
        private bool isLevelingUp;

        // Callbacks
        public event Action OnIncreaseHoleSize;
        public event Action<int> OnConsume;
        public event Action<int> OnLevelUp;

        public void Setup(Collider groundCollider, MeshCollider generatedMeshCollider)
        {
            this.groundCollider = groundCollider;
            this.generatedMeshCollider = generatedMeshCollider;
        }

        public void AddPoints(Food food)
        {
            TotalPoints += food.points;

            if (ProjectedLevel < playerSettings.MaxLevel)
            {
                CheckLevelUp();
            }

            OnConsume?.Invoke(food.points);
        }

        private void CheckLevelUp()
        {
            int newLevel = playerSettings.GetLevelFromTotalPoints(TotalPoints);

            if (ProjectedLevel < newLevel)
            {
                LevelUp(newLevel);
            }
        }

        private void LevelUp(int newLevel)
        {
            int levelsToGain = newLevel - ProjectedLevel;

            for (int i = 0; i < levelsToGain; i++)
            {
                levelUpQueue.Enqueue(1);
            }

            if (isLevelingUp)
            {
                return;
            }

            StartCoroutine(ProcessLevelUps());
        }

        private IEnumerator ProcessLevelUps()
        {
            isLevelingUp = true;

            while (levelUpQueue.Count > 0)
            {
                levelUpQueue.Dequeue();

                CurrentLevel++;

                OnLevelUp?.Invoke(CurrentLevel);

                yield return IncreaseSize();

                yield return new WaitForSeconds(0.1f);
            }

            isLevelingUp = false;
        }

        private IEnumerator IncreaseSize()
        {
            Vector3 startSize = transform.localScale;
            float growDuration = 0.4f;
            float growMultiplier = 1.2f;
            Vector3 endSize = transform.localScale * growMultiplier;
            float fixedY = transform.position.y;

            float elapsedTime = 0;
            while (elapsedTime < growDuration)
            {
                elapsedTime += Time.deltaTime;
                float time = Mathf.Clamp01(elapsedTime / growDuration);
                transform.localScale = Vector3.Lerp(startSize, endSize, time);

                Vector3 pos = transform.position;
                pos.y = fixedY;

                float alpha = Mathf.Sin(time * Mathf.PI) * 0.3f;
                Color color = lightMaterial.GetColor("_Color");
                color.a = alpha;
                lightMaterial.SetColor("_Color", color);

                yield return null;
            }

            OnIncreaseHoleSize?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            Physics.IgnoreCollision(other, groundCollider, true);
            Physics.IgnoreCollision(other, generatedMeshCollider, false);
        }

        private void OnTriggerExit(Collider other)
        {
            Physics.IgnoreCollision(other, groundCollider, false);
            Physics.IgnoreCollision(other, generatedMeshCollider, true);
        }
    }
}