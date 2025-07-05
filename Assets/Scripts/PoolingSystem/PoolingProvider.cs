using System.Collections.Generic;
using UnityEngine;

namespace EdwinGameDev.PoolingService
{
    public class PoolingProvider<T> where T : Component, IPool<T>
    {
        private readonly Queue<IPool<T>> availableObjects = new();

        private readonly Transform poolHolder;
        private readonly IPool<T> poolObject;

        public PoolingProvider(Transform poolHolder, IPool<T> poolObject, int startSize)
        {
            this.poolHolder = poolHolder;
            this.poolObject = poolObject;

            PreWarm(startSize);
        }

        private void PreWarm(int size)
        {
            for (int i = 0; i < size; i++)
            {
                AddNewObject();
            }
        }

        private void AddNewObject()
        {
            GameObject gameObject = Object.Instantiate(poolObject.GetGameObject(), poolHolder);
            IPool<T> item = gameObject.GetComponent<IPool<T>>();
            item.OnDisableObject += Enqueue;

            item.DisableObject();
        }

        private void Enqueue(IPool<T> item)
        {
            if(availableObjects.Contains(item))
            {
                return;
            }

            availableObjects.Enqueue(item);
        }

        private IPool<T> ExtendAndGetFromPool()
        {
            AddNewObject();

            return availableObjects.Dequeue();
        }

        public IPool<T> Get()
        {
            IPool<T> item = availableObjects.Count > 0
                ? availableObjects.Dequeue()
                : ExtendAndGetFromPool();
            
            item.EnableObject();

            return item;
        }
    }
}