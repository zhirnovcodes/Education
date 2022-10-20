using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FunctionBuffer : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private bool _shouldCacheFoundValues = true;
    [SerializeField] private int _maxCount = 300;

    private List<float> _values = new List<float>();
    private List<float> _times = new List<float>();
    private Dictionary<float, int> _cachedValues = new Dictionary<float, int>();

    public float GetValue(float time)
    {
        int index;

        if (_shouldCacheFoundValues && _cachedValues.ContainsKey(time))
        {
            index = _cachedValues[time];
        }
        else
        {
            index = FindIndex(time);

            if (_shouldCacheFoundValues)
            {
                _cachedValues.Add(time, index);
            }
        }

        if (index < 0)
        {
            return 0;
        }

        var nextIndex = Mathf.Min(index + 1, _values.Count - 1);

        var v1 = _values[index];
        var v2 = _values[nextIndex];

        var t1 = _times[index];
        var t2 = _times[nextIndex];

        var foundValue = Mathf.Lerp(v1, v2, Mathf.InverseLerp(t1, t2, time));

        return foundValue;
    }

    private int FindIndex(float time)
    {
        for (int i = 0; i < _times.Count; i++)
        {
            if (_times[i] > time)
            {
                return i - 1;
            }
        }

        return -1;
    }

    private void Update()
    {
        _cachedValues.Clear();

        var value = _function.Value;
        var time = Time.time;

        if (_values.Count + 1 >= _maxCount)
        {
            _values.RemoveAt(0);
            _times.RemoveAt(0);
        }

        _values.Add(value);
        _times.Add(time);
    }
}
