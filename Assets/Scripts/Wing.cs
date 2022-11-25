using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private Vector3 _targetScale;
    [SerializeField] private Color _targetColor = Color.gray;
    [SerializeField] private float _functionPower = 1;
    [SerializeField] private bool _inverse;
    [SerializeField] private bool _isLeft;

    private Vector3 _startPosition;
    private Vector3 _startScale;
    private Color _startColor;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _startPosition = transform.position;
        _startScale = transform.localScale;
        _startColor = _renderer?.color ?? Color.white;
    }

    private void OnDisable()
    {
        transform.position = _startPosition;
        transform.localScale = _startScale;
    }

    void LateUpdate()
    {
        var val = _function.Value;
        val = Mathf.Clamp01(Mathf.Abs((val) * _functionPower));

        var newScale = Vector3.Lerp(_startScale, _targetScale, val);
        var scaleChange = newScale - _startScale;
        transform.position = _startPosition + scaleChange * (_isLeft ? -1 : 1) / 2;
        transform.localScale = newScale;

        if (_renderer == null)
        {
            return;
        }
        _renderer.color = Color.Lerp(_startColor, _targetColor, val);
    }
}
