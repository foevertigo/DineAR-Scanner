using UnityEngine;
using UnityEngine.EventSystems;

public class WireframeCubeController : MonoBehaviour
{
    public Material lineMaterial;
    private LineRenderer lr;
    private Vector3[] corners;

    void Start()
    {
        // Create wireframe cube
        lr = gameObject.AddComponent<LineRenderer>();
        lr.positionCount = 16;
        lr.loop = false;
        lr.material = lineMaterial;
        lr.startWidth = 0.005f;
        lr.endWidth = 0.005f;
        lr.useWorldSpace = false;
        lr.startColor = Color.white;
        lr.endColor = Color.white;

        transform.position = new Vector3(0, 0, 0.5f);
        transform.rotation = Quaternion.identity;

        UpdateCube(Vector3.one * 0.1f); // medium-sized cube initially

        // add stretchable faces
        CreateFace("Front", new Vector3(0, 0, 0.05f), Vector3.forward);
        CreateFace("Back", new Vector3(0, 0, -0.05f), Vector3.back);
        CreateFace("Left", new Vector3(-0.05f, 0, 0), Vector3.left);
        CreateFace("Right", new Vector3(0.05f, 0, 0), Vector3.right);
        CreateFace("Top", new Vector3(0, 0.05f, 0), Vector3.up);
        CreateFace("Bottom", new Vector3(0, -0.05f, 0), Vector3.down);

        gameObject.AddComponent<MoveCube>(); // enable movement
    }

    void UpdateCube(Vector3 size)
    {
        corners = new Vector3[]
        {
        new Vector3(-size.x, -size.y, -size.z),
        new Vector3(size.x, -size.y, -size.z),
        new Vector3(size.x, size.y, -size.z),
        new Vector3(-size.x, size.y, -size.z),
        new Vector3(-size.x, -size.y, size.z),
        new Vector3(size.x, -size.y, size.z),
        new Vector3(size.x, size.y, size.z),
        new Vector3(-size.x, size.y, size.z)
        };

        int[,] edgesIndex = new int[,]
        {
        {0,1}, {1,2}, {2,3}, {3,0},
        {4,5}, {5,6}, {6,7}, {7,4},
        {0,4}, {1,5}, {2,6}, {3,7}
        };

        // Clear previous edges if any
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("Edge_"))
                Destroy(child.gameObject);
        }

        // Create 12 new edge objects
        for (int i = 0; i < 12; i++)
        {
            GameObject edge = new GameObject("Edge_" + i);
            edge.transform.SetParent(transform, false);

            LineRenderer lrEdge = edge.AddComponent<LineRenderer>();
            lrEdge.positionCount = 2;
            lrEdge.useWorldSpace = false;
            lrEdge.startWidth = 0.002f;
            lrEdge.endWidth = 0.002f;
            lrEdge.material = lineMaterial;
            lrEdge.startColor = Color.white;
            lrEdge.endColor = Color.white;
            lrEdge.SetPosition(0, corners[edgesIndex[i, 0]]);
            lrEdge.SetPosition(1, corners[edgesIndex[i, 1]]);
        }
    }



    void CreateFace(string name, Vector3 center, Vector3 direction)
    {
        GameObject face = GameObject.CreatePrimitive(PrimitiveType.Quad);
        face.name = name;
        face.transform.SetParent(transform, false);
        face.transform.localPosition = center;
        face.transform.localRotation = Quaternion.LookRotation(direction);
        face.GetComponent<MeshRenderer>().enabled = false; // invisible face
        BoxCollider col = face.AddComponent<BoxCollider>();
        col.size = Vector3.one * 0.1f;

        StretchFace stretch = face.AddComponent<StretchFace>();
        stretch.targetCube = this;
        stretch.direction = direction;
    }

    public void Stretch(Vector3 direction, float amount)
    {
        transform.localScale += direction * amount;
        UpdateCube(transform.localScale / 2);
    }
}
