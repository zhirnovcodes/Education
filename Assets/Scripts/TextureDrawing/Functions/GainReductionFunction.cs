using UnityEngine;

public class GainReductionFunction : GraphFunctionBase, IThresholdFunction
{
    public float Attack;
    public float Release;
    public float Ratio;

    [SerializeField] private float _threshold;
    [SerializeField] private GraphFunctionBase _function;

    private float _value;
    private float _startTime = -1;
    private float _releaseTime = -1;
    private float _releaseScale = -1;

    public override float Value
    {
        get
        {
            return _value;
        }
    }

    public float Threshold 
    { 
        get 
        {
            return _threshold;
        } 
        set
        {
            _threshold = value;
        }
    }

    private void OnEnable()
    {
        _startTime = -1;
        _value = 0;
    }

    private void Update()
    {
        var value = _function.Value;

        // defining current state
        if (_startTime < 0)
        {
            if (value > Threshold)
            {
                _startTime = Time.time;
                _releaseTime = -1;
            }
        }
        else if (_releaseTime < 0)
        {
            if (value <= Threshold)
            {
                _releaseTime = Time.time;
                _startTime = -1;
            }
        }
        else if (_releaseTime >= 0 && Time.time - _releaseTime >= Release)
        {
            _releaseTime = -1;
            _startTime = -1;
        }

        // calculating GR
        var grValue = _startTime >= 0 ? Mathf.InverseLerp(_startTime, _startTime + Attack, Time.time) :
            (_releaseTime >= 0 ? Mathf.InverseLerp(_releaseTime + Release, _releaseTime, Time.time) : 0);

        var grScale = _releaseTime >= 0 ? _releaseScale : Mathf.Lerp( 1, Threshold / value, Ratio);

        if (_releaseTime < 0)
        {
            _releaseScale = grScale; 
        }

        _value = -1 * grScale * grValue;
    }
}
