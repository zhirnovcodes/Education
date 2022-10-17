using UnityEngine;

public class LerpFunction : GraphFunctionBase
{
    [SerializeField] private GraphFunctionBase _f1;
    [SerializeField] private GraphFunctionBase _f2;
    [SerializeField] private float _ratio = 0.5f;
    
    public override float Value => Mathf.Lerp(_f1.Value, _f2.Value, _ratio);

}
