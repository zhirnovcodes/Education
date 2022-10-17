using System.Collections.Generic;
using UnityEngine;

public class FunctionsBuffer : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private int _maxCounters = 20;

    private Queue<float> _values = new Queue<float>();


    public float? ValueRemoved { get; private set; }
    public float? ValueAdded { get; private set; }

    public int Count => _values.Count;
    public int CountBefore { get; private set; }

    public IEnumerable<float> Values => _values;

    private void OnEnable()
    {
        _values.Clear();
        ValueRemoved = null;
        ValueAdded = null;
        CountBefore = 0;
    }

    private void Update()
    {
        CountBefore = Count;
        ValueRemoved = null;

        if (_values.Count + 1 >= _maxCounters)
        {
            ValueRemoved = _values.Dequeue();
        }

        ValueAdded = _function.Value;
        _values.Enqueue(ValueAdded.Value);
    }
}
