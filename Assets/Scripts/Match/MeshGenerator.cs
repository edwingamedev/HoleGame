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
            GlobalEventDispatcher.AddSubscriber<Events.HoleMoved>(UpdateHoleMesh);
        }

        ~MeshGenerator()
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
            meshGeneratorSettings.GroundCollider.GetComponent<Renderer>().enabled = false;
            
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
                points[i] = meshGeneratorSettings.Hole2DCollider.transform.TransformPoint(points[i]);
            }

            meshGeneratorSettings.Ground2DCollider.pathCount = 2;
            meshGeneratorSettings.Ground2DCollider.SetPath(1, points);
        }

        public void Make3DMeshCollider()
        {
            generatedMesh = meshGeneratorSettings.Ground2DCollider.CreateMesh(useBodyPosition: true, useBodyRotation: true);
            GenerateUVs();
            generatedMesh.RecalculateNormals();

            meshGeneratorSettings.GeneratedMeshCollider.sharedMesh = generatedMesh;
            meshGeneratorSettings.GeneratedMeshFilter.mesh = generatedMesh;
        }

        private void GenerateUVs()
        {
            Vector3[] vertices = generatedMesh.vertices;
            Vector2[] uvs = new Vector2[vertices.Length];

            float minX = float.MaxValue, maxX = float.MinValue;
            float minY = float.MaxValue, maxY = float.MinValue;

            // Find bounds on X and Y
            foreach (Vector3 vertex in vertices)
            {
                if (vertex.x < minX)
                {
                    minX = vertex.x;
                }

                if (vertex.x > maxX)
                {
                    maxX = vertex.x;
                }

                if (vertex.y < minY)
                {
                    minY = vertex.y;
                }

                if (vertex.y > maxY)
                {
                    maxY = vertex.y;
                }
            }

            float width = maxX - minX;
            float height = maxY - minY;

            if (Mathf.Approximately(width, 0f))
            {
                width = 0.0001f;
            }

            if (Mathf.Approximately(height, 0f))
            {
                height = 0.0001f;
            }

            // Normalize UVs
            for (int i = 0; i < vertices.Length; i++)
            {
                float u = (vertices[i].x - minX) / width;
                float v = (vertices[i].y - minY) / height;
                uvs[i] = new Vector2(u, v);
            }

            generatedMesh.uv = uvs;
        }
    }
}