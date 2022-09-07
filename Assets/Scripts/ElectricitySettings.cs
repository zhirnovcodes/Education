using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricitySettings : MonoBehaviour
{
    private static ElectricitySettings _instance;

    [SerializeField] private bool _isPluged = true;

    public void SetIsPlugged(bool value)
    {
        _isPluged = value;
    }

    public static bool IsPluged
    {
        get
        {
            return _instance?._isPluged ?? true;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
}
