using System.Linq;
using UnityEngine;

public class GraphController : MonoBehaviour
{
    private static GraphController _instance;

    [SerializeField] private float _maxOffset = 2f;
    [SerializeField] private float _multiplier = 1.5f;
    [SerializeField] private float _threshold = 1.5f;

    private GraphDrawerBase[] _drawers;
    private IThresholdFunction[] _thresholdFunctions;

    private float _threshBefore = -float.MaxValue;
    private float _maxBefore = -float.MaxValue;

    public static float? MaxOffset
    {
        get
        {
            return _instance?._maxOffset;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        _thresholdFunctions = UnityEngine.Object.FindObjectsOfType<GraphFunctionBase>().
            Where(t => t is IThresholdFunction).
            Select(t => t as IThresholdFunction).ToArray();

        _drawers = UnityEngine.Object.FindObjectsOfType<GraphDrawerBase>();

        _threshBefore = -float.MaxValue;
        _maxBefore = -float.MaxValue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            _maxOffset *= _multiplier;
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            _maxOffset /= _multiplier;
        }

        if (_threshBefore != _threshold)
        {
            _threshBefore = _threshold;
            UpdateThreshold();
        }

        if (_maxBefore != _maxOffset)
        {
            _maxBefore = _maxOffset;
            UpdateMaxValue();
        }
    }

    private void UpdateThreshold()
    {
        foreach (var th in _thresholdFunctions)
        {
            th.Threshold = _threshold;
        }
    }

    private void UpdateMaxValue()
    {
        foreach (var d in _drawers)
        {
            d.MaxOffset = _maxOffset;
        }
    }
}
