using System.Linq;
using UnityEngine;

public class PressureAffective : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    public PressureField Field { set; private get; }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody2D.AddForce(Field.Power);
        
    }
}
