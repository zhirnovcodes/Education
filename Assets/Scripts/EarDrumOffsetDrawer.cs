using System.Collections.Generic;
using UnityEngine;

public class EarDrumOffsetDrawer : MonoBehaviour
{
    [SerializeField] private ComputeShader _shader;
    [SerializeField] private int _count = 10;
    [SerializeField, Range(0.01f, 1f)] private float _length = 0.01f;
    [SerializeField, Range(1, 20f)] private float _scale = 1f;
    public RenderTexture Texture { get; private set; }

    private int _updateKernelIndex;

    private Rigidbody2D _rigidbody;
    private float _lastX;
    private float _lastTime;
    private float _currentTime;
    private float _minPosX;
    private float _maxPosX;

    private Queue<float> _xValues = new Queue<float>(10);
    private Vector2 _stablePos;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _stablePos = _rigidbody.position;
        /*
        Texture = new RenderTexture(256, 128, 1);
        Texture.filterMode = FilterMode.Point;
        Texture.enableRandomWrite = true;
        Texture.Create();
        */
    }

    void Start()
    {
        /*
        var kernel = _shader.FindKernel("Init");
        _shader.SetTexture(kernel, "Result", Texture);
        _shader.Dispatch(kernel, Texture.width / 8, Texture.height / 8, 1);

        _updateKernelIndex = _shader.FindKernel("Update");
        */
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        var posX = _rigidbody.position.x - _stablePos.x;

        if (_xValues.Count >= _count && _count > 0)
        {
            _xValues.Dequeue();
        }
        _lastTime = _currentTime;
        _currentTime = Time.realtimeSinceStartup;
        _lastX = CurrentValue();
        _xValues.Enqueue(posX);


        /*
        _shader.SetTexture(_updateKernelIndex, "Result", Texture);
        _shader.Dispatch(_updateKernelIndex, Texture.width / 8, Texture.height / 8, 1);
        */
    }

    private void OnDrawGizmos()
    {
        var lastVal = 0f;

        Gizmos.color = Color.red;

        var t = 0f;

        foreach (var val in _xValues)
        {
            if (t < _length)
            {
                lastVal = val;
                t += _length;
                continue;
            }

            var a = Vector2.up * val * _scale + Vector2.right * t + _stablePos;
            var b = Vector2.up * lastVal * _scale + Vector2.right * ( t - _length ) + _stablePos;
            lastVal = val;

            Gizmos.DrawLine(a, b);

            t += _length;
        }


    }

    private float CurrentValue()
    {
        return _xValues.Count == 0 ? 0 : _xValues.Peek();
    }
}
