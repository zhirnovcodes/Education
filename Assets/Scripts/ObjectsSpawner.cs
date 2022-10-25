using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSpawner
{
    private readonly GameObject _baseObject;
    private readonly Queue<GameObject> _despawned;

    public ObjectsSpawner(GameObject baseObject, int capacity = 10)
    {
        _baseObject = baseObject;
        _despawned = new Queue<GameObject>(capacity);
    }

    public GameObject Spawn(bool shouldSetActive = true)
    {
        var spawned = _despawned.Count <= 0 ? GameObject.Instantiate(_baseObject) : _despawned.Dequeue();

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
