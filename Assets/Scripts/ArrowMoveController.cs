using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ArrowMoveController : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private float _height = 0;

    private Vector3 _stablePos;

    void OnEnable()
    {
        _stablePos = _parent.transform.localPosition;
    }

    void Update()
    {
        var offset = _parent.transform.localPosition - _stablePos;

        transform.localPosition = _stablePos + offset / 2 + Vector3.up * _height;
        var scale = transform.localScale;
        scale.x = offset.x;
        transform.localScale = scale;
    }
}
