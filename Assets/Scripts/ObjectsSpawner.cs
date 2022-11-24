using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner
{
    private readonly GameObject _baseObject;
    private readonly Queue<GameObject> _despawned;
    private readonly string _prefabName = "";

    public ObjectsSpawner(GameObject baseObject, int capacity = 10)
    {
        _baseObject = baseObject;
        _despawned = new Queue<GameObject>(capacity);
    }

    public ObjectsSpawner(string prefabName, int capacity = 10)
    {
        _prefabName = prefabName;
        _despawned = new Queue<GameObject>(capacity);
    }

    private GameObject InstantiatePrefab()
    {
        if (_baseObject == null)
        {
            Debug.Log(Resources.Load(_prefabName));
            return GameObject.Instantiate(Resources.Load(_prefabName) as GameObject);
        }
        return GameObject.Instantiate(_baseObject);
    }

    public GameObject Spawn(bool shouldSetActive = true)
    {
        var spawned = _despawned.Count <= 0 ? InstantiatePrefab() : _despawned.Dequeue();

        if (shouldSetActive)
        {
            spawned.SetActive(true);
        }

        return spawned;
    }

    public void Despawn(GameObject go)
    {
        go.SetActive(false);
        _despawned.Enqueue(go);
    }
}
