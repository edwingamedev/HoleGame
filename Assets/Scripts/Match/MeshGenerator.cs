using EdwinGameDev.EventSystem;
using EdwinGameDev.Gameplay;
using UnityEngine;

namespace EdwinGameDev.Match
{
    public class MeshGenerator
    {
        private MeshGeneratorSettings meshGeneratorSettings;
        private Mesh generatedMesh;

        private const float InitialHoleScale = 2.5f;
        private Hole targetHole;

        public MeshGenerator(MeshGeneratorSettings meshGeneratorSettings)
        {
            this.meshGeneratorSettings = meshGeneratorSettings;
            GlobalEventDispatcher.RemoveSubscriber<Events.HoleMoved>(UpdateHoleMesh);
            GlobalEventDispatcher.AddSubscriber<Events.HoleMoved>(UpdateHoleMesh);
        }

        public void Dispose()
        {
            GlobalEventDispatcher.RemoveSubscriber<Events.HoleMoved>(UpdateHoleMesh);
            if (generatedMesh == null)
            {
                return;
            }

            Object.Destroy(generatedMesh);
        }

        public void GenerateMesh()
        {
            SetupGround2DCollider();
            
            Make3DMeshCollider();
        }
        
        private void SetupGround2DCollider()
        {
            Vector2 size = meshGeneratorSettings.GroundCollider.transform.localScale;

            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(-size.x / 2, -size.y / 2);
            points[1] = new Vector2(-size.x / 2, size.y / 2);
            points[2] = new Vector2(size.x / 2, size.y / 2);
            points[3] = new Vector2(size.x / 2, -size.y / 2);

            meshGeneratorSettings.Ground2DCollider.SetPath(0, points);
        }

        public void SetTarget(Hole targetHole)
        {
            if (this.targetHole)
            {
                this.targetHole.OnIncreaseHoleSize -= UpdateHoleMesh;
            }

            this.targetHole = targetHole;
            this.targetHole.OnIncreaseHoleSize += UpdateHoleMesh;
        }

        private void UpdateHoleMesh(Events.HoleMoved _)
        {
            UpdateHoleMesh();
        }

        private void UpdateHoleMesh()
        {
            if (!targetHole.transform.hasChanged)
            {
                return;
            }

            meshGeneratorSettings.Hole2DCollider.transform.position =
                new Vector2(
                    targetHole.transform.position.x,
                    targetHole.transform.position.z);
        
            meshGeneratorSettings.Hole2DCollider.transform.localScale = targetHole.transform.localScale * InitialHoleScale;

            MakeHole2D();
            Make3DMeshCollider();
        }

        private void MakeHole2D()
        {
            Vector2[] points = meshGeneratorSettings.Hole2DCollider.GetPath(0);

            for (int i = 0; i < points.Length; i++)
            {
                // Transform from Hole2DCollider local space to world space
                Vector3 worldPoint = meshGeneratorSettings.Hole2DCollider.transform.TransformPoint(points[i]);

                // Transform from world space to Ground2DCollider local space
                Vector3 localPoint = meshGeneratorSettings.Ground2DCollider.transform.InverseTransformPoint(worldPoint);

                points[i] = new Vector2(localPoint.x, localPoint.y);
            }

            meshGeneratorSettings.Ground2DCollider.pathCount = 2;
            meshGeneratorSettings.Ground2DCollider.SetPath(1, points);
        }

        private void Make3DMeshCollider()
        {
            generatedMesh = meshGeneratorSettings.Ground2DCollider.CreateMesh(useBodyPosition: true, useBodyRotation: false);
            generatedMesh.RecalculateNormals();

            meshGeneratorSettings.GeneratedMeshCollider.sharedMesh = generatedMesh;
            meshGeneratorSettings.GeneratedMeshFilter.mesh = generatedMesh;
        }
    }
}