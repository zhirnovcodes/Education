using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GraphDrawerMaterial : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private GraphDrawerBase _drawer;
    [SerializeField] private PeakAvgDrawer _peakAvgDrawer;

    void Start()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        if (_drawer == null)
        {
            _drawer = GetComponent<GraphDrawerBase>();
        }
        if (_peakAvgDrawer == null)
        {
            _peakAvgDrawer = GetComponent<PeakAvgDrawer>();
        }

        _renderer.material.mainTexture = _drawer == null ? _peakAvgDrawer.Texture : _drawer.Texture;
    }
}
