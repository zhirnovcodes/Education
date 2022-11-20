using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinylDrawer : MonoBehaviour
{
    [SerializeField] private CustomRenderTexture _texture;
    [SerializeField] private Texture2D _defaultTexture;
    [SerializeField] private int _radiusPixels = 5;
    [SerializeField] private Vector2 _point1;
    [SerializeField] private Vector2 _point2;
    [SerializeField] private bool _shouldSendToMaterial;

    private CustomRenderTextureUpdateZone[] _zones = new CustomRenderTextureUpdateZone[3];

    private void Awake()
    {
        if (_shouldSendToMaterial && _texture != null)
        {
            var renderer = GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                return;
            }

            _texture = CloneCustomTexture();
            Clear();

            var pb = new MaterialPropertyBlock();
            pb.SetTexture("_MainTex", _texture);
            renderer.SetPropertyBlock(pb);
        }
    }

    private CustomRenderTexture CloneCustomTexture()
    {
        var res = new CustomRenderTexture(_texture.width, _texture.height, _texture.format);
        res.initializationSource = _texture.initializationSource;
        res.initializationTexture = _texture.initializationTexture;
        res.initializationMode = _texture.initializationMode;
        res.initializationColor = _texture.initializationColor;

        res.updateMode = _texture.updateMode;
        res.updatePeriod = _texture.updatePeriod;

        res.material = _texture.material;
        res.shaderPass = _texture.shaderPass;
        res.antiAliasing = _texture.antiAliasing;
        res.anisoLevel = _texture.anisoLevel;

        return res;
    }

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

        var pos = (_point2 + _point1) / 2f;
        var d = _point2 - _point1;
        var dN = d.normalized;
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

        _zones[0].updateZoneCenter = _point1;
        _zones[0].updateZoneSize = rGlobUV;
        _zones[0].rotation = 0;

        _zones[2].updateZoneCenter = _point2;
        _zones[2].updateZoneSize = rGlobUV;
        _zones[2].rotation = 0;

        _texture.SetUpdateZones(_zones);
        _texture.Update();
    }

    public void Clear()
    {
        if (_texture == null)
        {
            return;
        }
        if (_defaultTexture != null)
        {
            _texture.initializationTexture = _defaultTexture;
        }
        _texture.Initialize();
    }

    public string Save()
    {
        return FilesExtensions.SaveRTToFile(_texture, "Vinyl");
    }
}
