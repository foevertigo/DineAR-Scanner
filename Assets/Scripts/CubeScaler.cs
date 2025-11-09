using UnityEngine;

public class CubeScaler : MonoBehaviour
{
    private WireframeCube wireCube;
    private float initialDistance;
    private float initialSize;

    void Start()
    {
        wireCube = GetComponent<WireframeCube>();
    }

    void Update()
    {
        // two-finger pinch detected
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            // distance between fingers now and before
            float currentDist = Vector2.Distance(t0.position, t1.position);
            float prevDist = Vector2.Distance(t0.position - t0.deltaPosition, t1.position - t1.deltaPosition);
            float diff = currentDist - prevDist;

            // scale change
            if (Mathf.Abs(diff) > 1f)
            {
                float scaleFactor = 1 + diff * 0.002f;
                wireCube.SetCubeSize(wireCube.cubeSize * scaleFactor);
            }
        }
    }
}

