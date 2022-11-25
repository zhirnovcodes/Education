using System.Collections;
using UnityEngine;

public class MonoKeyboardController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] _behaviours;
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private KeyCode _keyCode = KeyCode.Space;

    [SerializeField] private bool _hasDefaultState = true;
    [SerializeField] private bool _dafaultState = false;
    private void Awake()
    {
        if (_hasDefaultState)
        {
            if (_behaviours != null)
            {
                foreach (var b in _behaviours)
                {
                    if (b == null)
                    {
                        continue;
                    }
                    b.enabled = _dafaultState;
                }
            }
            if (_objects != null)
            {
                foreach (var o in _objects)
                {
                    if (o == null)
                    {
                        continue;
                    }
                    o.SetActive(_dafaultState);
                }
            }

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            if (_behaviours != null)
            {
                foreach (var b in _behaviours)
                {
                    if (b == null)
                    {
                        continue;
                    }
                    b.enabled = !b.enabled;
                }
            }
            if (_objects != null)
            {
                foreach (var o in _objects)
                {
                    if (o == null)
                    {
                        continue;
                    }
                    o.SetActive(!o.activeSelf);
                }
            }
        }
    }
}
