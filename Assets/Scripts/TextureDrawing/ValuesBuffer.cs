using System;
using UnityEngine;

public class ValuesBuffer : IDisposable
{
    private readonly float _deltaTime = 0.05f;

    private ComputeShader _shader;

    private int _initKernelIndex;
    private int _setValueKernelIndex;
    private int _shiftKernelIndex;

    private const string ShaderName = "ValuesBuffer";

    public RenderTexture Values 
    {
        get;
        private set;
    }

    public ValuesBuffer(int capacity, float deltaTime)
    {
        _deltaTime = deltaTime;

        Values = new RenderTexture(capacity + 2, 1, 1);
        Values.filterMode = FilterMode.Point;
        Values.enableRandomWrite = true;
        Values.format = RenderTextureFormat.RFloat;
        Values.Create();
    }

    public void Init()
    {
        if (_shader == null)
        {
            _shader = (ComputeShader)GameObject.Instantiate(Resources.Load(ShaderName));

            _initKernelIndex = _shader.FindKernel("Init");
            _setValueKernelIndex = _shader.FindKernel("SetValue");
            _shiftKernelIndex = _shader.FindKernel("ShiftValues");

            _shader.SetInt("ValuesSize", Values.width);

            _shader.SetTexture(_initKernelIndex, "Values", Values);
            _shader.SetTexture(_setValueKernelIndex, "Values", Values);
            _shader.SetTexture(_shiftKernelIndex, "Values", Values);
        }

        _shader.SetFloat("DeltaTime", _deltaTime);
        _shader.SetFloat("StartTime", Time.time);
        _shader.Dispatch(_initKernelIndex, Mathf.Max(Values.width / 8, 1), 1, 1);
    }

    public void SetValue(int index, float value)
    {
        _shader.SetInt("IndexNew", index);
        _shader.SetFloat("ValueNew", value);
        _shader.Dispatch(_initKernelIndex, Mathf.Max(1, 1), 1, 1);
    }

    public void Shift(int shiftCount = 1)
    {
        _shader.SetInt("CountShifted", shiftCount);
        _shader.Dispatch(_shiftKernelIndex, Mathf.Max(Values.width / 8, 1), 1, 1);
    }

    public void Clear()
    {
        Init();
    }

    public void Dispose()
    {
        Values.Release();

        if (_shader != null)
        {
            UnityEngine.Object.Destroy(_shader);
        }
    }
}
