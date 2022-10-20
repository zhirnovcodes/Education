using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoleculeFunction : GraphFunctionBase
{
    [SerializeField] private FluctuatingString1D[] _string;

    private Vector3? _stablePos;

    private Vector3 StablePos
    {
        get
        {
            if (_stablePos == null)
            {
                _stablePos = transform.position;
            }

            return _stablePos.Value;
        }
    }

    private void OnDisable()
    {
        _stablePos = null;
    }

    public override float Value
    {
        get
        {
            _string.Select(s =>
            {
                var maxDelay = s.transform;
                delay = Mathf.Lerp(maxDelay, 0, Mathf.Clamp01(Air.Density));

                var value = _string.Fluctuation.GetValue(_string.TimeStart - delay);
                return value;
            });

        }
    }

    private void Update()
    {
        
    }

    
}
