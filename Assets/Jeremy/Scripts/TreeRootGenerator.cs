using UnityEngine;

public class TwistedTreeRoot : MonoBehaviour
{
    public float height = 1.0f;
    public float radius = 1.0f;
    public int heightSegments = 10;
    public int radialSegments = 20;
    public float twistAmount = 0.1f;

    private void Start()
    {
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[(radialSegments + 1) * (heightSegments + 1) + 1];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[radialSegments * heightSegments * 6 + radialSegments * 3];

        float angleStep = 360.0f / radialSegments;
        float heightStep = height / heightSegments;
        float radiusStep = (radius - 0.01f) / heightSegments;

        int vIdx = 0;
        for (int h = 0; h <= heightSegments; h++)
        {
            float currentRadius = radius - h * radiusStep;

            for (int r = 0; r <= radialSegments; r++)
            {
                float angle = r * angleStep;
                float x = Mathf.Cos(angle * Mathf.Deg2Rad);
                float y = Mathf.Sin(angle * Mathf.Deg2Rad);
                float z = h * heightStep;

                // Add a random offset in the radial direction to create twisting effect
                float randomRadialOffset = Random.Range(-twistAmount, twistAmount);
                x = x * (currentRadius + randomRadialOffset * h);
                y = y * (currentRadius + randomRadialOffset * h);

                vertices[vIdx] = new Vector3(x, y, z);
                normals[vIdx] = Vector3.back;
                uv[vIdx++] = new Vector2(r / (float)radialSegments, h / (float)heightSegments);
            }
        }

        // Add center vertex for the end cap
        vertices[vIdx] = Vector3.zero;
        normals[vIdx] = Vector3.back;
        uv[vIdx++] = new Vector2(0.5f, 0.5f);

        int tIdx = 0;
        for (int h = 0; h < heightSegments; h++)
        {
            for (int r = 0; r < radialSegments; r++)
            {
                int curIdx = h * (radialSegments + 1) + r;
                int nextIdx = curIdx + radialSegments + 1;
                triangles[tIdx++] = curIdx;
                triangles[tIdx++] = curIdx + 1;
                triangles[tIdx++] = nextIdx;

                triangles[tIdx++] = curIdx + 1;
                triangles[tIdx++] = nextIdx + 1;
                triangles[tIdx++] = nextIdx;
            }
        }

        // Add end cap triangles
        for (int r = 0; r < radialSegments; r++)
        {
            int curIdx = heightSegments * (radialSegments + 1) + r;
            int nextIdx = curIdx + 1;

            if (nextIdx >= vertices.Length)
                nextIdx = heightSegments * (radialSegments + 1);

            if (curIdx < vertices.Length && nextIdx < vertices.Length)
            {
                triangles[tIdx++] = vIdx;
                triangles[tIdx++] = curIdx;
                triangles[tIdx++] = nextIdx;
            }
        }


        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
    }
}










/*using UnityEngine;

public class TreeRootGenerator : MonoBehaviour
{
    public int heightSegments = 10;
    public int radialSegments = 8;
    public float height = 10.0f;
    public float bottomRadius = 0.5f;
    public float topRadius = 0.2f;
    public float twistAmount = 0.1f;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;

    private void Start()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        mesh = new Mesh();

        Vector3[] vertices = new Vector3[(radialSegments + 1) * (heightSegments + 1)];
        Vector3[] normals = new Vector3[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];

        float angleStep = 360.0f / radialSegments;
        float heightStep = height / heightSegments;

        int vIdx = 0;
        for (int h = 0; h <= heightSegments; h++)
        {
            for (int r = 0; r <= radialSegments; r++)
            {
                float hRatio = (float)h / heightSegments;
                float radius = Mathf.Lerp(bottomRadius, topRadius, hRatio);

                float angle = r * angleStep;
                float x = Mathf.Cos(angle * Mathf.Deg2Rad);
                float y = Mathf.Sin(angle * Mathf.Deg2Rad);
                float z = h * heightStep;

                // Add a random offset in the radial direction to create twisting effect
                float randomRadialOffset = Random.Range(-twistAmount, twistAmount);
                x = x * (radius + randomRadialOffset * h);
                y = y * (radius + randomRadialOffset * h);

                vertices[vIdx] = new Vector3(x, y, z);
                normals[vIdx] = Vector3.back;
                uv[vIdx++] = new Vector2(r / (float)radialSegments, h / (float)heightSegments);
            }
        }


        
        //float hStep = height / heightSegments;
        //float rStep = Mathf.PI * 2.0f / radialSegments;
        
        //int vIdx = 0;
        //for (int h = 0; h <= heightSegments; h++)
        //{
        //    float hRatio = (float)h / heightSegments;
        //    float radius = Mathf.Lerp(bottomRadius, topRadius, hRatio);

        //    for (int r = 0; r <= radialSegments; r++)
        //    {
        //        float angle = r * rStep;
        //        float x = Mathf.Sin(angle) * radius;
        //        float z = Mathf.Cos(angle) * radius;

        //        vertices[vIdx] = new Vector3(x, h * hStep, z);
        //        normals[vIdx] = Vector3.up;
        //        uv[vIdx] = new Vector2(hRatio, (float)r / radialSegments);

        //        vIdx++;
        //    }
        //}

        int[] triangles = new int[radialSegments * heightSegments * 6];
        int tIdx = 0;
        for (int h = 0; h < heightSegments; h++)
        {
            for (int r = 0; r < radialSegments; r++)
            {
                int i0 = h * (radialSegments + 1) + r;
                int i1 = i0 + 1;
                int i2 = i0 + radialSegments + 1;
                int i3 = i2 + 1;

                triangles[tIdx++] = i0;
                triangles[tIdx++] = i1;
                triangles[tIdx++] = i2;

                triangles[tIdx++] = i1;
                triangles[tIdx++] = i3;
                triangles[tIdx++] = i2;
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
    }
}
*/