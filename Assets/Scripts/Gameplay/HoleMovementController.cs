using System;
using UnityEngine;

public class HoleMovementController : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    
    public event Action OnMove;
    private Vector3 lastPos;
    
    private void Update()
    {
        Move();
    }

    public void IncreaseSpeed()
    {
        speed += 1f;
    }
    
    private void Move()
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
}