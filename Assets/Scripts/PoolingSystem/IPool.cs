using System;
using UnityEngine;

namespace EdwinGameDev.PoolingService
{
    public interface IPool<T>
    {
        bool isEnabled();
        void EnableObject();
        void DisableObject();
        event Action<T> OnDisableObject;
        GameObject GetGameObject();
        T GetObjectByType();
    }
}