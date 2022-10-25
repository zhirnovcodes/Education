using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoleculesSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 _planeSize;
    [SerializeField, Range(0, 1f)] private float _density = 0.5f;
    [SerializeField, Range(0, 1f)] private float _randomizePosition = 0f;
    [SerializeField, Range(0, 1f)] private float _randomizeScale = 0f;
    [SerializeField] private float _moleculeDiameter = 1;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _parent;
    [SerializeField] private FunctionBuffer _buffer;
    [SerializeField] private GraphDrawerBase _drawer;
    [SerializeField] private bool _shouldDrawGizmos = true;

    private ObjectsSpawner _spawner;
    private List<Molecule1D> _spawned = new List<Molecule1D>();

    public float Density { get => _density; set { _density = value; } }

    public void Spawn()
    {
        if (_spawner == null)
        {
            if (_prefab == null)
            {
                return;
            }
            _spawner = new ObjectsSpawner(_prefab);
        }

        var maxDelay = Mathf.Lerp(3, 0.2f, _density);
        var minPower = Mathf.Lerp(0.1f, 1, _density);
        Molecule1D last = null;

        foreach (var pos in SpawnPositions())
        {
            var spawned = _spawner.Spawn(false);
            spawned.transform.parent = _parent;
            spawned.transform.position = pos.Position;
            spawned.transform.localScale = pos.Scale;

            var molecule = spawned.GetComponent<Molecule1D>();
            var factor = pos.Uv.x;
            var delay = Mathf.Lerp(0.1f, maxDelay, factor);
            var power = Mathf.Lerp(1, minPower, factor);
            molecule.Delay = delay;
            molecule.Power = power;
            molecule.Buffer = _buffer;

            last = molecule;

            _spawned.Add(molecule);

            spawned.SetActive(true);
        }

        if (last != null && _drawer != null)
        {
            _drawer.Function = last;
        }
    }

    public void Clear()
    {
        if (_spawner == null)
        {
            return;
        }
        _drawer.Function = null;

        while (_spawned.Count > 0)
        {
            var index = _spawned.Count() - 1;
            _spawner.Despawn(_spawned[index].gameObject);
            _spawned.RemoveAt(index);
        }
    }

    private void OnValidate()
    {
        //Spawn();
    }

    private IEnumerable<MoleculePosition> SpawnPositions()
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

        foreach (var pos in SpawnPositions())
        {
            Gizmos.DrawSphere(pos.Position, _moleculeDiameter * pos.Scale.x / 2);
        }
    }

    private struct MoleculePosition
    {
        public Vector3 Position;
        public Vector2 Uv;
        public Vector3 Scale;
    }
}
