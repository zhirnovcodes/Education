using System.Linq;
using UnityEngine;

public class FluctuatingObjectMover1D : MonoBehaviour
{
    [SerializeField] private float _maxOffset = Mathf.Infinity;
    [SerializeField] private float _power = 1;

    private FluctuatingObject1D[] _flucObjects;
    private Vector2 _stablePosition;

    private void FixedUpdate()
    {
        _flucObjects = _flucObjects ?? GetComponents<FluctuatingObject1D>();

        //todo performance
        var o = _flucObjects.Select(f => f.TimeStart <= 0 ? 0 : f.Fluctuation.GetValue(Time.time - f.TimeStart)).Sum();

        o = Mathf.Clamp(o * _power, -_maxOffset, _maxOffset);

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
