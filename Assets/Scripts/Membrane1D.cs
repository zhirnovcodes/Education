using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Membrane1D : MonoBehaviour, IForceProvider2D
{
    [SerializeField] private float _power = 1;
    [SerializeField] private float _stabelizeDistance = 0.2f;
    [SerializeField] private float _maxDeviation = 1f;

    private Rigidbody2D _rigidbody2D;
    private Vector2? _stablePosition;

    public Vector2 Force { get; private set; }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _stablePosition = _stablePosition ?? _rigidbody2D.position;

        var dir = _stablePosition.Value - _rigidbody2D.position;
        var sqrDist = dir.sqrMagnitude;

        if (sqrDist < _stabelizeDistance * _stabelizeDistance)
        {
            Force = Vector2.zero;
            return;
        }

        //var dirPower = Mathf.Clamp01( Mathf.InverseLerp(_stabelizeDistance * _stabelizeDistance, _maxDeviation * _maxDeviation, sqrDist) );
        var dirPower = Mathf.Clamp01( Mathf.InverseLerp(_stabelizeDistance * _stabelizeDistance, _maxDeviation * _maxDeviation, sqrDist) ) * _power * Time.fixedDeltaTime;
        Force = dir.normalized * dirPower;
    }
}
