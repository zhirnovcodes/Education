using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionScaler : MonoBehaviour
{
    [SerializeField] private Vector3 _direction = Vector3.right;
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private float _scale = 1;

    private Vector3 _scaleStart;

    private void OnEnable()
    {
        _scaleStart = transform.localScale;
    }


    // Update is called once per frame
    void Update()
    {
        transform.localScale = _scaleStart + _direction * _function.Value * _scale;
    }
}
