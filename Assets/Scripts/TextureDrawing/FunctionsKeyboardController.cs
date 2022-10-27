using System.Linq;
using UnityEngine;

public class FunctionsKeyboardController : MonoBehaviour
{
    [SerializeField] private GraphFunctionBase[] _functions;
    [SerializeField] private bool _autoAssign = true;

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
        if (Input.GetKeyDown(KeyCode.Space))
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
