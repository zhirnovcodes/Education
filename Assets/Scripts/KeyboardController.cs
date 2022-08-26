using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    [SerializeField] private Workspace _workspace;

    private float _lastDelayTime;

    private void Start()
    {
        _workspace = GetComponent<Workspace>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StringsHit();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.realtimeSinceStartup - _lastDelayTime <= 0.5f)
            {
                MoleculesReset();
                EarReset();
            }
            else
            {
                StringsReset();
            }

            _lastDelayTime = Time.realtimeSinceStartup;
        }
    }

    private void StringsHit()
    {
        foreach (var str in _workspace.Strings)
        {
            str.Hit();
        }
    }

    private void StringsReset()
    {
        foreach (var str in _workspace.Strings)
        {
            str.Reset();
        }
    }

    private void MoleculesReset()
    {
        foreach (var mol in _workspace.Molecules)
        {
            mol.SetActive(false);
            mol.SetActive(true);
        }
    }

    private void EarReset()
    {
        _workspace.Ear.SetActive(false);
        _workspace.Ear.SetActive(true);
    }
}
