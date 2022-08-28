using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class EarDrumOffsetMaterial : MonoBehaviour
{
    [SerializeField] private EarDrumOffsetDrawer _drawer;

    void Start()
    {
        var renderer = GetComponent<Renderer>();

        renderer.material.mainTexture = _drawer.Texture;
    }
}
