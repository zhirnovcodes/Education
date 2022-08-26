using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceForceProvider2D : MonoBehaviour, IForceProvider2D
{
    [SerializeField] private float _power = 1;
    private Vector2 _force;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private RaycastHit2D[] _hits = new RaycastHit2D[1];

    public Vector2 GetForce()
    {
        _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();
        _collider = _collider ?? GetComponent<Collider2D>();

        if (_collider.Raycast(Vector2.zero, _hits, 0) > 0)
        {
            var direction = _rigidbody.position - _hits[0].rigidbody.position;
            return direction.normalized * _power * Time.fixedDeltaTime;
        }

        return Vector2.zero;
    }
}
