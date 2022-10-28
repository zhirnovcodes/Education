using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct Line
{
    public Vector3 P1;
    public Vector3 P2;

    public Vector3 Direction => P2 - P1;

    public float Length { get => Direction.magnitude; }
}

public class ReverbZone : MonoBehaviour
{
    [SerializeField] private Line[] _walls;
    [SerializeField] private Transform _source;
    [SerializeField] private float _radius = 1;
    [SerializeField] private float _timeScale = 1;

    public static ReverbZone Instance { get; private set; }

    public Line[] Walls => _walls;

    public float Radius { get => _radius; set { _radius = value; } }

    private Vector3[] _reflectedPositions = new Vector3[0];

    public Transform Source
    {
        get
        {
            if (_source == null)
            {
                _source = transform;
            }
            return _source;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        _reflectedPositions = _walls.Select(w => MathIZ.ReflectedPosition(Source.position, w)).Where(w => w.HasValue).Select(w => w.Value).ToArray();
    }

    private void OnDrawGizmosSelected()
    {
        //for (int i = 0; i < _walls.Length; i++)
        {
            //Gizmos.DrawSphere(transform.position, _radius);
        }
    }

    public IEnumerable< Vector4 > Directions(Vector3 position)
    {
        yield return GetDirection(position, Source.position);

        for (int i = 0; i < _reflectedPositions.Length; i++)
        {
            yield return GetDirection(position, _reflectedPositions[i]); ;
        }
    }

    private Vector4 GetDirection(Vector3 objectPos, Vector3 sourcePos)
    {
        var dir = objectPos - sourcePos;
        var magnitude = dir.magnitude;
        var result = magnitude <= _radius ? (Vector4)dir.normalized * (1 - magnitude / _radius) : Vector4.zero;
        result.w = magnitude * _timeScale;
        return result;
    }
}
