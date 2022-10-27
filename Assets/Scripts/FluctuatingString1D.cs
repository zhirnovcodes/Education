using System.Collections;
using UnityEngine;

public class FluctuatingString1D : FunctionBase, IResetable
{
    [SerializeField] private Fluctuation _fluctuation;
    [SerializeField] private float _releaseTime = 0;

    public float TimeStart { get; private set; }
    public Fluctuation Fluctuation => _fluctuation;

    private Coroutine _coroutine;

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
        Hit();
    }

    public void Hit()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        TimeStart = Time.time;
    }

    public void Stop()
    {
        if (_releaseTime > 0)
        {
            _coroutine = StartCoroutine(StopInTime());
            return;
        }

        TimeStart = 0;
    }

    public override float GetValue(float t)
    {
        if (TimeStart <= 0)
        {
            return 0;
        }

        var v = Fluctuation.GetValue(t - TimeStart);
        //Debug.Log(t + " " + v);
        return v;
    }

    private IEnumerator StopInTime()
    {
        yield return new WaitForSeconds(_releaseTime);

        TimeStart = 0;

        _coroutine = null;
    }

    public void Reset()
    {
        TimeStart = 0;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}

