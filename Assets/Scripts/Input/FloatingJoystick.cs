using UnityEngine;
using UnityEngine.EventSystems;

namespace EdwinGameDev.Input
{
    public class FloatingJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform joystickBackground;
        [SerializeField] private RectTransform joystickHandle;
        [SerializeField] private GameObject joystick;
        private Vector2 inputVector;
        private Canvas canvas;
    
        public float Horizontal => inputVector.x;
        public float Vertical => inputVector.y;
    
        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
            joystick.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            joystick.SetActive(true);
        
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect: (RectTransform)canvas.transform,
                screenPoint: eventData.position,
                cam: canvas.worldCamera,
                localPoint: out Vector2 localPoint);

            joystickBackground.anchoredPosition = localPoint;
            joystickHandle.anchoredPosition = Vector2.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rect: joystickBackground,
                    screenPoint: eventData.position,
                    cam: canvas.worldCamera,
                    localPoint: out Vector2 pos))
            {
                return;
            }

            float radius = joystickBackground.sizeDelta.x / 2f;

            inputVector = pos / radius;
            inputVector = inputVector.magnitude > 1f ? inputVector.normalized : inputVector;

            joystickHandle.anchoredPosition = inputVector * radius;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            inputVector = Vector2.zero;
            joystickHandle.anchoredPosition = Vector2.zero;
            
            joystick.SetActive(false);
        }
    }
}
