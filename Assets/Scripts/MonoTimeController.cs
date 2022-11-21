using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTimeController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] _behaviours;
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private float _timeStart;
    [SerializeField] private float _workingTime;

    private float _enabledTime;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        _enabledTime = Time.time;
    }

    void Update()
    {
        var t = Time.time - _enabledTime;
        var isEnabled = t >= _timeStart && t <= _timeStart + _workingTime; 
        if (_behaviours != null)
        {
            foreach (var b in _behaviours)
            {
                if (b == null)
                {
                    continue;
                }
                b.enabled = isEnabled;
            }
        }
        if (_objects != null)
        {
            foreach (var o in _objects)
            {
                if (o == null)
                {
                    continue;
                }
                o.SetActive(isEnabled);
            }
        }
    }
}
