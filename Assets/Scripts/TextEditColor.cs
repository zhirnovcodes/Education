using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEditColor : MonoBehaviour
{
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private TMPro.TMP_Text[] _texts;

    private void OnValidate()
    {
        if (_texts == null)
        {
            return;
        }
        foreach (var t in _texts)
        {
            if (t == null)
            {
                continue;
            }

            t.color = _color;
        }
    }
}
