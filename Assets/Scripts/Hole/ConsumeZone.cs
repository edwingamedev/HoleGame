using System.Collections.Generic;
using UnityEngine;

public class ConsumeZone : MonoBehaviour
{
    private Hole hole;
    private HashSet<Food> foodsInside = new HashSet<Food>();

    private void Awake()
    {
        hole = GetComponentInParent<Hole>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Food food = other.GetComponent<Food>();
        if (food != null)
        {
            foodsInside.Add(food);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Food food = other.GetComponent<Food>();
        if (food == null || !foodsInside.Contains(food))
        {
            return;
        }

        foodsInside.Remove(food);
        hole.ConfirmCapture(food);
    }
}
