using System.Collections.Generic;
using UnityEngine;

public class OffsetToElectricity : MonoBehaviour
{
    [SerializeField] private Transform _main;

    private Vector3 _stablePos;

    public float U { get; private set; }

    private void Awake()
    {
        _stablePos = _main.position;
    }

    private void Update()
    {
        var offset = (_main.position - _stablePos).magnitude * Mathf.Sign((_main.position - _stablePos).x);
        U = offset;

    }
}
