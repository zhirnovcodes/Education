using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FunctionBuffer : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private int _maxCount = 300;
    [SerializeField] private float _deltaTime = 0.05f;

    private List<float> _values = new List<float>();
    private List<float> _times = new List<float>();

    private Coroutine _coroutine;
    private float _value;
    private float _timeStart;
    private List<string> _logs = new List<string>();

    private PseudoTimer _timer;

    private void OnEnable()
    {
        _coroutine = StartCoroutine(QuantizeRoutine());
        if (_timer == null)
        {
            _timer = new PseudoTimer(this);
            _timer.Tick += OnTimerTick;

        }

        _timer.Start(_deltaTime);
        _timeStart = Time.time;
    }

    private void OnTimerTick(float time)
    {
        var value = _value;

        if (_values.Count + 1 > _maxCount)
        {
            _timeStart += _deltaTime;
            _values.RemoveAt(0);
        }

        _values.Add(value);
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
        _coroutine = null;
        _timer.Stop();
    }

    public float GetValue(float time)
    {
        var index = FindIndex(time);

        if (index < 0 || index >= _values.Count)
        {
            return 0;
        }

        var nextIndex = Mathf.Min(index + 1, _values.Count - 1);

        var v1 = _values[index];
        var v2 = _values[nextIndex];

        var t1 = _timeStart + _deltaTime * index;
        var t2 = _timeStart + _deltaTime * (index + 1);

        var foundValue = Mathf.Lerp(v1, v2, Mathf.InverseLerp(t1, t2, time));

        return foundValue;
    }

    private int FindIndex(float time)
    {
        return Mathf.RoundToInt( (time - _timeStart) / _deltaTime );
    }

    void Update()
    {
        _value = _function.Value;

        /*
        var val = _function.Value;

        var pixelsInSecond = Drawer.Texture.width / _timeOfFilling;

        var timeNow = Time.time - _timeStart;

        var indexNow = Mathf.RoundToInt(timeNow * pixelsInSecond);

        if (indexNow == _lastValuesIndex)
        {
            return;
        }

        for (int i = _lastValuesIndex + 1; i <= indexNow; i++)
        {
            Drawer.AddValue(i, val);
        }

        Drawer.Paint(_bckgColor, _linesColor, _maxOffset, _drawType);

        _lastValuesIndex = indexNow;
        */
    }


    private IEnumerator QuantizeRoutine()
    {
        yield return new WaitForEndOfFrame();
        _timeStart = Time.time;

        while (_coroutine != null)
        {
            yield return new WaitForSeconds(_deltaTime);

        }
    }
}
