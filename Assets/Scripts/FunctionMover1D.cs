using UnityEngine;

public class FunctionMover1D : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;

    private Vector2 _stablePosition;

    public float Offset { get; private set; }

    private void FixedUpdate()
    {
        Offset = _function.Value;

        transform.localPosition = _stablePosition + Vector2.right * Offset;
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