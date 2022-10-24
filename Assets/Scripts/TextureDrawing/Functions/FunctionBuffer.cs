using UnityEngine;

public class FunctionBuffer : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private float _maxTime = 4;
    [SerializeField] private float _deltaTime = 0.05f;

    private IndexedQueue<float> _queue;

    private float _value;
    private float _timeStart;

    private PseudoTimer _timer;

    private void OnEnable()
    {
        if (_queue == null)
        {
            var count = Mathf.CeilToInt(_maxTime / _deltaTime);
            _queue = new IndexedQueue<float>(count);
        }

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
        if (_queue.Add(_value))
        {
            _timeStart += _deltaTime;
        }
    }

    private void OnDisable()
    {
        _timer.Stop();
    }

    public float GetValue(float time)
    {
        if (_queue == null)
        {
            return 0;
        }

        var index = FindIndex(time);

        if (index < 0)
        {
            return 0;
        }

        index = Mathf.Min(index, _queue.Count - 1);

        var nextIndex = Mathf.Min(index + 1, _queue.Count - 1);

        var v1 = _queue[index];
        var v2 = _queue[nextIndex];

        var t1 = _timeStart + _deltaTime * index;
        var t2 = _timeStart + _deltaTime * (index + 1);

        var foundValue = Mathf.Lerp(v1, v2, Mathf.InverseLerp(t1, t2, time));

        return foundValue;
    }

    private int FindIndex(float time)
    {
        return Mathf.FloorToInt( (time - _timeStart) / _deltaTime );
    }

    void Update()
    {
        _value = _function.Value;
    }
}
