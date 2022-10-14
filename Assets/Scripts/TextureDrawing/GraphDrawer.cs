using System;
using UnityEngine;

public class GraphDrawer : IDisposable
{
    private ComputeShader _shader;

    private int _initResultKernelIndex;
    private int _initValuesKernelIndex;
    private int _paintKernelIndex;
    private int _addValueKernelIndex;

    private const string ResultName = "Result";
    private const string ResultSizeName = "ResultSize";
    private const string ValuesName = "Values";
    private const string MaxOffsetName = "MaxOffset";
    private const string ValueSizeName = "ValuesSize";
    private const string ValueNewName = "ValueNew";
    private const string IndexNewName = "IndexNew";
    private const string BckgColName = "BckgCol";
    private const string LinesColName = "LinesCol";
    private const string DrawTypeName = "DrawType";

    private const string ShaderName = "EarDrumOffset";

    public const int DefaultTextureWidth = 256;
    public const int DefaultTextureHeight = 128;

    private bool _shouldReleaseValues = true;

    public RenderTexture Texture { get; }
    public RenderTexture Values { get; }

    public GraphDrawer(int textureWidth = DefaultTextureWidth, int textureHeight = DefaultTextureHeight)
    {
        Texture = new RenderTexture(textureWidth, textureHeight, 1);
        Texture.filterMode = FilterMode.Point;
        Texture.enableRandomWrite = true;
        Texture.Create();

        Values = new RenderTexture(Texture.width, 1, 1);
        Values.filterMode = FilterMode.Point;
        Values.enableRandomWrite = true;
        Values.format = RenderTextureFormat.RFloat;
        Values.Create();

        Init();

    }

    public GraphDrawer(RenderTexture values, int textureWidth = DefaultTextureWidth, int textureHeight = DefaultTextureHeight)
    {
        Texture = new RenderTexture(textureWidth, textureHeight, 1);
        Texture.filterMode = FilterMode.Point;
        Texture.enableRandomWrite = true;
        Texture.Create();

        _shouldReleaseValues = false;

        Values = values;

        Init();
    }

    private void Init()
    {
        _shader = (ComputeShader)GameObject.Instantiate(Resources.Load(ShaderName));

        _initResultKernelIndex = _shader.FindKernel("InitResult");
        _initValuesKernelIndex = _shader.FindKernel("InitValues");
        _paintKernelIndex = _shader.FindKernel("Paint");
        _addValueKernelIndex = _shader.FindKernel("AddValue");

        _shader.SetInt(ValueSizeName, Values.width);
        _shader.SetVector(ResultSizeName, new Vector2(Texture.width, Texture.height));

        _shader.SetTexture(_addValueKernelIndex, ValuesName, Values);
        _shader.SetTexture(_paintKernelIndex, ValuesName, Values);
        _shader.SetTexture(_initValuesKernelIndex, ValuesName, Values);

        _shader.SetTexture(_paintKernelIndex, ResultName, Texture);
        _shader.SetTexture(_initResultKernelIndex, ResultName, Texture);
    }


    public void Clear()
    {
        ClearValues();
        ClearTexture();
    }

    public void ClearValues()
    {
        _shader.Dispatch(_initValuesKernelIndex, Values.width / 8, 1, 1);
    }

    public void ClearTexture()
    {
        _shader.Dispatch(_initResultKernelIndex, Texture.width / 8, Texture.height / 8, 1);
    }

    public void AddValue(int x, float value)
    {
        _shader.SetFloat(ValueNewName, value);
        _shader.SetInt(IndexNewName, x);
        _shader.Dispatch(_addValueKernelIndex, Values.width / 8, 1, 1);

    }

    public void Paint(Color bckgColor, Color dotsColor, float maxOffset, DrawType type = DrawType.Lines)
    {
        _shader.SetFloat(MaxOffsetName, maxOffset);
        _shader.SetInt(DrawTypeName, (int)type);

        _shader.SetVector(BckgColName, bckgColor);
        _shader.SetVector(LinesColName, dotsColor);
        _shader.Dispatch(_paintKernelIndex, Texture.width / 8, Texture.height / 8, 1);
    }

    public void Dispose()
    {
        Texture.Release();
        if (_shouldReleaseValues)
        {
            Values.Release();
        }

        if (_shader != null)
        {
            UnityEngine.Object.Destroy(_shader);
        }
    }

    public enum DrawType
    {
        Lines = 0,
        Point = 1,
        FillColor = 2,
        FillColorBckg = 3
    }
}

/*
private Queue<PeakValley> _peaks = new Queue<PeakValley>(10);
private Queue<PeakValley> _valleys = new Queue<PeakValley>(10);
private float _minVal = float.MaxValue;
private float _maxVal = float.MinValue;
*/


/*
private float CalcMax(float value, int index, int indexMin)
{
    var delta = value - _lastValue;

    var wasChanged = false;

    if (delta < 0)
    {
        _peaks.Enqueue(new PeakValley { Value = _lastValue, Index = index - 1 });
        wasChanged = true;
    }

    while (_peaks.Count > 0 && _peaks.Peek().Index < indexMin)
    {
        _peaks.Dequeue();
        wasChanged = true;
    }

    if (wasChanged)
    {
        var max = float.MinValue;
        foreach (var p in _peaks)
        {
            max = Mathf.Max(max, p.Value);
        }

        _maxVal = max;
    }
    else
    {
        _maxVal = Mathf.Max(_maxVal, value);
    }
    _maxVal = Mathf.Max(_maxVal, value);
    return _maxVal;
}

private float CalcMin(float value, int index, int indexMin)
{
    var delta = value - _lastValue;

    var wasChanged = false;

    if (delta > 0)
    {
        _valleys.Enqueue(new PeakValley { Value = _lastValue, Index = index });
        wasChanged = true;
    }

    while (_valleys.Count > 0 && _valleys.Peek().Index < indexMin)
    {
        _valleys.Dequeue();
        wasChanged = true;
    }


    if (wasChanged)
    {
        var min = float.MaxValue;
        foreach (var v in _valleys)
        {
            min = Mathf.Min(min, v.Value);
        }
        _minVal = min;
    }
    else
    {
        _minVal = Mathf.Min(_minVal, value);
    }

    _minVal = Mathf.Min(_minVal, value);
    return _maxVal;
}

private struct PeakValley
{
    public float Value;
    public int Index;
}
*/
