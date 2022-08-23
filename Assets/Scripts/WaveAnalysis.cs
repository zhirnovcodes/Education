using UnityEngine;

public class WaveAnalysis : MonoBehaviour
{
    [SerializeField] private EarDrumOffsetDrawer _drawer;
    [SerializeField] private SpriteRenderer _renderer;

    private Sprite _sprite;

    void Start()
    {
        _drawer = _drawer ?? GetComponentInChildren<EarDrumOffsetDrawer>();
        _renderer = _renderer ?? GetComponent<SpriteRenderer>();

        //_renderer.sprite = _drawer.Texture;

        var material = _renderer.sharedMaterial;
        material.SetTexture("_MainTex", _drawer.Texture);
    }

    void Update()
    {
    }
}
