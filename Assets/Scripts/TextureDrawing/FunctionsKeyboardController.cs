using System.Linq;
using UnityEngine;

public class FunctionsKeyboardController : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase[] _functions;
    [SerializeField] private bool _autoAssign = true;
    [SerializeField] private KeyCode _code = KeyCode.Space; 

    private void Awake()
    {
        if (_autoAssign)
        {
            _functions = GetComponents<GraphFunctionBase>().ToArray();
        }

        ResetAll();
    }

    void Update()
    {
        if (Input.GetKeyDown(_code))
        {
            foreach (var f in _functions)
            {
                f.enabled = !f.enabled;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetAll();
        }
    }

    private void ResetAll()
    {
        foreach (var f in _functions)
        {
            f.enabled = false;
            if (f is IResetable res)
            {
                res.Reset();
            }
        }
    }
}
