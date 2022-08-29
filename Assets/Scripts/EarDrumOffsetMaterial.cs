using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class EarDrumOffsetMaterial : MonoBehaviour
{
    [SerializeField] private PositionOffsetDrawer _drawer;

    void Start()
    {
        var renderer = GetComponent<Renderer>();

        renderer.material.mainTexture = _drawer.Texture;
    }
}
