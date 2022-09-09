using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitalCableRandom : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private Color _zeroColor;
    [SerializeField] private Color _oneColor;

    private Coroutine _routine;

    void Update()
    {
        if (_routine == null)
        {
            _routine = StartCoroutine(Send());
        }
    }

    private IEnumerator Send()
    {
        var value = Random.Range(0, 2);

        var a = _material.color.a;
        var col = value == 0 ? _zeroColor : _oneColor;
        col.a = a;
        _material.color = col;

        yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));

        _routine = null;
    }
}
