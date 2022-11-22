using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMoveController : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private float _height = 0;

    private Vector3? _stablePos;

    void OnEnable()
    {
        _stablePos = null;
    }

    private void OnDisable()
    {
        
    }

    void Update()
    {
        _stablePos = _stablePos ?? _parent.transform.position;
        var offset = _parent.transform.position - _stablePos.Value;

        transform.position = _stablePos.Value + offset / 2 + Vector3.up * _height;
        var scale = transform.localScale;
        scale.x = offset.x;
        transform.localScale = scale;
    }
}
