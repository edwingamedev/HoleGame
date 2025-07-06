using System;
using System.Collections.Generic;
using EdwinGameDev.Gameplay;
using UnityEngine;

namespace EdwinGameDev.Levels
{
    [Serializable]
    public class LevelController : MonoBehaviour, ILevel
    {
        public Collider GroundCollider;
        [SerializeField] private int levelDuration;
        [SerializeField] private LevelStatus levelStatus = new();

        private List<Food> foods = new();
        
        public int LevelDuration() => levelDuration;
        public List<Food> GetFoods() => foods;
        
        public event Action OnLevelComplete;

        private void OnValidate()
        {
            levelStatus.CheckMaxLevel(transform);
        }

        public void RegisterFood(Food food)
        {
            GetComponentsInChildren(true, foods);

            if (foods.Contains(food))
            {
                return;
            }

            foods.Add(food);
        }

        public void UnregisterFood(Food food)
        {
            foods.Remove(food);

            if (foods.Count > 0)
            {
                return;
            }

            OnLevelComplete?.Invoke();
        }
    }
}