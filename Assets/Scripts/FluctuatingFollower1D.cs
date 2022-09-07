using UnityEngine;

public class FluctuatingFollower1D : MonoBehaviour
{
    [SerializeField] private FluctuatingObjectMover1D _source;
    [SerializeField] private bool _normalized;
    [SerializeField] private float _maxOffset = 2;
    [SerializeField] private bool _reflected;
    [SerializeField] private bool _zeroIfUnplugged = false;

    private Vector3? _stablePosition;

    void FixedUpdate()
    {
        var offset = _source.Offset;

        offset = _normalized ? Mathf.InverseLerp(-_maxOffset, _maxOffset, offset) : offset;
        offset = _reflected ? offset * -1 : offset;

        offset = _zeroIfUnplugged ? (ElectricitySettings.IsPluged ? offset : 0) : offset;

        _stablePosition = _stablePosition ?? transform.localPosition;

        transform.localPosition = _stablePosition.Value + Vector3.right * offset;
    }
}
