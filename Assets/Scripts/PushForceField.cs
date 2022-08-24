using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushForceField : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _power = 2;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null)
        {
            return;
        }
        var direction = (collision.attachedRigidbody.position - _rigidbody.position);
        collision.attachedRigidbody.AddForce(direction * _power);
    }
}
