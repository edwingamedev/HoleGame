using System;
using UnityEngine;

public class HoleMovementController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    
    public event Action OnMove;
    private Vector3 lastPos;
    
    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        MoveWithKeyboard();
#else
        MoveWithTouch();
#endif
    }

    public void IncreaseSpeed()
    {
        speed += 1f;
    }
    
    private void MoveWithKeyboard()
    {
        Vector3 moveDir = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (moveDir == Vector3.zero)
        {
            return;
        }

        transform.position += moveDir * (speed * Time.deltaTime);
        OnMove?.Invoke();

        transform.hasChanged = false;
    }
    
    private Vector2 touchStartPos;
    private bool isTouching;
    
    private void MoveWithTouch()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                touchStartPos = touch.position;
                isTouching = true;
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (!isTouching)
                    return;

                Vector2 touchDelta = touch.position - touchStartPos;

                // Convert screen delta to world movement direction
                Vector3 moveDir = new Vector3(touchDelta.x, 0, touchDelta.y).normalized;

                transform.position += moveDir * (speed * Time.deltaTime);
                OnMove?.Invoke();
                transform.hasChanged = false;

                // Optional: update start pos to make it more like dragging
                touchStartPos = touch.position;
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                isTouching = false;
                break;
        }
    }
}