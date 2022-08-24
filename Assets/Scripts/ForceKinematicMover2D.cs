using UnityEngine;

public class ForceKinematicMover2D : MonoBehaviour
{
    private IForceProvider2D[] _providers;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _providers = GetComponents<IForceProvider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var force = Vector2.zero;
        for (int i = 0; i < _providers.Length; i++)
        {
            force += _providers[i].Force;
        }

        if (_rigidbody.isKinematic)
        {
            _rigidbody.MovePosition(_rigidbody.position + force * Time.fixedDeltaTime);
        }
    }
}
