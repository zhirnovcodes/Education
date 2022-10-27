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
}

public class ReverbZone : MonoBehaviour
{
    [SerializeField] private Line[] _walls;
    [SerializeField] private float _radius = 1;
    [SerializeField] private float _timeScale = 1;

    public static ReverbZone Instance { get; private set; }

    public Line[] Walls => _walls;

    public float Radius { get => _radius; set { _radius = value; } }

    private Vector3[] _reflectedPositions = new Vector3[0];

    private void Awake()
    {
        Instance = this;
        _reflectedPositions = _walls.Select(w => MathIZ.ReflectedPosition(transform.position, w)).Where(w => w.HasValue).Select(w => w.Value).ToArray();
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
        yield return GetDirection(position, transform.position);

        for (int i = 0; i < _reflectedPositions.Length; i++)
        {
            yield return GetDirection(position, _reflectedPositions[i]);
        }
    }

    private Vector4 GetDirection(Vector3 objectPos, Vector3 sourcePos)
    {
        var p2s = objectPos - sourcePos;
        var magnitude = p2s.magnitude;
        var result = magnitude <= _radius ? (Vector4)p2s.normalized * magnitude / _radius + new Vector4(0, 0, 0, magnitude * _timeScale) : Vector4.zero;
        return result;
    }
}
