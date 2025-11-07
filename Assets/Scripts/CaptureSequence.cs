using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class CaptureSequence : MonoBehaviour
{
    [Header("Camera & Capture Settings")]
    public Camera captureCamera;          // assign AR/Main camera in Inspector
    public int captureCount = 30;         // number of frames per dish
    public float captureInterval = 0.4f;  // seconds between captures
    public int jpgQuality = 90;           // jpg quality (0-100)

    [Header("Dish Settings")]
    public string dishName = "Pasta Alfredo"; // set dish name in Inspector
    private string dishId;               // auto-generated dish ID
    private string saveFolder;           // full folder path for this dish

    private string baseFolder = @"C:\Projects\DineAR\colmap"; // root folder on disk

    void Start()
    {
        // Auto-generate dish ID based on existing folders
        dishId = GenerateDishId();
        saveFolder = Path.Combine(baseFolder, dishId, "images");
        Directory.CreateDirectory(saveFolder);

        // Write metadata.json
        WriteMetadata();

        Debug.Log($"[CaptureSequence] Ready to capture {dishName} at {saveFolder}");
    }

    public void StartCapture()
    {
    Debug.Log("[CaptureSequence] StartCapture button pressed!");
    StartCoroutine(CaptureCoroutine());
}

IEnumerator CaptureCoroutine()
{
    Debug.Log("[CaptureSequence] CaptureCoroutine started");

    for (int i = 0; i < captureCount; i++)
    {
        yield return new WaitForEndOfFrame();

        if (captureCamera == null)
        {
            Debug.LogError("[CaptureSequence] No captureCamera assigned!");
            yield break;
        }

            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            captureCamera.targetTexture = rt;
            captureCamera.Render();
            RenderTexture.active = rt;

            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();

            captureCamera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            byte[] bytes = tex.EncodeToJPG(jpgQuality);
            string fileName = Path.Combine(saveFolder, $"img_{i:D4}.jpg");
            File.WriteAllBytes(fileName, bytes);
            Destroy(tex);

            Debug.Log($"Saved {fileName}");

            yield return new WaitForSeconds(captureInterval);
        }

        Debug.Log("Capture complete!");
    }

    private string GenerateDishId()
    {
        if (!Directory.Exists(baseFolder))
            Directory.CreateDirectory(baseFolder);

        int count = Directory.GetDirectories(baseFolder).Length + 1;
        return $"dish_{count:D3}"; // dish_001, dish_002, etc.
    }

    private void WriteMetadata()
    {
        string jsonPath = Path.Combine(baseFolder, dishId, "metadata.json");

        var metadata = new
        {
            id = dishId,
            name = dishName,
            captured_at = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssK"),
            capture_count = captureCount,
            notes = "Captured using DineAR-Scanner"
        };

        string jsonString = JsonUtility.ToJson(metadata, true);
        Directory.CreateDirectory(Path.Combine(baseFolder, dishId)); // ensure folder exists
        File.WriteAllText(jsonPath, jsonString);

        Debug.Log($"Metadata written to {jsonPath}");
    }
}
