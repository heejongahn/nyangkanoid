using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    private void OnSceneGUI()
    {
        LevelManager levelManager = (LevelManager)target;

        if (levelManager.levelSettings == null)
        {
            return;
        }


        Handles.Label(levelManager.transform.position, $"Aspect Ratio: {levelManager.levelSettings.aspectRatio}");
        Handles.color = Color.green;
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawLevelAspectRatio(LevelManager levelManager, GizmoType gizmoType)
    {
        if (levelManager.levelSettings == null)
        {
            return;
        }

        float targetAspect = levelManager.levelSettings.aspectRatio;
        float height = Camera.main.orthographicSize * 2;
        float width = height * targetAspect;

        Vector3 center = Camera.main.transform.position;
        Vector3 topLeft = center + new Vector3(-width / 2, height / 2, 0);
        Vector3 topRight = center + new Vector3(width / 2, height / 2, 0);
        Vector3 bottomLeft = center + new Vector3(-width / 2, -height / 2, 0);
        Vector3 bottomRight = center + new Vector3(width / 2, -height / 2, 0);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
