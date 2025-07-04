using UnityEngine;

namespace EdwinGameDev.Match
{
    public class MeshGeneratorSettings
    {
        public readonly PolygonCollider2D Hole2DCollider;
        public readonly PolygonCollider2D Ground2DCollider;
        public readonly MeshCollider GeneratedMeshCollider;
        public readonly MeshFilter GeneratedMeshFilter;
        public readonly Collider GroundCollider;

        public MeshGeneratorSettings(PolygonCollider2D hole2DCollider, PolygonCollider2D ground2DCollider, MeshCollider generatedMeshCollider, MeshFilter generatedMeshFilter, Collider groundCollider)
        {
            Hole2DCollider = hole2DCollider;
            Ground2DCollider = ground2DCollider;
            GeneratedMeshCollider = generatedMeshCollider;
            GeneratedMeshFilter = generatedMeshFilter;
            GroundCollider = groundCollider;
        }
    }
}