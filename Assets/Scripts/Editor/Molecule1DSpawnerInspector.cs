using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Molecules1DSpawner))]
public class Molecule1DSpawnerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var spawner = (Spawner)target;
        if (GUILayout.Button("Spawn"))
        {
            spawner.Spawn();
        }
        if (GUILayout.Button("Clear"))
        {
            spawner.Clear();
        }
    }
}
