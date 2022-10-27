using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molecule2D : MonoBehaviour
{
    [SerializeField] private FunctionBuffer _buffer;
    [SerializeField] private ReverbZone _reverb;

    private Vector3 _stablePosition;

    private void OnEnable()
    {
        _stablePosition = transform.position;
    }

    private void OnDisable()
    {
        transform.position = transform.position;
    }

    private void Update()
    {
        if (!enabled)
        {
            return;
        }

        var t = 0f;
        var d = Vector3.zero;

        foreach (var dir in _reverb.Directions(_stablePosition))
        {
            t = Mathf.Max(dir.w, t);
            d += (Vector3)dir;
            break;
        }

        transform.position = _stablePosition + d * _buffer.GetValue(Time.time - t);
    }
}
