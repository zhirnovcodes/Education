using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitalCable : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private PositionOffsetDrawer _drawer;
    [SerializeField] private Color _zeroColor;
    [SerializeField] private Color _oneColor;
    [SerializeField] private float _periodGet = 0.1f;
    [SerializeField] private float _periodSend = 0.01f;

    private Queue<int> _values = new Queue<int>();
    private float _lastTime;
    private float _avg;
    private int _count;
    private Coroutine _routine;

    public event Action<int> OnDigitalValueSent;

    void OnEnable()
    {
        _drawer.OnValueAdded += ValueAdded;
        _lastTime = 0;
        _avg = 0;
        _count = 0;
    }

    private void OnDisable()
    {
        _drawer.OnValueAdded -= ValueAdded;
    }

    private void ValueAdded(int index, float value)
    {
        _avg += value;
        _count++;

        if (Time.time - _lastTime >= _periodGet)
        {
            _avg = _avg / _count;

            var intAvg = (int)_avg;

            for (int i = 0; i < 4; i++)
            {
                var b = UnityEngine.Random.Range(0, 2);
                _values.Enqueue(b);
            }

            SendBits();

            _lastTime = Time.time;
            _avg = 0;
            _count = 0;
        }

    }

    void SendBits()
    {
        if (_routine == null)
        {
            _routine = StartCoroutine(Routine());
        }
    }

    private IEnumerator Routine()
    {
        while (_values.Count > 0)
        {
            var a = _material.color.a;
            var value = _values.Count > 0 ? _values.Dequeue() : 0;
            var color = value == 0 ? _oneColor : _zeroColor;
            color.a = a;
            _material.color = color;

            OnDigitalValueSent?.Invoke(value);

            yield return new WaitForSeconds(_periodSend);
        }

        _routine = null;
    }
}
