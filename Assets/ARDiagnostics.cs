using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARDiagnostics : MonoBehaviour
{
    [SerializeField] private ARSession session;
    [SerializeField] private ARPlaneManager planeManager;

    void Update()
    {
        if (session == null || planeManager == null)
            return;

        // Log the AR session state
        Debug.Log($"AR Session State: {ARSession.state}");

        // Log number of detected planes
        int planeCount = 0;
        foreach (var plane in planeManager.trackables)
            planeCount++;

        Debug.Log($"Detected Planes: {planeCount}");
    }
}
