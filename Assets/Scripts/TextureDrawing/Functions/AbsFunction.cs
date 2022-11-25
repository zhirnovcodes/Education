using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsFunction : GraphFunctionBase
{
    [SerializeField] private GraphFunctionBase _func;

    public override float Value => Mathf.Abs(_func.Value);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
