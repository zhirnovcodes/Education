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

    private FluctuatingObject1D[] _flucObjects;
    private Vector2 _stablePosition;

    public float Offset { get; private set; }

    private void FixedUpdate()
    {
        _flucObjects = _flucObjects ?? GetComponents<FluctuatingObject1D>();

        //todo performance
        var o = _flucObjects.Select(f => f.TimeStart <= 0 ? 0 : f.Fluctuation.GetValue(Time.time - f.TimeStart)).Sum();

        o = Mathf.Clamp(o * _power, -_maxOffset, _maxOffset);

        Offset = o;

        transform.localPosition = _stablePosition + Vector2.right * o;
    }

    private void OnEnable()
    {
        _stablePosition = transform.localPosition;
    }

    private void OnDisable()
    {
        transform.localPosition = _stablePosition;
        _flucObjects = null;
    }
}
