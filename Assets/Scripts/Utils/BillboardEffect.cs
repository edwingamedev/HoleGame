using UnityEngine;

namespace EdwinGameDev.Utils
{
    public class BillboardEffect : MonoBehaviour
    {
        private Camera targetCamera;

        private void Start()
        {
            targetCamera = Camera.main;
        }

        private void LateUpdate()
        {
            Vector3 lookPos = transform.position + targetCamera.transform.rotation * Vector3.forward;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos, Vector3.up);
        }
    }
}