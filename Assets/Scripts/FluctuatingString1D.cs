using UnityEngine;

public class FluctuatingString1D : FunctionBase
{
    [SerializeField] private Fluctuation _fluctuation;

    public float TimeStart { get; private set; }
    public Fluctuation Fluctuation => _fluctuation;

    public void SetFrq(float value)
    {
        _fluctuation.Frequency = value;
    }

    public void SetAmp(float value)
    {
        _fluctuation.Amplitude = value;
    }

    public void SetLen(float value)
    {
        _fluctuation.Time = value;
    }

    public void SetAtt(float value)
    {
        _fluctuation.Attack = value;
    }

    private void OnEnable()
    {
        TimeStart = 0;
    }

    public void Hit()
    {
        TimeStart = Time.time;
    }

    public override float GetValue(float t)
    {
        return Fluctuation.GetValue(t);
    }
}

