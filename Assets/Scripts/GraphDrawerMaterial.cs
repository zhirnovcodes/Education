using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GraphDrawerMaterial : MonoBehaviour
{
    void Start()
    {
        var renderer = GetComponent<Renderer>();
        var drawer = GetComponent<IGraphDrawer>();

        renderer.material.mainTexture = drawer.Texture;
    }
}
