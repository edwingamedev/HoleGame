using UnityEngine;

namespace EdwinGameDev.Gameplay
{
    public class ConsumeZone : MonoBehaviour
    {
        private Hole hole;

        private void Awake()
        {
            hole = GetComponentInParent<Hole>();
        }

        private void OnTriggerExit(Collider other)
        {
            Food food = other.GetComponent<Food>();
            if (!food)
            {
                return;
            }

            food.ConsumerHole = hole;
            food.Consume();
        }
    }
}
