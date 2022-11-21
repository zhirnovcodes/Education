using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VinylGraphDrawer))]
public class VinylGraphDrawerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var drawer = (VinylGraphDrawer)target;
        
    }
}
