using UnityEngine;

public class EarDrumOffsetDrawer : MonoBehaviour
{
    private const string ResultName = "Result";
    private const string ResultSizeName = "ResultSize";
    private const string ValuesName = "Values";
    private const string ValueScaleName = "ValueScale";
    private const string ValueSizeName = "ValuesSize";
    private const string ValueNewName = "ValueNew";
    private const string IndexNewName = "IndexNew";

    [SerializeField] private Transform _target;
    [SerializeField] private ComputeShader _shader;
    [SerializeField, Range(0.001f, 2f)] private float _analysisDeltaSec = 0.1f;
    [SerializeField, Range(0.01f, 7)] private float _minOffset = 1f;
    [SerializeField] private RenderTexture _valuesTexture;

    private int _initResultKernelIndex;
    private int _initValuesKernelIndex;
    private int _paintKernelIndex;
    private int _addValueKernelIndex;
    private int _scaleKernelIndex;

    private float _timeWithoutPainting;
    private int _lastValuesIndex;
    private float _lastOffset;
    private Vector2 _stablePos;
    private RenderTexture _mainTexture;

    /*
    private Queue<PeakValley> _peaks = new Queue<PeakValley>(10);
    private Queue<PeakValley> _valleys = new Queue<PeakValley>(10);
    private float _minVal = float.MaxValue;
    private float _maxVal = float.MinValue;
    */
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

    public float MinOffset
    {
        get
        {
            return _minOffset;
        }
        set
        {
            if (value == 0)
            {
                return;
            }

            _minOffset = Mathf.Abs(value);
        }
    }

    private void OnEnable()
    {
        _shader.Dispatch(_initValuesKernelIndex, _valuesTexture.width / 8, 1, 1);
        _shader.Dispatch(_initResultKernelIndex, Texture.width / 8, Texture.height / 8, 1);
    }

    void Awake()
    {
        _stablePos = _target.position;
        _lastOffset = _minOffset;

        _valuesTexture = new RenderTexture(256, 1, 1);
        _valuesTexture.filterMode = FilterMode.Trilinear;
        _valuesTexture.enableRandomWrite = true;
        _valuesTexture.format = RenderTextureFormat.RFloat;
        _valuesTexture.Create();

        _shader = (ComputeShader)Instantiate(Resources.Load(_shader.name));

        _initResultKernelIndex = _shader.FindKernel("InitResult");
        _initValuesKernelIndex = _shader.FindKernel("InitValues");
        _paintKernelIndex = _shader.FindKernel("Paint");
        _addValueKernelIndex = _shader.FindKernel("AddValue");
        _scaleKernelIndex = _shader.FindKernel("ScaleAndOffsetValues");

        _shader.SetInt(ValueSizeName, _valuesTexture.width);
        _shader.SetVector(ResultSizeName, new Vector2(Texture.width, Texture.height));

        _shader.SetTexture(_addValueKernelIndex, ValuesName, _valuesTexture);
        _shader.SetTexture(_paintKernelIndex, ValuesName, _valuesTexture);
        _shader.SetTexture(_initValuesKernelIndex, ValuesName, _valuesTexture);
        _shader.SetTexture(_scaleKernelIndex, ValuesName, _valuesTexture);

        _shader.SetTexture(_paintKernelIndex, ResultName, Texture);
        _shader.SetTexture(_initResultKernelIndex, ResultName, Texture);
    }

    // Update is called once per frame
    void Update()
    {
        var posX = _target.position.x - _stablePos.x;

        if (_timeWithoutPainting >= _analysisDeltaSec)
        {
            _timeWithoutPainting = 0;

            var timeIndex = Mathf.RoundToInt( Mathf.Clamp(_lastValuesIndex, 0, _valuesTexture.width));

            //max = (max - min) >= _maxHeight ? max : min + _maxHeight;
            var normValue = (Mathf.InverseLerp(-_minOffset, _minOffset, posX) - 0.5f) * 2f;

            if (_lastOffset != _minOffset)
            {
                _shader.SetFloat(ValueScaleName, _lastOffset / _minOffset);
                _shader.Dispatch(_scaleKernelIndex, Texture.width / 8, 1, 1);
            }

            _shader.SetFloat(ValueNewName, normValue);
            _shader.SetInt(IndexNewName, timeIndex);
            _shader.Dispatch(_addValueKernelIndex, _valuesTexture.width / 8, 1, 1);
            _shader.Dispatch(_paintKernelIndex, Texture.width / 8, Texture.height / 8, 1);

            _lastValuesIndex++;
            _lastOffset = _minOffset;
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
