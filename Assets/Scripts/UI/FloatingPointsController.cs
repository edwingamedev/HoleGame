using EdwinGameDev.Gameplay;
using UnityEngine;

namespace EdwinGameDev.UI
{
    public class FloatingPointsController : MonoBehaviour
    {
        [SerializeField] private GameObject pointsDisplay;
        private Hole hole;

        public void SetHole(Hole hole)
        {
            this.hole = hole;
            hole.OnConsume += HoleOnOnConsume;
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