using UnityEngine;

public class FunctionMover1D : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private float _power = 1;
    [SerializeField] private Vector3 _speed = Vector2.right;

    private Vector3 _stablePosition;

    public float Offset { get; private set; }

    private void FixedUpdate()
    {
        Offset = _function.Value * _power;

        transform.localPosition = _stablePosition + _speed * Offset;
    }

    private void OnEnable()
    {
        _stablePosition = transform.localPosition;
    }

    private void OnDisable()
    {
        transform.localPosition = _stablePosition;
    }
}
