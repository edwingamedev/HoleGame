using EdwinGameDev.Gameplay;
using UnityEngine;

namespace EdwinGameDev.UI
{
    public class FloatingPointsController : MonoBehaviour
    {
        [SerializeField] private GameObject pointsDisplay;
        [SerializeField] private Hole hole;

        private void OnEnable()
        {
            hole.OnConsume += HoleOnOnConsume;
        }

        private void OnDisable()
        {
            hole.OnConsume -= HoleOnOnConsume;
        }

        private void HoleOnOnConsume(int points)
        {
            GameObject go = Instantiate(pointsDisplay, transform.position, Quaternion.identity, transform);
            FlyingPoints flyingPoints = go.GetComponent<FlyingPoints>();
            flyingPoints.SetPoints(points);
            go.SetActive(true);
        }
    }
}