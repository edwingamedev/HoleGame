using System;
using EdwinGameDev.Gameplay;

namespace EdwinGameDev.Levels
{
    public interface ILevel
    {
        event Action OnLevelComplete;
        void RegisterFood(Food food);
        void UnregisterFood(Food food);
    }
}