using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTimeController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _behaviour;
    [SerializeField] private float _timeStart;
    [SerializeField] private float _timeEnd;

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
        _behaviour.enabled = t >= _timeStart && t <= _timeEnd;
    }
}
