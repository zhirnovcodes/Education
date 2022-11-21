using UnityEngine;

public class VinylPlane : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Texture _defaultAlphaMap;
    [SerializeField] private Rect _mainTexUvRect = new Rect(new Vector2(0,0), new Vector2(1,1));
    [SerializeField] private Rect _alphaMapUvRect = new Rect(new Vector2(0,0), new Vector2(1,1));

    private void OnValidate()
    {
        SetData();
    }

    private void Awake()
    {
        SetData();
    }

    private Vector4 RectToV4(Rect rect)
    {
        return new Vector4(rect.size.x, rect.size.y,
            rect.position.x, rect.position.y);
    }

    private void SetData()
    {
        _renderer = _renderer ?? GetComponent<MeshRenderer>();
        Debug.Assert(_renderer != null);

        var pb = new MaterialPropertyBlock();
        pb.SetVector("_MainTex_ST", RectToV4(_mainTexUvRect));
        if (_defaultAlphaMap != null)
        {
            pb.SetTexture("_AlphaMap", _defaultAlphaMap);
        }
        else
        {

            pb.SetTexture("_AlphaMap", Texture2D.whiteTexture);
        }
        pb.SetVector("_AlphaMap_ST",RectToV4(_alphaMapUvRect));
        _renderer.SetPropertyBlock(pb);
    }
}
