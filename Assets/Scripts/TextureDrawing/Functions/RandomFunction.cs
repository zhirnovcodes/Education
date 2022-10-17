using UnityEngine;

public class RandomFunction : GraphFunctionBase
{
    [SerializeField] private float _min;
    [SerializeField] private float _max;
    [SerializeField] private float _time;

    private float _lastTime;
    private float _value;

    public override float Value => _value;

    void OnEnable()
    {
        _lastTime = -1;
        _value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastTime == -1 || Time.time - _lastTime >= _time)
        {
            _value = Random.Range(_min, _max);
            _lastTime = Time.time;
        }
    }
}
