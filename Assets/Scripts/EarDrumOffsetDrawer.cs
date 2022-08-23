using UnityEngine;

public class EarDrumOffsetDrawer : MonoBehaviour
{
    [SerializeField] private ComputeShader _shader;
    public RenderTexture Texture { get; private set; }

    private int _updateKernelIndex;

    void Awake()
    {
        Texture = new RenderTexture(256, 128, 1);
        Texture.filterMode = FilterMode.Point;
        Texture.enableRandomWrite = true;
        Texture.Create();
    }

    void Start()
    {
        var kernel = _shader.FindKernel("Init");
        _shader.SetTexture(kernel, "Result", Texture);
        _shader.Dispatch(kernel, Texture.width / 8, Texture.height / 8, 1);

        _updateKernelIndex = _shader.FindKernel("Update");
    }

    // Update is called once per frame
    void Update()
    {
        _shader.SetTexture(_updateKernelIndex, "Result", Texture);
        _shader.Dispatch(_updateKernelIndex, Texture.width / 8, Texture.height / 8, 1);
    }
}
