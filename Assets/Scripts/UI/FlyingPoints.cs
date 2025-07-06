using System;
using EdwinGameDev.PoolingService;
using TMPro;
using UnityEngine;

namespace EdwinGameDev.UI
{
    public class FlyingPoints : MonoBehaviour, IPool<FlyingPoints>
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Animator animator;
        public event Action<FlyingPoints> OnDisableObject;
        
        public void SetPoints(int points)
        {
            text.SetText($"+{points}");
        }

        public void EnableObject()
        {
            gameObject.SetActive(true);
            
            animator.Play(0);
        }
        
        public void DisableObject()
        {
            gameObject.SetActive(false);
            OnDisableObject?.Invoke(this);
        }
        
        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public FlyingPoints GetObjectByType()
        {
            return this;
        }
        
        // Controlled by an event on the animation
        public void OnAnimationFinished()
        {
            DisableObject();
        }
    }
}