using System;
using UnityEngine;

public interface IGraphDrawer
{
    RenderTexture Texture { get; }
}

public class PositionOffsetDrawer : MonoBehaviour, IDisposable, IGraphDrawer
{

    [SerializeField] private Transform _target;
    [SerializeField, Range(0.01f, 7)] private float _maxOffset = 1f;
    [SerializeField] private float _timeOfFilling = 8f;

    [SerializeField] private Color _bckgColor = new Color(0,0,0,0);
    [SerializeField] private Color _linesColor = new Color(1,1,1,1);

    [SerializeField] private int _textureWidth = GraphDrawer.DefaultTextureWidth;
    [SerializeField] private int _textureHeight = GraphDrawer.DefaultTextureHeight;

    [SerializeField] private GraphDrawer.DrawType _drawType = GraphDrawer.DrawType.Lines;

    private GraphDrawer _drawer;


    private float _timeStart;
    private int _lastValuesIndex;
    private Vector3 _stablePos;

    public event Action<int, float> OnValueAdded;

    public Transform Target
    {
        set
        {
            _target = value;
            _stablePos = _target.position;
        }
    }

    public RenderTexture Texture 
    {
        get
        {
            _drawer = _drawer ?? new GraphDrawer(_textureWidth, _textureHeight);
            return _drawer.Texture;
        }
    }

    public float MaxOffset
    {
        get
        {
            return _maxOffset;
        }
    }

    public RenderTexture Values
    {
        get 
        { 
            _drawer = _drawer ?? new GraphDrawer(_textureWidth, _textureHeight);
            return _drawer.Values;
        }
    }

    private void OnEnable()
    {
        _drawer.Clear();

        _lastValuesIndex = -1;
        _timeStart = Time.time;
    }

    void Awake()
    {
        if (_target != null)
        {
            _stablePos = _target.position;
        }

        _drawer = _drawer ?? new GraphDrawer(_textureWidth, _textureHeight);
    }

    void Update()
    {
        if (_target == null)
        {
            return;
        }

        var offset = (_target.position - _stablePos).magnitude * Mathf.Sign((_target.position - _stablePos).x);

        var pixelsInSecond = _drawer.Texture.width / _timeOfFilling;

        var timeNow = Time.time - _timeStart;

        var indexNow = Mathf.RoundToInt( timeNow * pixelsInSecond);

        if (indexNow == _lastValuesIndex)
        {
            return;
        }

        for (int i = _lastValuesIndex + 1; i <= indexNow; i++)
        {
            var value = offset;
            _drawer.AddValue(i, value);

            OnValueAdded?.Invoke(i, value);
        }

        _drawer.Paint(_bckgColor, _linesColor, _maxOffset, _drawType);

        _lastValuesIndex = indexNow;
    }

    public void Dispose()
    {
        _drawer.Dispose();
    }
}
