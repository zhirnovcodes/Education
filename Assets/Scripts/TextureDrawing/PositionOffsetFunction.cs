using UnityEngine;

public class PositionOffsetFunction : GraphFunctionBase
{
    [SerializeField] private Transform _target;
    private Vector3? _stablePos;

    public Transform Target
    {
        set
        {
            _target = value;
            _stablePos = _target.position;
        }
    }

    private void Awake()
    {
        if (_target != null)
        {
            _stablePos = _target.position;
        }
    }

    public override float Value
    {
        get
        {
            if (_stablePos == null)
            {
                return 0;
            }

            var distance = _target.position - _stablePos.Value;
            return distance.magnitude * Mathf.Sign(distance.x);
        }
    }
}
