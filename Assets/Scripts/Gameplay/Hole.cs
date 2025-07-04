using System;
using System.Collections;
using UnityEngine;

namespace EdwinGameDev.Gameplay
{
    public class Hole : MonoBehaviour
    {
        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private Material lightMaterial;
        
        private Collider groundCollider;
        private MeshCollider generatedMeshCollider;

        private int currentPoints { get; set; }
        public int PointsToLevelUpThreshold => playerSettings.PointsToLevelUpThreshold;
        public int CurrentLevel { get; private set; } = 1;
        
        public event Action OnIncreaseHoleSize;
        public event Action<int> OnConsume;
        
        public void Setup(Collider groundCollider, MeshCollider generatedMeshCollider)
        {
            this.groundCollider = groundCollider;
            this.generatedMeshCollider = generatedMeshCollider;
        }
    
        public void AddPoints(Food food)
        {
            currentPoints += food.points;

            if (CurrentLevel < playerSettings.MaxLevel)
            {
                int elapsedLevel = Mathf.FloorToInt(currentPoints / PointsToLevelUpThreshold) + 1;

                if (CurrentLevel < elapsedLevel)
                {
                    SetLevel(elapsedLevel);
                }
            }
            
            OnConsume?.Invoke(food.points);
        }

        private void SetLevel(int newLevel)
        {
            CurrentLevel = newLevel;
            StartCoroutine(nameof(IncreaseSize));
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