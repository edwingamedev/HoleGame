using UnityEngine;

public class MeshGenerator
{
    private readonly PolygonCollider2D hole2DCollider;
    private readonly PolygonCollider2D ground2DCollider;
    private readonly MeshCollider generatedMeshCollider;
    private readonly MeshFilter generatedMeshFilter;
    
    private Collider groundCollider;
    private Mesh generatedMesh;

    private const float InitialHoleScale = 2.5f;

    public MeshGenerator(PolygonCollider2D hole2DCollider, PolygonCollider2D ground2DCollider, MeshCollider generatedMeshCollider, MeshFilter generatedMeshFilter, Collider groundCollider)
    {
        this.hole2DCollider = hole2DCollider;
        this.ground2DCollider = ground2DCollider;
        this.generatedMeshCollider = generatedMeshCollider;
        this.generatedMeshFilter = generatedMeshFilter;
        this.groundCollider = groundCollider;
        
        SetupGround2DCollider();
        groundCollider.GetComponent<Renderer>().enabled = false;
    }

    ~MeshGenerator()
    {
        if (generatedMesh == null)
        {
            return;
        }

        Object.Destroy(generatedMesh);
    }

    private void SetupGround2DCollider()
    {
        Vector2 size = groundCollider.transform.localScale;

        Vector2[] points = new Vector2[4];
        points[0] = new Vector2(-size.x / 2, -size.y / 2);
        points[1] = new Vector2(-size.x / 2, size.y / 2);
        points[2] = new Vector2(size.x / 2, size.y / 2);
        points[3] = new Vector2(size.x / 2, -size.y / 2);

        ground2DCollider.SetPath(0, points);
    }
    
    public void UpdateHoleMesh(Transform holeTransform)
    {
        if (!holeTransform.hasChanged)
        {
            return;
        }

        hole2DCollider.transform.position = new Vector2(holeTransform.position.x, holeTransform.position.z);
        hole2DCollider.transform.localScale = holeTransform.localScale * InitialHoleScale;

        MakeHole2D();
        Make3DMeshCollider();
    }

    private void MakeHole2D()
    {
        Vector2[] points = hole2DCollider.GetPath(0);

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = hole2DCollider.transform.TransformPoint(points[i]);
        }

        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, points);
    }

    public void Make3DMeshCollider()
    {
        generatedMesh = ground2DCollider.CreateMesh(useBodyPosition: true, useBodyRotation: true);
        GenerateUVs();
        generatedMesh.RecalculateNormals();

        generatedMeshCollider.sharedMesh = generatedMesh;
        generatedMeshFilter.mesh = generatedMesh;
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