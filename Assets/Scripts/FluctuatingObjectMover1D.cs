using System.Linq;
using UnityEngine;

public class FluctuatingObjectMover1D : MonoBehaviour
{
    private FluctuatingObject1D[] _flucObjects;
    private Vector2 _stablePosition;

    private void FixedUpdate()
    {
        _flucObjects = _flucObjects ?? GetComponents<FluctuatingObject1D>();

        //todo performance
        var o = _flucObjects.Select(f => f.TimeStart <= 0 ? 0 : f.Fluctuation.GetValue(Time.time - f.TimeStart)).Sum();

        transform.position = _stablePosition + Vector2.right * o;
    }

    private void OnEnable()
    {
        _stablePosition = transform.position;
    }

    private void OnDisable()
    {
        transform.position = _stablePosition;
        _flucObjects = null;
    }
}
