using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ReverbZone))]
public class ReverbZoneEditor : Editor
{
    public void OnSceneGUI()
    {
        var zone = target as ReverbZone;
        var walls = zone.Walls;
        var p0 = zone.Source.transform.position;

        zone.Radius = Handles.RadiusHandle(Quaternion.identity, p0, zone.Radius);


        for (int i = 0; i < walls.Length; i++)
        {
            Handles.color = Color.red;
            walls[i].P1 = Handles.PositionHandle(walls[i].P1, Quaternion.identity);
            walls[i].P2 = Handles.PositionHandle(walls[i].P2, Quaternion.identity);

            var p1 = walls[i].P1;
            var p2 = walls[i].P2;

            Handles.DrawLine(p1, p2);

            var perp = (Vector3)Vector2.Perpendicular(p2 - p1).normalized;

            Handles.DrawLine(p1 + perp, p1);
            Handles.DrawLine(p2 + perp, p2);

            Handles.color = Color.yellow;
            var pos = MathIZ.ReflectedPosition(p0, walls[i]);
            if (pos == null)
            {
                continue;
            }
            Handles.RadiusHandle(Quaternion.identity, pos.Value, zone.Radius);
        }
    }
}