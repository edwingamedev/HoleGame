using UnityEngine;

namespace EdwinGameDev.Utils
{
    public class Spinner : MonoBehaviour
    {
        [SerializeField] private float speed = 30f;
        [SerializeField] private Vector3 axis;
        
        private void LateUpdate()
        {
            transform.Rotate(axis, Time.deltaTime * speed, Space.World);
        }
    }
}