using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinylDrawer : MonoBehaviour
{
    [SerializeField] private CustomRenderTexture _texture;
    [SerializeField] private int _radiusPixels = 5;
    [SerializeField] private Vector2 _point1;
    [SerializeField] private Vector2 _point2;

    private CustomRenderTextureUpdateZone[] _zones = new CustomRenderTextureUpdateZone[3];

    public Vector2 Point1
    {
        set
        {
            _point1 = value;
        }
    }

    public Vector2 Point2
    {
        set
        {
            _point2 = value;
        }
    }

    public void Paint()
    {
        if (_texture == null)
        {
            return;
        }


        /*
        var pos = (_point2 + _point1) / 2f;
        var min = new Vector2( Mathf.Min(_point1.x, _point2.x), Mathf.Min(_point1.y, _point2.y));
        var max = new Vector2( Mathf.Max(_point1.x, _point2.x), Mathf.Max(_point1.y, _point2.y));


        min -= rGlobUV;
        max += rGlobUV;
        var size = max - min;

        _zones[0].updateZoneCenter = pos;
        _zones[0].updateZoneSize = size;

        _texture.material.SetVector("_RadiusV", rGlobUV);
        _texture.material.SetVector("_Point1", _point1);
        _texture.material.SetVector("_Point2", _point2);
        //_texture.material.SetFloat("_Point2Value", rLocUV);
        */


        var pos = (_point2 + _point1) / 2f;
        var d = _point2 - _point1;
        var dN = (_point2 - _point1).normalized;
        var rGlobUV = new Vector2((float)_radiusPixels / _texture.width, (float)_radiusPixels / _texture.height);
        var p = Vector2.Perpendicular(d).normalized;
        var r1 = new Vector2(p.x * rGlobUV.x, p.y * rGlobUV.y);
        var size = new Vector2(d.magnitude, r1.magnitude);
        var ang = Vector2.SignedAngle(Vector2.right, d);

        if (_zones.Length < 3)
        {
            _zones = new CustomRenderTextureUpdateZone[3];
        }

        _zones[1].updateZoneCenter = pos;
        _zones[1].updateZoneSize = size;
        _zones[1].rotation = ang;

        var r0 = new Vector2(dN.x * rGlobUV.x, dN.y * rGlobUV.y);

        _zones[0].updateZoneCenter = _point1 - dN * r0.magnitude / 2;
        _zones[0].updateZoneSize = new Vector2(r0.magnitude, r1.magnitude); ;
        _zones[0].rotation = ang;


        _zones[2].updateZoneCenter = _point2 + dN * r0.magnitude / 2;
        _zones[2].updateZoneSize = new Vector2(r0.magnitude, r1.magnitude); ;
        _zones[2].rotation = ang;

        _texture.SetUpdateZones(_zones);
        _texture.Update();
    }

    public void Clear()
    {
        if (_texture == null)
        {
            return;
        }
        _texture.Initialize();
    }
}
