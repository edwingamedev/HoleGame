using System;
using System.Collections.Generic;
using EdwinGameDev.Gameplay;
using UnityEngine;

namespace EdwinGameDev.Levels
{
    public class LevelController : MonoBehaviour, ILevel
    {
        public Collider GroundCollider;
        private readonly List<Food> foods = new();
        public event Action OnLevelComplete;

        public void RegisterFood(Food food)
        {
            if (foods.Contains(food))
            {
                return;
            }

            foods.Add(food);
        }

        public void UnregisterFood(Food food)
        {
            foods.Remove(food);
        
            if (foods.Count <= 0)
            {
                OnLevelComplete?.Invoke();
            }
        }
    }
}