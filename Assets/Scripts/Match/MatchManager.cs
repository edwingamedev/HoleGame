using EdwinGameDev.EventSystem;
using EdwinGameDev.Gameplay;
using EdwinGameDev.Levels;
using UnityEngine;

namespace EdwinGameDev.Match
{
    public class MatchManager : MonoBehaviour
    {
        [Header("Match")] 
         private Collider groundCollider;
        [SerializeField] private GameObject holePrefab;

        [Header("Mesh")] 
        [SerializeField] private PolygonCollider2D hole2DCollider;
        [SerializeField] private PolygonCollider2D ground2DCollider;
        [SerializeField] private MeshCollider generatedMeshCollider;
        [SerializeField] private MeshFilter generatedMeshFilter;

        private MeshGenerator meshGenerator;

        [SerializeField] private GameSettings gameSettings;
        
        private float MatchDuration => gameSettings.MatchDuration;

        private float matchTimer;
        private bool matchIsOn;
        
        private ILevel currentLevel;
        
        private void Start()
        {
            matchTimer = MatchDuration;
            
            DisableFoodCollision();

            StartGame();
        }

        private void GenerateLevel()
        {
            LevelController gameSettingsLevel = gameSettings.Stages[gameSettings.selectedLevel];
            
            GameObject level = Instantiate(gameSettingsLevel.gameObject);
            level.GetComponent<ILevel>().OnLevelComplete += GameOver;
            
            groundCollider = level.GetComponent<LevelController>().GroundCollider;
            
            meshGenerator = new MeshGenerator(new MeshGeneratorSettings(
                hole2DCollider,
                ground2DCollider,
                generatedMeshCollider,
                generatedMeshFilter,
                groundCollider));
            
            CreateNewHole();
            
            meshGenerator.GenerateMesh();
        }
        
        private void StartGame()
        {
            GenerateLevel();
            
            StartTimer();
            matchIsOn = true;

            GlobalEventDispatcher.Publish(new Events.GameStarted());
        }

        private void GameOver()
        {
            matchIsOn = false;

            GlobalEventDispatcher.Publish(new Events.GameEnded());
        }

        private void StartTimer()
        {
            matchTimer = MatchDuration;
        }

        private void Update()
        {
            if (!matchIsOn)
            {
                return;
            }

            matchTimer -= Time.deltaTime;

            if (matchTimer <= 0)
            {
                matchTimer = 0f;
                GameOver();
            }
            
            GlobalEventDispatcher.Publish(new Events.OnMatchTimerUpdated(matchTimer));
        }

        private void CreateNewHole()
        {
            GameObject holeGameObject = Instantiate(holePrefab);
            Hole hole = holeGameObject.GetComponentInChildren<Hole>();
            hole.Setup(groundCollider, generatedMeshCollider);

            meshGenerator.SetTarget(hole);

            GlobalEventDispatcher.Publish(new Events.HoleCreated(hole));
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