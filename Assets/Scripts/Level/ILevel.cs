using System;
using System.Collections.Generic;
using EdwinGameDev.Gameplay;

namespace EdwinGameDev.Levels
{
    public interface ILevel
    {
        List<Food> GetFoods();
        int LevelDuration();
        event Action OnLevelComplete;
        void RegisterFood(Food food);
        void UnregisterFood(Food food);
    }
}