using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public abstract void Clear();
    public abstract void Spawn();
}

public abstract class MoleculesSpawner<T> : Spawner where T : MonoBehaviour
{

    [SerializeField] protected MoleculesGrid _grid;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private string _prefabName;
    [SerializeField] private Transform _parent;

    private ObjectsSpawner _spawner;
    private List<T> _spawned = new List<T>();

    public IEnumerable<T> Spawned => _spawned;

    public override void Spawn()
    {
        if (_spawner == null)
        {
            if (_prefab == null)
            {
                _spawner = new ObjectsSpawner(_prefabName);
            }
            else
            {
                _spawner = new ObjectsSpawner(_prefab);
            }
        }

        var maxDelay = Mathf.Lerp(3, 0.2f, _grid.Density);
        var minPower = Mathf.Lerp(0.1f, 1, _grid.Density);
        T last = null;

        foreach (var pos in _grid.Positions())
        {
            var spawned = _spawner.Spawn(false);
            var spawnedScale = spawned.transform.localScale;
            var newScale = new Vector3(pos.Scale.x * spawnedScale.x, pos.Scale.y * spawnedScale.y, 1);
            spawned.transform.parent = _parent;
            spawned.transform.position = pos.Position;
            spawned.transform.localScale = newScale;

            var molecule = spawned.GetComponent<T>();
            SetupComponent(molecule, pos);
            last = molecule;

            _spawned.Add(molecule);

            spawned.SetActive(true);
        }
    }

    protected abstract void SetupComponent(T component, MoleculePosition pos);

    public override void Clear()
    {
        if (_spawner == null)
        {
            return;
        }

        while (_spawned.Count > 0)
        {
            var index = _spawned.Count() - 1;
            _spawner.Despawn(_spawned[index].gameObject);
            _spawned.RemoveAt(index);
        }
    }

    private void OnValidate()
    {
        _spawner = null;
    }
}
