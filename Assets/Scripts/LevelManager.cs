using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelSettingsSO levelSettings;

    private void Start()
    {
        if (levelSettings != null)
        {
            ApplyAspectRatio();
        }
    }

    private void ApplyAspectRatio()
    {
        // TODO:
        // Add fixed width letterbox so that UI can fit in

        float targetAspect = levelSettings.aspectRatio;
        float screenAspect = (float)Screen.width / Screen.height;

        Camera cam = Camera.main;
        if (cam == null) return;

        if (screenAspect >= targetAspect) // Wider than target -> Add pillarboxing (black bars on left/right)
        {
            float scaleHeight = targetAspect / screenAspect;
            cam.rect = new Rect(
                (1f - scaleHeight) / 2f, // Center the camera horizontally
                0f,
                scaleHeight,
                1f
            );
        }
        else // Taller than target -> Add letterboxing (black bars on top/bottom)
        {
            float scaleWidth = screenAspect / targetAspect;
            cam.rect = new Rect(
                0f,
                (1f - scaleWidth) / 2f, // Center the camera vertically
                1f,
                scaleWidth
            );
        }

    }
}
