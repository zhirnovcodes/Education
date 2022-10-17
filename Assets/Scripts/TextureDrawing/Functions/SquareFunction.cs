using UnityEngine;

public class SquareFunction : GraphFunctionBase
{
    [SerializeField] private float _amplitude = 1;
    [SerializeField] private float _period = 1;
    
    private float _value;
    private float _lastTime;
    private bool _boolVal;

    public override float Value => _value;

    void OnEnable()
    {
        _value = _amplitude;
        _lastTime = 0;
        _boolVal = true;
    }

    void Update()
    {
        if (Time.time - _lastTime >= _period)
        {
            _lastTime = Time.time;
            _boolVal = !_boolVal;
            _value = _amplitude * (_boolVal ? 1 : -1);
        }
    }
}
