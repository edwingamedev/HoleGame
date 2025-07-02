using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] private HoleMovementController movementController;

    private Collider groundCollider;
    private MeshCollider generatedMeshCollider;
    [SerializeField] private int points;

    private readonly HashSet<Food> capturedFoods = new();

    private readonly int sizeThreshold = 5;

    public event Action OnIncreaseHoleSize;
    public event Action<Transform> OnHoleMove;
    public event Action<int> OnConsume;
    public bool HasCaptured(Food food) => capturedFoods.Contains(food);
    
    public void Setup(Collider groundCollider, MeshCollider generatedMeshCollider)
    {
        this.groundCollider = groundCollider;
        this.generatedMeshCollider = generatedMeshCollider;
        
        if (movementController != null)
        {
            movementController.OnMove += OnMoveEventHandler;
        }
    }

    private void OnMoveEventHandler()
    {
        OnHoleMove?.Invoke(transform);
    }
    
    public void AddPoints(Food food)
    {
        points += food.points;

        OnConsume?.Invoke(food.points);

        if (points % sizeThreshold != 0)
        {
            return;
        }

        StartCoroutine(nameof(IncreaseSize));
    }

    private IEnumerator IncreaseSize()
    {
        Vector3 startSize = transform.localScale;
        float growDuration = 0.4f;
        float growMultiplier = 1.2f;
        Vector3 endSize = transform.localScale * growMultiplier;
        float fixedY = transform.position.y;
        
        float time = 0;
        while (time < growDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / growDuration);
            transform.localScale = Vector3.Lerp(startSize, endSize, t);
            
            Vector3 pos = transform.position;
            pos.y = fixedY;
            transform.position = pos;
            yield return null;
        }

        OnIncreaseHoleSize?.Invoke();
        OnMoveEventHandler();
    }

    public void ConfirmCapture(Food food)
    {
        if (capturedFoods.Add(food))
        {
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Food food = other.GetComponent<Food>();

        if (food != null)
        {
            food.ConsumerHole = this;
        }

        Physics.IgnoreCollision(other, groundCollider, true);
        Physics.IgnoreCollision(other, generatedMeshCollider, false);
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, groundCollider, false);
        Physics.IgnoreCollision(other, generatedMeshCollider, true);
    }
}