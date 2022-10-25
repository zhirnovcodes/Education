using UnityEngine;

public class GraphDrawerBase : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;

    [SerializeField, Range(0.01f, 7)] private float _maxOffset = 1f;
    [SerializeField] private float _timeOfFilling = 8f;

    [SerializeField] private Color _bckgColor = new Color(0, 0, 0, 0);
    [SerializeField] private Color _linesColor = new Color(1, 1, 1, 1);

    [SerializeField] private int _textureWidth = GraphDrawer.DefaultTextureWidth;
    [SerializeField] private int _textureHeight = GraphDrawer.DefaultTextureHeight;

    [SerializeField] private GraphDrawer.DrawType _drawType = GraphDrawer.DrawType.Lines;
    [SerializeField] private bool _drawBeforeFirstValue = false;
    [SerializeField] private bool _antiAliasing = false;

    private GraphDrawer _drawer;

    private GraphDrawer Drawer
    {
        get
        {
            _drawer = _drawer ?? new GraphDrawer(_textureWidth, _textureHeight, _antiAliasing);
            return _drawer;
        }
    }


    private float _timeStart;
    private int _lastValuesIndex;

    public RenderTexture Texture
    {
        get
        {
            return Drawer.Texture;
        }
    }

    public float MaxOffset
    {
        get
        {
            return _maxOffset;
        }
        set
        {
            _maxOffset = value;
        }
    }

    public RenderTexture Values
    {
        get
        {
            return Drawer.Values;
        }
    }

    public GraphFunctionBase Function
    {
        set
        {
            _function = value;
        }
    }

    private void OnEnable()
    {
        Drawer.Clear();

        _lastValuesIndex = -1;
        _timeStart = Time.time;
    }

    void Update()
    {
        var val = _function?.Value ?? 0;

        var pixelsInSecond = Drawer.Texture.width / _timeOfFilling;

        var timeNow = Time.time - _timeStart;

        var indexNow = Mathf.RoundToInt(timeNow * pixelsInSecond);

        if (indexNow == _lastValuesIndex)
        {
            return;
        }

        for (int i = _lastValuesIndex + 1; i <= indexNow; i++)
        {
            Drawer.AddValue(i, val);
        }

        Drawer.Paint(_bckgColor, _linesColor, _maxOffset, _drawType);

        _lastValuesIndex = indexNow;
    }

    public void Dispose()
    {
        _drawer.Dispose();
    }
}
