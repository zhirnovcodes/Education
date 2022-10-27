using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct MoleculePosition
{
    public Vector3 Position;
    public Vector2 Uv;
    public Vector3 Scale;
}

public class MoleculesGrid : MonoBehaviour
{
    [SerializeField] private Vector2 _planeSize;
    [SerializeField, Range(0, 1f)] private float _density = 0.5f;
    [SerializeField, Range(0, 1f)] private float _randomizePosition = 0f;
    [SerializeField, Range(0, 1f)] private float _randomizeScale = 0f;
    [SerializeField] private float _moleculeDiameter = 1;
    [SerializeField] private bool _shouldDrawGizmos = true;

    public float Density { get => _density; set { _density = value; } }

    public IEnumerable<MoleculePosition> Positions()
    {
        var v1 = transform.position - (Vector3)_planeSize / 2;
        var moleculeSize = new Vector2(Mathf.Lerp(_planeSize.x / 2, _moleculeDiameter, _density), _moleculeDiameter);
        var count = new Vector2Int(Mathf.FloorToInt(_planeSize.x / moleculeSize.x), Mathf.FloorToInt(_planeSize.y / moleculeSize.y));
        var sizeXToUv = _moleculeDiameter / _planeSize.x;

        for (int x = 0; x < count.x; x++)
        {
            for (int y = 0; y < count.y; y++)
            {
                var xOffset = Mathf.Lerp(0.5f, Random.Range(sizeXToUv, 1 - sizeXToUv), _randomizePosition);
                var uv = new Vector2((x + xOffset) / count.x, (y + 0.5f) / count.y);
                var pos = new Vector3(_planeSize.x * uv.x, _planeSize.y * uv.y, 0);

                pos += v1;

                var size = Random.Range(0.1f, 1);
                var scale = Vector3.Lerp(Vector3.one, new Vector3(size, size, 0), _randomizeScale);

                yield return new MoleculePosition() { Position = pos, Uv = uv, Scale = scale };
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!_shouldDrawGizmos)
        {
            return;
        }
        var v1 = transform.position - (Vector3)_planeSize / 2;
        var v2 = v1 + Vector3.up * _planeSize.y;
        var v3 = transform.position + (Vector3)_planeSize / 2;
        var v4 = v3 + Vector3.down * _planeSize.y;

        Gizmos.DrawLine(v1, v2);
        Gizmos.DrawLine(v2, v3);
        Gizmos.DrawLine(v3, v4);
        Gizmos.DrawLine(v4, v1);

        foreach (var pos in Positions())
        {
            Gizmos.DrawSphere(pos.Position, _moleculeDiameter * pos.Scale.x / 2);
        }
    }

}
