using System;
using EdwinGameDev.Gameplay;

namespace EdwinGameDev.Stages
{
    public interface IStage
    {
        event Action OnLevelComplete;
        void RegisterFood(Food food);
        void UnregisterFood(Food food);
    }
}