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
        if (value > Threshold)
        {
            // if not running
            if (_startTime < 0 || Time.time - _startTime >= Attack + Ratio)
            {
                _startTime = Time.time;
            }
        }
        else if (_startTime < 0)
        {
            _value = 0;
            return;
        }

        var time = Time.time - _startTime;

        // function ended
        if (time >= Attack + Ratio)
        {
            _startTime = -1;
            _value = 0;
            return;
        }

        // function is going
        var delta = value - Threshold;

        var functionNormValue = time >= Attack + Release ? 0 :
            (time <= Attack ?
            Mathf.InverseLerp(0, Attack, time) :
            Mathf.InverseLerp(Attack + Release, Attack, time));

        var functionScale = 1f;//Ratio * Mathf.Max( delta, 0 );

        _value = functionNormValue * functionScale;

    }
}
