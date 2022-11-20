using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VinylDrawer))]
public class VinylDrawerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var drawer = (VinylDrawer)target;
        if (GUILayout.Button("Paint"))
        {
            drawer.Paint();
        }
        if (GUILayout.Button("Clear"))
        {
            drawer.Clear();
        }
        if (GUILayout.Button("Save"))
        {
            FilesExtensions.ShowInExplorer(drawer.Save());
        }
    }
}
