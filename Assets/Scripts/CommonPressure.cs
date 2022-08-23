using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommonPressure : MonoBehaviour
{
    private static CommonPressure _instance;

    public static Vector3 Power => _instance._power;

    private PressureField[] _pressureFields;
    private Vector3 _power;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _pressureFields = GameObject.FindGameObjectsWithTag("Pressure").Select(g => g.GetComponent<PressureField>()).ToArray();
    }

    public static void Reset()
    {
        _instance.Start();
    }

    private void Update()
    {
        _power = Vector3.zero;
        foreach (var p in _pressureFields)
        {
            _power += p.Power;
        }
    }
}
