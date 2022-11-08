using UnityEngine;

public class PeakFunction : GraphFunctionBase
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private float _declineSpeed = 0.01f;

    private float _lastPeak;

    public override float Value => _lastPeak;

    private void OnEnable()
    {
        _lastPeak = -Mathf.Infinity;
    }

    void Update()
    {
        var val = _function.Value;
        if (val < _lastPeak)
        {
            if (_lastPeak != Mathf.Infinity)
            {
                _lastPeak -= _declineSpeed * Time.deltaTime;
            }
            return;
        }
        _lastPeak = val;
    }
}
