using UnityEngine;

public class FunctionColorChanger : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase _function;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color _colorZero = Color.white;
    [SerializeField] private Color _color1 = Color.black;
    [SerializeField] private Color _colorMinus1 = Color.black;
    [SerializeField] private float _scale = 1;

    void Update()
    {
        var val = Mathf.Clamp( _function.Value * _scale, -1, 1);

        _renderer.color = val < 0 ? Color.Lerp(_colorMinus1, _colorZero, val + 1) : Color.Lerp(_colorZero, _color1, val);
    }
}
