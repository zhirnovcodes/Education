using System.Linq;
using UnityEngine;

public class FluctuatingObjectMover1D : MonoBehaviour
{
    private IFluctuatingObject1D[] _flucObjects;
    private Vector2 _stablePosition;

    private void FixedUpdate()
    {
        _flucObjects = _flucObjects ?? GetComponents<IFluctuatingObject1D>();

        //todo performance
        var o = _flucObjects.Select(f => f.GetX()).Sum();

        transform.position = _stablePosition + Vector2.right * o;
    }

    private void OnEnable()
    {
        _stablePosition = transform.position;
    }

    private void OnDisable()
    {
        _flucObjects = null;
    }
}
