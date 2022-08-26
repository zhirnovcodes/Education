using System.Collections.Generic;
using UnityEngine;

public class ForceMover2D : MonoBehaviour
{
    private enum ForceMixType
    {
        Add,
        Avg
    }

    [SerializeField] private float _speed = 1;
    [SerializeField] private ForceMixType _mixType = ForceMixType.Avg;


    private IForceProvider2D[] _providers;
    private Rigidbody2D _rigidbody;

    private List<Vector2> _forces = new List<Vector2>(3);
    private Vector2 _velocityLast;
    private Vector2? _stablePosition;

    private void Awake()
    {
        _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();
    }

    private void OnEnable() 
    {
        _providers = GetComponents<IForceProvider2D>();
    }

    private void OnDisable()
    {
        _rigidbody.position = _stablePosition ?? Vector2.zero;
        _stablePosition = null;
        _velocityLast = Vector2.zero;
    }

    void FixedUpdate()
    {
        _stablePosition = _stablePosition ?? transform.position;

        var force = Vector2.zero;
        for (int i = 0; i < _providers.Length; i++)
        {
            _forces.Clear();
            var f = (_providers[i] as MonoBehaviour).isActiveAndEnabled ? _providers[i].GetForce() : Vector2.zero;
            _forces.Add(f);
            force += f;
        }

        if (_mixType == ForceMixType.Avg)
        {
            force /= _providers.Length;
        }

        if (_rigidbody.isKinematic)
        {
            var velocity = force * Time.fixedDeltaTime + _velocityLast;
            velocity *= 1 - Air.Drag;
            _rigidbody.MovePosition(_rigidbody.position + velocity * _speed);
            _velocityLast = velocity;
        }
        else
        {
            _rigidbody.AddForce(force);
        }
    }
    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < _forces.Count; i++)
        {
            Gizmos.color = new Color(((float)i / _forces.Count), 1, 1);
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + _forces[i]);
        }
    }
}
