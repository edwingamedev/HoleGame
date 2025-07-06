using System.Collections.Generic;
using EdwinGameDev.EventSystem;
using UnityEngine;

namespace EdwinGameDev.Utils
{
    public class TransparencyManager : MonoBehaviour
    {
        [SerializeField] private LayerMask fadeableLayer;
        [SerializeField] private float fadeSpeed = 5f;
        [SerializeField] private float transparentAlpha = 0.5f;
    
        private Transform target;
        private Dictionary<FadeableObject, bool> fadingObjects = new();

        private void OnEnable()
        {
            GlobalEventDispatcher.AddSubscriber<Events.HoleCreated>(SetTarget);
        }

        private void OnDisable()
        {
            GlobalEventDispatcher.RemoveSubscriber<Events.HoleCreated>(SetTarget);
        }
    
        private void SetTarget(Events.HoleCreated holeEvent)
        {
            target = holeEvent.Hole.transform;
        }
    
        private void LateUpdate()
        {
            Vector3 direction = target.position - transform.position;
            float distance = direction.magnitude;

            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance, fadeableLayer);

            HashSet<FadeableObject> currentlyHit = new();
        
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.TryGetComponent(out FadeableObject fadeable))
                {
                    fadeable.FadeTo(transparentAlpha, fadeSpeed);
                    currentlyHit.Add(fadeable);
                    fadingObjects[fadeable] = true;
                }
            }

            List<FadeableObject> toRemove = new();
            foreach (KeyValuePair<FadeableObject, bool> kvp in fadingObjects)
            {
                if (!currentlyHit.Contains(kvp.Key))
                {
                    kvp.Key.FadeTo(1f, fadeSpeed);
                
                    if (Mathf.Abs(kvp.Key.GetCurrentAlpha() - 1f) < 0.01f)
                    {
                        toRemove.Add(kvp.Key);
                    }
                }
            }

            foreach (FadeableObject obj in toRemove)
            {
                fadingObjects.Remove(obj);
            }
        }
    }
}