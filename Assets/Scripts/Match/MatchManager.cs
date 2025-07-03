using UnityEngine;
using EdwinGameDev.UI;
using EdwinGameDev.Gameplay;

namespace EdwinGameDev.Match
{
    public class MatchManager : MonoBehaviour
    {
        // Match
        [Header("Match")] [SerializeField] private Collider groundCollider;
        [SerializeField] private GameObject holePrefab;
        [SerializeField] private CameraFollow cameraFollow;

        [Header("Mesh")] [SerializeField] private PolygonCollider2D hole2DCollider;
        [SerializeField] private PolygonCollider2D ground2DCollider;
        [SerializeField] private MeshCollider generatedMeshCollider;
        [SerializeField] private MeshFilter generatedMeshFilter;

        private MeshGenerator meshGenerator;

        [Header("UI")] [SerializeField] private UIManager uiManager;

        private void Start()
        {
            meshGenerator = new MeshGenerator(
                hole2DCollider,
                ground2DCollider,
                generatedMeshCollider,
                generatedMeshFilter,
                groundCollider);

            CreateNewHole();
            DisableFoodCollision();

            meshGenerator.Make3DMeshCollider();
        }

        private void CreateNewHole()
        {
            GameObject holeGameObject = Instantiate(holePrefab);
            Hole hole = holeGameObject.GetComponentInChildren<Hole>();
            hole.Setup(groundCollider, generatedMeshCollider);
            hole.OnHoleMove += meshGenerator.UpdateHoleMesh;

            cameraFollow.SetTarget(holeGameObject.transform);
            hole.OnIncreaseHoleSize += cameraFollow.OnTargetSizeChanges;
            uiManager.SetupHole(hole);
        }

        private void DisableFoodCollision()
        {
            Food[] foodObjects = FindObjectsOfType<Food>();

            foreach (Food food in foodObjects)
            {
                Collider foodCollider = food.GetComponent<Collider>();
                if (foodCollider == null)
                {
                    continue;
                }

                Physics.IgnoreCollision(foodCollider, generatedMeshCollider, true);
            }
        }
    }
}