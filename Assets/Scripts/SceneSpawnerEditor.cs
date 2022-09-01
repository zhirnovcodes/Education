using UnityEngine;

public class SceneSpawnerEditor : MonoBehaviour
{
    [SerializeField] private Transform _leftAnchor;
    [SerializeField] private Transform _rightAnchor;
    [SerializeField] private GameObject _molPrefab;
    [SerializeField] private FluctuatingObject1D _first;
    [SerializeField] private float _density = 0.8f;
    [SerializeField] private bool _shoudSpawn = false;

    private GameObject _go;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_leftAnchor == null || _rightAnchor == null || _molPrefab == null || _density <= 0 || !_shoudSpawn)
        {
            return;
        }

        if (_go != null)
        {
            //StartCoroutine(DestroyCor(_go));
        }

        _go = SceneSpawner.SpawnMoleculesChain(_molPrefab, _leftAnchor.position, _rightAnchor.position, _density, _first);
    }
#endif
}