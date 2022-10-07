using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GraphDrawerMaterial : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    void Start()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        var drawer = GetComponent<IGraphDrawer>();

        _renderer.material.mainTexture = drawer.Texture;
    }
}
