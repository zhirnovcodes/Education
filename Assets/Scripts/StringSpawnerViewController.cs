using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StringSpawnerViewController : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Slider _slider;
    [SerializeField] private float x;
    [SerializeField] private float y;

    private float _lastValue;
    private ObjectsSpawner _spawner;

    void Start()
    {
        _lastValue = _slider.minValue - 1;
        _spawner = new ObjectsSpawner();
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastValue != _slider.value)
        {
            //_spawner.Spawn(_prefab, (int)_slider.value, x, x, y, false);
            _lastValue = _slider.value;

            CommonPressure.Reset();
        }
    }
}
