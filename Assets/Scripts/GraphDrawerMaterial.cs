using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GraphDrawerMaterial : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private GraphDrawerBase _drawer;

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

        _renderer.material.mainTexture = _drawer.Texture;
    }
}
