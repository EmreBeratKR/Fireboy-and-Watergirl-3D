using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FluidMeshRenderer : MonoBehaviour
{
    [SerializeField] private WaveSettings settings;
    [SerializeField] private int dimensionX;
    [SerializeField] private int dimensionZ;
    private MeshFilter meshFilter;
    private Mesh mesh;


    private void Start()
    {
        mesh = new Mesh();
        mesh.name = "Fluid";

        mesh.vertices = CreateVertices();
        mesh.triangles = CreateTriangles(this.mesh.vertexCount);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter = this.GetComponent<MeshFilter>();
        meshFilter.mesh = this.mesh;
    }

    private void Update()
    {
        Simulate(this.mesh);
    }

    [SerializeField] private PreviewMode previewMode;
    private void OnDrawGizmos()
    {
        if (!previewMode.preview) return;

        Gizmos.color = previewMode.color;

        var preview = new Mesh();

        preview.vertices = CreateVertices();
        preview.triangles = CreateTriangles(preview.vertexCount);
        preview.RecalculateNormals();

        if (previewMode.simulate)
        {
            Simulate(preview);
        }

        Gizmos.DrawMesh(preview, transform.position, Quaternion.identity, transform.lossyScale);
    }
    
    private void Simulate(Mesh targetMesh)
    {
        var old = targetMesh.vertices;

        for (int x = 0; x <= dimensionX; x++)
        {
            for (int z = 1; z <= dimensionZ; z++)
            {
                var vertex = old[VertexIndex(x, z)];
                var elongation = Mathf.Sin(x * settings.waveLength + Time.time * settings.speed) * settings.amplitude;
                vertex = new Vector3(vertex.x, elongation, vertex.z);
                old[VertexIndex(x, z)] = vertex;
            }
        }

        targetMesh.vertices = old;
        targetMesh.RecalculateNormals();
    }


    private Vector3[] CreateVertices()
    {
        var offset = Vector3.left * dimensionX * 0.5f;

        var result = new Vector3[(dimensionX + 1) * (dimensionZ + 1)];

        for (int x = 0; x <= dimensionX; x++)
        {
            for (int z = 0; z <= dimensionZ; z++)
            {
                if (z == 0)
                {
                    result[VertexIndex(x, z)] = new Vector3(x, -settings.frontHeight, 0) + offset;
                    continue;
                }
                result[VertexIndex(x, z)] = new Vector3(x, 0, z-1) + offset;
            }
        }

        return result;
    }

    private int[] CreateTriangles(int vertexCount)
    {
        var result = new int[vertexCount * 6];

        for (int x = 0; x < dimensionX; x++)
        {
            for (int z = 0; z < dimensionZ; z++)
            {
                result[VertexIndex(x, z) * 6 + 0] = VertexIndex(x, z);
                result[VertexIndex(x, z) * 6 + 1] = VertexIndex(x + 1, z + 1);
                result[VertexIndex(x, z) * 6 + 2] = VertexIndex(x + 1, z);
                result[VertexIndex(x, z) * 6 + 3] = VertexIndex(x, z);
                result[VertexIndex(x, z) * 6 + 4] = VertexIndex(x, z + 1);
                result[VertexIndex(x, z) * 6 + 5] = VertexIndex(x + 1, z + 1);
            }
        }

        return result;
    }

    private int VertexIndex(int x, int z)
    {
        return x * (dimensionZ + 1) + z;
    }
    

    [Serializable]
    public struct WaveSettings
    {
        public float amplitude;
        public float waveLength;
        public float speed;
        public float frontHeight;
    }

    [Serializable]
    public struct PreviewMode
    {
        public bool preview;
        public bool simulate;
        public Color color;
    }
}
