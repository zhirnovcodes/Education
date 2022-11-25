using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VinylDrawer))]
public class VinylGraphDrawer : MonoBehaviour
{
    [SerializeField] private FunctionBase _function;
    [SerializeField] private MeshFilter _mesh;
    [SerializeField] private float _timeOfFilling = 8;
    [SerializeField, Range(0, 3)] private float _scale = 1;
    [SerializeField, Range(0, 1)] private float _startUOffset = 0.1f;
    [SerializeField, Range(0, 1)] private float _endU = 1f;
    [SerializeField, Range(0, 1)] private float _startV = 0.5f;
    [SerializeField] private bool _shouldClearOnEnable = true;
    [SerializeField, Range(0.0001f, 10)] private float _drawGizmosSize = 5f;
    [SerializeField, Range(0, 15)] private float _drawGizmosTime = 0f;

    private VinylDrawer _drawer;
    private float _timeStart;
    private float _timeEnd;
    private float _valueBefore;
    private Vector2 _uvCurrent;

    public float RecordedT => this.enabled ? Mathf.Min(Time.time - _timeStart, _timeOfFilling) : _timeEnd;


    public Vector3 GetPosition(float time)
    {
        _mesh = _mesh ?? GetComponent<MeshFilter>();
        var size = _mesh == null ? new Vector2(1, 1) : new Vector2(_mesh.sharedMesh.bounds.size.x, _mesh.sharedMesh.bounds.size.z);
        var value = _function?.GetValue(time) ?? 0;
        var uv = new Vector2(time / _timeOfFilling + _startUOffset, value / 2f * _scale + _startV);

        uv.x = Mathf.Clamp(uv.x, 0, _endU);

        var pos = uv - Vector2.one * 0.5f;
        pos.x *= size.x;
        pos.y *= size.y;

        var localPos = new Vector3(-pos.x, 0, pos.y);
        var globalPos = transform.TransformPoint( localPos );
        return globalPos;
    }

    private VinylDrawer Drawer
    {
        get
        {
            _drawer = _drawer ?? GetComponent<VinylDrawer>();
            return _drawer;
        }
    }

    private void OnEnable()
    {
        _timeStart = Time.time;

        _valueBefore = GetValueAsV();
        if (_shouldClearOnEnable)
        {
            Drawer.Clear();
        }
        _uvCurrent = Vector2.zero;
    }

    private void OnDisable()
    {
        _timeEnd = Time.time - _timeStart;
    }

    private float TimeToU(float time)
    {
        return Mathf.InverseLerp(_timeStart, _timeStart + _timeOfFilling, time) + _startUOffset;
    }

    private float GetValueAsV()
    {
        return _function.GetValue(RecordedT) / 2f * _scale + _startV;
    }

    void Update()
    {
        var t = Time.time;
        var val = GetValueAsV();

        if (t >= _timeStart + _timeOfFilling)
        {
            return;
        }

        float t1 = TimeToU(t - Time.deltaTime);
        float t2 = TimeToU(t);

        Paint(t1, t2, _valueBefore, val);

        _valueBefore = val;
    }

    private void Paint(float t1, float t2, float val1, float val2)
    {
        if (t2 > _endU)
        {
            return;
        }

        _uvCurrent = new Vector2(t2, val1);

        Drawer.Point1 = new Vector2(t1, val2);
        Drawer.Point2 = new Vector2(t2, val1);

        Drawer.Paint();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(GetPosition(_drawGizmosTime), _drawGizmosSize);
    }
}
