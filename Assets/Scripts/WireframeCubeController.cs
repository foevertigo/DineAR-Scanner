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

        int[] order = {
            0,1,2,3,0,4,5,1,5,6,2,6,7,3,7,4
        };

        Vector3[] positions = new Vector3[order.Length];
        for (int i = 0; i < order.Length; i++)
            positions[i] = corners[order[i]];

        lr.positionCount = positions.Length;
        lr.SetPositions(positions);
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
