using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuesToMoleculeBridge : MonoBehaviour
{
    [SerializeField] private GraphDrawerBase _drawer;
    [SerializeField] private Transform _source;
    [SerializeField] private Material _materialWithValue;
    [SerializeField] private Color _color = new Color(1,1,1,0.7f);
    [SerializeField] private float _speed = 1;
    [SerializeField, Range(-2, 10)] private float _scale = 1;
    [SerializeField] private Vector3 _noiseSpeed = new Vector3(0.5f, 0.5f, 1);
    [SerializeField] private Vector3 _noiseAmp = new Vector3(1f, 1f, 1);
    [SerializeField] private float _range = 50;
    [SerializeField, Range(0, 1)] private float _functionPower = 1;
    [SerializeField, Range(0, 1)] private float _waveColorValue = 0;
    [SerializeField, Range(0, 1)] private float _waveScale = 0;
    [SerializeField, Range(0, 1)] private float _radian = 0;
    [SerializeField] private bool _shouldFollowSource = false;
    [SerializeField] private bool _hasValues = true;

    private void OnEnable()
    {
        if (_hasValues)
        {
            _materialWithValue.EnableKeyword("HAS_VALUES");
        }
        else
        {
            _materialWithValue.DisableKeyword("HAS_VALUES");
        }
        
        _materialWithValue.SetVector("_SourcePos", _source.transform.position);
        _materialWithValue.SetTexture("_Values", _drawer.Values);
        _materialWithValue.SetInteger("_ValuesWidth", _drawer.Values.width);
        _materialWithValue.SetFloat("_DeltaTime", _drawer.DeltaTime);

    }

    private void OnDisable()
    {
        _materialWithValue.DisableKeyword("HAS_VALUES");

    }

    private void LateUpdate()
    {

        _materialWithValue.SetFloat("_WaveSpeed", _speed);
        _materialWithValue.SetFloat("_WaveDecay", _range);
        _materialWithValue.SetFloat("_StartTime", _drawer.TimeStart);
        _materialWithValue.SetFloat("_MaxIndex", _drawer.MaxIndex);
        _materialWithValue.SetFloat("_WaveColorValue", _waveColorValue);
        _materialWithValue.SetFloat("_FunctionPower", _functionPower);
        _materialWithValue.SetFloat("_WaveScale", _waveScale);
        _materialWithValue.SetFloat("_IsRadiant", _radian);
        _materialWithValue.SetFloat("_Scale", _scale);
        _materialWithValue.SetVector("_Speed", _noiseSpeed);
        _materialWithValue.SetVector("_Color", _color);
        _materialWithValue.SetVector("_NoiseAmplitude", _noiseAmp);

        if (_shouldFollowSource)
        {
            _materialWithValue.SetVector("_SourcePos", _source.transform.position);
        }

    }
}
