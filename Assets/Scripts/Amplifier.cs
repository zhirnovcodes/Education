using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amplifier : MonoBehaviour
{
    [SerializeField] private Transform _main;
    [SerializeField] private Color _zero = new Color(0, 0, 0, 1);
    [SerializeField] private Color _one = new Color(0.5f, 0.5f, 0.5f, 1);

    private Vector3 _stablePos;

    public float Offset
    {
        get
        {
            return (_main.position - _stablePos).magnitude * Mathf.Sign((_main.position - _stablePos).x) * 2;
        }
    }

    private void Awake()
    {
        _stablePos = _main.position;
    }

    private void Update()
    {
    }
}
