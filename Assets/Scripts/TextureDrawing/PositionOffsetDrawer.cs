using System;
using UnityEngine;

public class PositionOffsetDrawer : MonoBehaviour, IDisposable
{

    [SerializeField] private Transform _target;
    [SerializeField, Range(0.001f, 2f)] private float _analysisDeltaSec = 0.1f;
    [SerializeField, Range(0.01f, 7)] private float _maxOffset = 1f;

    [SerializeField] private Color _bckgColor = new Color(0,0,0,0);
    [SerializeField] private Color _linesColor = new Color(1,1,1,1);

    [SerializeField] private int _textureWidth = GraphDrawer.DefaultTextureWidth;
    [SerializeField] private int _textureHeight = GraphDrawer.DefaultTextureHeight;

    private GraphDrawer _drawer;

    private float _timeWithoutPainting;
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

    private void OnEnable()
    {
        _drawer.Clear();

        _lastValuesIndex = 0;
        _timeWithoutPainting = 0;
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
        
        if (_timeWithoutPainting >= _analysisDeltaSec)
        {
            _timeWithoutPainting = 0;


            var timeIndex = Mathf.RoundToInt( Mathf.Clamp(_lastValuesIndex, 0, Texture.width));
            var value = offset;
            _drawer.AddValue(timeIndex, value);
            _drawer.Paint(_bckgColor, _linesColor, _maxOffset);

            _lastValuesIndex++;
        }
        else
        {
            _timeWithoutPainting += Time.deltaTime;
        }

    }

    public void Dispose()
    {
        _drawer.Dispose();
    }
}
