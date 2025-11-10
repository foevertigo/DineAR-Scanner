using UnityEngine;

public class MoveCube : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                transform.Translate(
                    touch.deltaPosition.x * 0.001f,
                    touch.deltaPosition.y * 0.001f,
                    0,
                    Space.Self
                );
            }
        }
    }
}
