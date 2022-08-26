using System.Collections.Generic;
using UnityEngine;

public class ForceMover2D : MonoBehaviour
{
    private IForceProvider2D[] _providers;
    private Rigidbody2D _rigidbody;

    private List<Vector2> _forces = new List<Vector2>(3);

    private void OnEnable() 
    {
        _providers = _providers ?? GetComponents<IForceProvider2D>();
        _rigidbody = _rigidbody ?? GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var force = Vector2.zero;
        for (int i = 0; i < _providers.Length; i++)
        {
            _forces.Clear();
            var f = (_providers[i] as MonoBehaviour).isActiveAndEnabled ? _providers[i].GetForce() : Vector2.zero;
            _forces.Add(f);
            force += f;
        }

        if (_rigidbody.isKinematic)
        {
            _rigidbody.MovePosition(_rigidbody.position + force);
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
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + _forces[i]);
        }
    }
}
