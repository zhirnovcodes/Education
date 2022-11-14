using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    private void OnSceneGUI()
    {
        var controller = target as CameraController;

        var rect = controller.Borders;

        var newRect = DrawRect(rect);

        controller.Borders = newRect;
    }

    private Rect DrawRect(Rect rect)
    {
        var p1 = (Vector2)Handles.PositionHandle(rect.min, Quaternion.identity);
        var p3 = (Vector2)Handles.PositionHandle(rect.max, Quaternion.identity);

        var height = (p3 - p1).y;

        var p2 = p1 + Vector2.up * height;
        var p4 = p3 - Vector2.up * height;

        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);
        Handles.DrawLine(p3, p4);
        Handles.DrawLine(p4, p1);

        return new Rect(p1, p3 - p1);
    }
}
