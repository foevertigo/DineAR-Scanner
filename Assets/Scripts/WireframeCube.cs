using UnityEngine;

public class WireframeCube : MonoBehaviour
{
    private Vector3[] corners;
    private LineRenderer[] edges;
    public float cubeSize = 0.05f; // You can adjust this from Inspector

    void Start()
    {
        DrawCube();
    }

    void DrawCube()
    {
        // Define cube corners dynamically based on cubeSize
        float s = cubeSize / 2f;
        corners = new Vector3[]
        {
            new Vector3(-s, -s, -s),
            new Vector3(s, -s, -s),
            new Vector3(s, -s, s),
            new Vector3(-s, -s, s),
            new Vector3(-s, s, -s),
            new Vector3(s, s, -s),
            new Vector3(s, s, s),
            new Vector3(-s, s, s)
        };

        int[,] edgesIndex = new int[,]
        {
            {0,1}, {1,2}, {2,3}, {3,0},
            {4,5}, {5,6}, {6,7}, {7,4},
            {0,4}, {1,5}, {2,6}, {3,7}
        };

        edges = new LineRenderer[12];
        for (int i = 0; i < 12; i++)
        {
            GameObject edge = transform.GetChild(i).gameObject;
            LineRenderer lr = edge.GetComponent<LineRenderer>();

            lr.positionCount = 2;
            lr.startWidth = 0.005f;
            lr.endWidth = 0.005f;
            lr.useWorldSpace = false;

            lr.material = new Material(Shader.Find("Unlit/Color"));
            lr.material.color = Color.cyan;

            lr.SetPosition(0, corners[edgesIndex[i, 0]]);
            lr.SetPosition(1, corners[edgesIndex[i, 1]]);
            edges[i] = lr;
        }
    }

    // 🟢 This lets you resize dynamically (for example, on pinch gesture)
    public void SetCubeSize(float newSize)
    {
        cubeSize = Mathf.Clamp(newSize, 0.05f, 1f); // limit size range
        DrawCube(); // redraw cube
    }
}
