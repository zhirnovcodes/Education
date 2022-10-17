using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IFluctuatingObject1D
{
    public float Offset { get; }
}

public class FluctuatingObjectMover1D : MonoBehaviour, IFluctuatingObject1D
{
    [SerializeField] private float _maxOffset = Mathf.Infinity;
    [SerializeField] private float _power = 1;
    [SerializeField] private FunctionOperation _operation = FunctionOperation.Plus;

    private GraphFunctionBase[] _functions;
    private Vector2 _stablePosition;

    public float Offset { get; private set; }

    private void FixedUpdate()
    {
        _functions = _functions ?? GetComponents<GraphFunctionBase>();

        //todo performance
        float o = 0;
        switch (_operation)
        {
            case FunctionOperation.Plus:
                o = Values().Sum();
                break;
            case FunctionOperation.Multiply:
                o = Values().Aggregate((float v1, float v2) => 
                {
                    return v1 * v2;
                }
                );
                break;
            case FunctionOperation.Minus:
                o = Values().Aggregate((float v1, float v2) =>
                {
                    return v1 - v2;
                }
                );
                break;
            case FunctionOperation.Divide:
                o = Values().Aggregate((float v1, float v2) =>
                {
                    return v1 / v2;
                }
                );
                break;
            default:
                throw new System.NotImplementedException();

        }

        o = Mathf.Clamp(o * _power, -_maxOffset, _maxOffset);

        Offset = o;

        transform.localPosition = _stablePosition + Vector2.right * o;
    }

    private IEnumerable<float> Values()
    {
        return _functions.Select(f => f.Value);
    }

    private void OnEnable()
    {
        _stablePosition = transform.localPosition;
    }

    private void OnDisable()
    {
        transform.localPosition = _stablePosition;
        _functions = null;
    }
}
