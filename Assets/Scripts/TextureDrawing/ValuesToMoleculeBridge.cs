using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuesToMoleculeBridge : MonoBehaviour
{
    [SerializeField] private GraphDrawerBase _drawer;
    [SerializeField] private Transform _source;
    [SerializeField] private Material _materialWithValue;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _range = 50;

    private void OnEnable()
    {
        _materialWithValue.EnableKeyword("HAS_VALUES");

        _materialWithValue.SetTexture("_Values", _drawer.Values);
        _materialWithValue.SetInteger("_ValuesWidth", _drawer.Values.width);
        _materialWithValue.SetVector("_SourcePos", _source.transform.position);
        _materialWithValue.SetFloat("_DeltaTime", _drawer.DeltaTime);
        _materialWithValue.SetFloat("_WaveSpeed", _speed);
        _materialWithValue.SetFloat("_WaveDecay", _range);
    }

    private void OnDisable()
    {
        _materialWithValue.DisableKeyword("HAS_VALUES");
    }

    private void LateUpdate()
    {
        _materialWithValue.SetFloat("_StartTime", _drawer.TimeStart);
        _materialWithValue.SetFloat("_MaxIndex", _drawer.MaxIndex);
    }
}
