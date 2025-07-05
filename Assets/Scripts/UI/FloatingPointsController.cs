using System;
using EdwinGameDev.Gameplay;
using EdwinGameDev.PoolingService;
using UnityEngine;

namespace EdwinGameDev.UI
{
    public class FloatingPointsController : MonoBehaviour
    {
        [SerializeField] private FlyingPoints pointsDisplay;
        [SerializeField] private Hole hole;
        private PoolingProvider<FlyingPoints> poolingProvider;

        private void Start()
        {
            poolingProvider = new(transform, pointsDisplay, 10);
        }

        private void OnEnable()
        {
            hole.OnConsume += HoleOnOnConsume;
        }

        private void OnDisable()
        {
            hole.OnConsume -= HoleOnOnConsume;
        }

        private void HoleOnOnConsume(int points)
        {
            IPool<FlyingPoints> item = poolingProvider.Get();
            item.GetObjectByType().SetPoints(points);
        }
    }
}