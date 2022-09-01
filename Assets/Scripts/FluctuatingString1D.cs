using UnityEngine;

public class FluctuatingString1D : FluctuatingObject1D
{
    [SerializeField] private Fluctuation _fluctuation;

    private float _timeStart;
    public override float TimeStart => _timeStart;
    public override Fluctuation Fluctuation => _fluctuation;

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
        _fluctuation.Length = value;
    }

    public void SetAtt(float value)
    {
        _fluctuation.Attack = value;
    }

    private void OnEnable()
    {
        _timeStart = 0;
    }

    public void Hit()
    {
        _timeStart = Time.time;
    }
}

