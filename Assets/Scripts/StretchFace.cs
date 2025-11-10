using UnityEngine;
using UnityEngine.EventSystems;

public class StretchFace : MonoBehaviour, IDragHandler
{
    public WireframeCubeController targetCube;
    public Vector3 direction;

    public void OnDrag(PointerEventData eventData)
    {
        float dragAmount = eventData.delta.y * 0.001f;
        targetCube.Stretch(direction, dragAmount);
    }
}
