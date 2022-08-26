using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushForceField : MonoBehaviour
{
    [SerializeField] private float _power = 2;
    private Rigidbody2D _rigidbody;

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(0);
        if (collision.rigidbody == null)
        {
            return;
        }
        var direction = (collision.rigidbody.position - _rigidbody.position);
        collision.rigidbody.AddForce(direction * _power);
    }
}
