using System;
using System.Collections;
using UnityEngine;

public class PseudoTimer : IDisposable
{
    private MonoBehaviour _coroutineInvoker;

    public event Action<float> Tick;
    public float StartTime { get; private set; }

    private Coroutine _coroutine;

    public PseudoTimer(MonoBehaviour coroutineInvoker)
    {
        _coroutineInvoker = coroutineInvoker;
    }

    public void Dispose()
    {
        Stop();
    }

    public void Start(float interval)
    {
        Stop();

        _coroutine = _coroutineInvoker.StartCoroutine(Routine(interval));
    }

    public void Stop()
    {
        if (_coroutine != null)
        {
            _coroutineInvoker.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private float _timeLast;

    private IEnumerator Routine(float interval)
    {
        _timeLast = Time.time;
        StartTime = Time.time;

        yield return new WaitForEndOfFrame();

        while (_coroutine != null)
        {
            var deltaTime = Time.time - _timeLast;
            if (deltaTime >= interval)
            {
                var count = Mathf.RoundToInt(deltaTime / interval);
                for (int i = 0; i < count; i++)
                {
                    _timeLast += interval;
                    Tick.Invoke(_timeLast);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
