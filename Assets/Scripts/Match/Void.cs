using UnityEngine;

public class Void : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Food food = other.GetComponent<Food>();

        if (food == null)
        {
            return;
        }

        //food.Consume();
        
        // Find all holes (or just one if needed)
        Hole[] holes = FindObjectsOfType<Hole>();
        foreach (Hole hole in holes)
        {
            if (!hole.HasCaptured(food))
            {
                continue;
            }

            food.ConsumerHole = hole;
            food.Consume();
            break;
        }
    }
}
