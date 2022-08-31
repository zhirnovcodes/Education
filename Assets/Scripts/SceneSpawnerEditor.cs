using UnityEngine;

public class SceneSpawnerEditor : MonoBehaviour
{
    [SerializeField] private Transform _leftAnchor;
    [SerializeField] private Transform _rightAnchor;
    [SerializeField] private GameObject _molPrefab;
    [SerializeField] private FluctuatingObject1D _first;
    [SerializeField] private int _count;
    [SerializeField] private bool _shoudSpawn = false;

    private GameObject _go;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_leftAnchor == null || _rightAnchor == null || _molPrefab == null || _count <= 0 || !_shoudSpawn)
        {
            return;
        }

        if (_go != null)
        {
            //StartCoroutine(DestroyCor(_go));
        }

        _go = SceneSpawner.SpawnMoleculesChain(_molPrefab, _leftAnchor.position, _rightAnchor.position, _count, _first);
    }
#endif
}