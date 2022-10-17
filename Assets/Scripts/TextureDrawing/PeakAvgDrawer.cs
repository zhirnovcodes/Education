using System;
using UnityEngine;

public class PeakAvgDrawerAdapter : IDisposable
{
    private int _resetKernelIndex;
    private int _calcKernelIndex;
    private int _paintKernelIndex;

    public readonly ComputeBuffer _buffer;

    private ComputeShader _shader;


    private RenderTexture _values;

    public RenderTexture Texture { get; }

    private const string ShaderName = "AvgPeakDrawer";

    public PeakAvgDrawerAdapter(RenderTexture values, int height)
    {
        _values = values;

        Texture = new RenderTexture(1, height, 1);
        Texture.filterMode = FilterMode.Point;
        Texture.enableRandomWrite = true;
        Texture.Create();

        _shader = (ComputeShader)GameObject.Instantiate(Resources.Load(ShaderName));

        // Kernel

        _resetKernelIndex = _shader.FindKernel("Reset");
        _calcKernelIndex = _shader.FindKernel("Calculate");
        _paintKernelIndex = _shader.FindKernel("Paint");

        // Int

        _shader.SetInt("ValuesSize", values.width);
        _shader.SetInt("ResultHeight", height);

        // Textures

        _shader.SetTexture(_paintKernelIndex, "Result", Texture);
        _shader.SetTexture(_resetKernelIndex, "Result", Texture);
        _shader.SetTexture(_calcKernelIndex, "Values", values);

        // Buffer

        _buffer = new ComputeBuffer(3, 4);
        const string buffName = "Buff";
        _shader.SetBuffer(_resetKernelIndex, buffName, _buffer);
        _shader.SetBuffer(_paintKernelIndex, buffName, _buffer);
        _shader.SetBuffer(_calcKernelIndex,  buffName, _buffer);
    }

    public void Draw(Color bckgColor, Color avgColor, Color peakColor, float transparency, float maxOffset)
    {
        _shader.Dispatch(_resetKernelIndex, 1, Texture.height / 8, 1);

        _shader.Dispatch(_calcKernelIndex, 1, 1, 1);

        _shader.SetFloat("MaxOffset", maxOffset);
        _shader.SetFloat("Transparency", transparency);

        _shader.SetVector("BckgCol", bckgColor);
        _shader.SetVector("AvgCol", avgColor);
        _shader.SetVector("PeakCol", peakColor);

        _shader.Dispatch(_paintKernelIndex, 1, Texture.height / 8, 1);
    }

    public void Dispose()
    {
        Texture.Release();

        if (_shader != null)
        {
            UnityEngine.Object.Destroy(_shader);
        }
    }
}

public class PeakAvgDrawer : MonoBehaviour, IGraphDrawer, IDisposable
{
    [SerializeField] private GraphDrawerBase _drawer;
    [SerializeField, Range(0.01f, 7)] private float _maxOffset = 1f;
    [SerializeField, Range(0, 1)] private float _transparency = 0.1f;

    [SerializeField] private Color _bckgColor = new Color(0, 0, 0, 0);
    [SerializeField] private Color _avgColor = new Color(0, 1, 0, 1);
    [SerializeField] private Color _peakColor = new Color(0, 1, 1, 1);

    public RenderTexture Texture
    {
        get
        {
            Start();
            return _adapter.Texture;
        }
    }

    private PeakAvgDrawerAdapter _adapter;
    private void Start()
    {
        _adapter = _adapter ?? new PeakAvgDrawerAdapter(_drawer.Values, 128);
    }

    private void Update()
    {
        var maxOffset = GraphController.MaxOffset ?? _maxOffset;
        _adapter.Draw(_bckgColor, _avgColor, _peakColor, _transparency, maxOffset);
    }

    public void Dispose()
    {
        _adapter.Dispose();
    }
}