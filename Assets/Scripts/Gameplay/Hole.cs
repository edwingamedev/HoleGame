using System;
using System.Collections;
using EdwinGameDev.Gameplay;
using UnityEngine;

namespace EdwinGameDev.Gameplay
{
    public class Hole : MonoBehaviour
    {
        [SerializeField] private HoleMovementController movementController;
        [SerializeField] private Material emission;
        
        private Collider groundCollider;
        private MeshCollider generatedMeshCollider;

        private int Points { get; set; }
        public readonly int PointsToLevelUpThreshold = 5;
        public int CurrentLevel { get; private set; } = 1;
        
        public event Action OnIncreaseHoleSize;
        public event Action<Transform> OnHoleMove;
        public event Action<int> OnConsume;
        
        public void Setup(Collider groundCollider, MeshCollider generatedMeshCollider)
        {
            this.groundCollider = groundCollider;
            this.generatedMeshCollider = generatedMeshCollider;
        
            if (movementController != null)
            {
                movementController.OnMove += OnMoveEventHandler;
            }
        }

        private void OnMoveEventHandler()
        {
            OnHoleMove?.Invoke(transform);
        }
    
        public void AddPoints(Food food)
        {
            Points += food.points;

            OnConsume?.Invoke(food.points);
            
            if (Points % PointsToLevelUpThreshold != 0)
            {
                return;
            }

            LevelUp();
        }

        private void LevelUp()
        {
            CurrentLevel++;
            StartCoroutine(nameof(IncreaseSize));
        }

        private IEnumerator IncreaseSize()
        {
            Vector3 startSize = transform.localScale;
            float growDuration = 0.4f;
            float growMultiplier = 1.2f;
            Vector3 endSize = transform.localScale * growMultiplier;
            float fixedY = transform.position.y;
            
            float time = 0;
            while (time < growDuration)
            {
                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / growDuration);
                transform.localScale = Vector3.Lerp(startSize, endSize, t);
                
                Vector3 pos = transform.position;
                pos.y = fixedY;
                transform.position = pos;
                
                float alpha = Mathf.Sin(t * Mathf.PI) * 0.3f;
                Color color = emission.GetColor("_Color");
                color.a = alpha;
                emission.SetColor("_Color", color);
                
                yield return null;
            }

            movementController.IncreaseSpeed();
            OnIncreaseHoleSize?.Invoke();
            OnMoveEventHandler();
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