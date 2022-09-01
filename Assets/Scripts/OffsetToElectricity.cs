using System.Collections.Generic;
using UnityEngine;

public class OffsetToElectricity : MonoBehaviour
{
    [SerializeField] private Transform _main;
    [SerializeField] private List<SpriteRenderer> _sprites;
    [SerializeField] private Color _zero = new Color(0,0,0,1);
    [SerializeField] private Color _one = new Color(0.5f,0.5f,0.5f,1);

    private Vector3 _stablePos;

    private void Awake()
    {
        _stablePos = _main.position;
    }

    private void Update()
    {
        var offset = (_main.position - _stablePos).magnitude * Mathf.Sign((_main.position - _stablePos).x);
        //var offset = _main.TimeStart <= 0 ? 0 : ((_main.Fluctuation.GetValue(Time.time - _main.TimeStart) / _main.Fluctuation.Amplitude + 1) / 2f);
        var col = Color.Lerp(_zero, _one, offset);

        foreach (var spr in _sprites)
        {
            spr.color = col;
        }
    }
}
