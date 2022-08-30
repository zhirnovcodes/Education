using UnityEngine;

public class PositionOffsetDrawer : MonoBehaviour
{
    private const string ResultName = "Result";
    private const string ResultSizeName = "ResultSize";
    private const string ValuesName = "Values";
    private const string MaxOffsetName = "MaxOffset";
    private const string ValueSizeName = "ValuesSize";
    private const string ValueNewName = "ValueNew";
    private const string IndexNewName = "IndexNew";

    private const string BckgColName = "BckgColor";
    private const string LinesColName = "LinesCol";

    [SerializeField] private Transform _target;
    [SerializeField] private ComputeShader _shader;
    [SerializeField, Range(0.001f, 2f)] private float _analysisDeltaSec = 0.1f;
    [SerializeField, Range(0.01f, 7)] private float _maxOffset = 1f;
    [SerializeField] private RenderTexture _valuesTexture;

    [SerializeField] private Color _bckgColor = new Color(0,0,0,0);
    [SerializeField] private Color _linesColor = new Color(1,1,1,1);

    private int _initResultKernelIndex;
    private int _initValuesKernelIndex;
    private int _paintKernelIndex;
    private int _addValueKernelIndex;

    private float _timeWithoutPainting;
    private int _lastValuesIndex;
    private Vector3 _stablePos;
    private RenderTexture _mainTexture;

    /*
    private Queue<PeakValley> _peaks = new Queue<PeakValley>(10);
    private Queue<PeakValley> _valleys = new Queue<PeakValley>(10);
    private float _minVal = float.MaxValue;
    private float _maxVal = float.MinValue;
    */
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
            if (_mainTexture == null)
            {
                _mainTexture = new RenderTexture(256, 128, 1);
                _mainTexture.filterMode = FilterMode.Point;
                _mainTexture.enableRandomWrite = true;
                _mainTexture.Create();
            }
            return _mainTexture;
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
            if (value == 0)
            {
                return;
            }

            _maxOffset = Mathf.Abs(value);
        }
    }

    private void OnEnable()
    {
        _shader.Dispatch(_initValuesKernelIndex, _valuesTexture.width / 8, 1, 1);
        _shader.Dispatch(_initResultKernelIndex, Texture.width / 8, Texture.height / 8, 1);

        _lastValuesIndex = 0;
        _timeWithoutPainting = 0;
    }

    void Awake()
    {
        if (_target != null)
        {
            _stablePos = _target.position;
        }

        _valuesTexture = new RenderTexture(256, 1, 1);
        _valuesTexture.filterMode = FilterMode.Point;
        _valuesTexture.enableRandomWrite = true;
        _valuesTexture.format = RenderTextureFormat.RFloat;
        _valuesTexture.Create();

        _shader = (ComputeShader)Instantiate(Resources.Load(_shader.name));

        _initResultKernelIndex = _shader.FindKernel("InitResult");
        _initValuesKernelIndex = _shader.FindKernel("InitValues");
        _paintKernelIndex = _shader.FindKernel("Paint");
        _addValueKernelIndex = _shader.FindKernel("AddValue");

        _shader.SetInt(ValueSizeName, _valuesTexture.width);
        _shader.SetVector(ResultSizeName, new Vector2(Texture.width, Texture.height));

        _shader.SetTexture(_addValueKernelIndex, ValuesName, _valuesTexture);
        _shader.SetTexture(_paintKernelIndex, ValuesName, _valuesTexture);
        _shader.SetTexture(_initValuesKernelIndex, ValuesName, _valuesTexture);

        _shader.SetTexture(_paintKernelIndex, ResultName, Texture);
        _shader.SetTexture(_initResultKernelIndex, ResultName, Texture);
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

            var timeIndex = Mathf.RoundToInt( Mathf.Clamp(_lastValuesIndex, 0, _valuesTexture.width));

            _shader.SetVector(BckgColName, _bckgColor);
            _shader.SetVector(LinesColName, _linesColor);

            _shader.SetFloat(ValueNewName, offset);
            _shader.SetFloat(MaxOffsetName, _maxOffset);
            _shader.SetInt(IndexNewName, timeIndex);
            _shader.Dispatch(_addValueKernelIndex, _valuesTexture.width / 8, 1, 1);
            _shader.Dispatch(_paintKernelIndex, Texture.width / 8, Texture.height / 8, 1);

            _lastValuesIndex++;
        }
        else
        {
            _timeWithoutPainting += Time.deltaTime;
        }

    }

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
}
