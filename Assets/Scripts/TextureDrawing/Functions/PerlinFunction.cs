using UnityEngine;

public class PerlinFunction : FunctionBase
{
    [SerializeField] private float _xScale = 1;
    [SerializeField] private float _x;
    [SerializeField] private float _y;

    private float _timeStart = 0;

    private void OnEnable()
    {
        _timeStart = Time.time;
    }

    public override float Value => GetValue(Time.time - _timeStart);

    public override float GetValue(float t)
    {
        t += _x;
        var n = Mathf.PerlinNoise(t * _xScale, _y);
        return (n - 0.5f) * 2f;
    }
}
