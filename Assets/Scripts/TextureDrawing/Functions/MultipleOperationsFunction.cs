using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleOperationsFunction : GraphFunctionBase
{
    [SerializeField] private List<GraphFunctionBase> _functions;
    [SerializeField] private FunctionOperation _operation;

    public override float Value
    {
        get
        {
            if (_functions != null)
            {
                float? r = null;
                foreach (var f in _functions)
                {
                    if (f != null)
                    {
                        if (r == null)
                        {
                            r = f.Value;
                            continue;
                        }

                        r = FunctionOperationFunction.Operation(_operation, r.Value, f.Value);
                    }
                }

                return r ?? 0;
            }
            return 0;
        }
    }

}
