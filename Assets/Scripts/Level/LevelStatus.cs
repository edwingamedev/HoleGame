using EdwinGameDev.Gameplay;
using EdwinGameDev.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace EdwinGameDev.Levels
{
    [System.Serializable]
    public class LevelStatus
    {
        [SerializeField] private PlayerSettings playerSettings;
        
        [Header("Level Potential")]
        [ReadOnly] [SerializeField] private int experience;
        [ReadOnly] [SerializeField] private int maxLevel;
        
        [Header("Missing Exp")]
        [ReadOnly] [SerializeField] private int remainingExp; // used to see remaining Exp on inspector

        public void CheckMaxLevel(Transform targetTransform)
        {
            if (!playerSettings)
            {
                return;
            }

            Food[] food = targetTransform.GetComponentsInChildren<Food>();
            
            experience = 0;
            maxLevel = 0;

            for (int i = 0; i < food.Length; i++)
            {
                experience += food[i].points;
            }

            maxLevel = playerSettings.GetLevelFromTotalPoints(experience);
            remainingExp = maxLevel >= playerSettings.MaxLevel
                ? 0
                : playerSettings.TotalExpToReachLevel(playerSettings.MaxLevel) - experience;
        }
    }
}