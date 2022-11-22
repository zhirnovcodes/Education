using UnityEngine;

public class TimeController : MonoBehaviour
{
    private static TimeController _instance;

    [SerializeField, Range(0,3)] private float _scale = 1;

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            return;
        }

        if (!_instance.Equals(this))
        {
            Destroy(this);
        }
        Time.timeScale = _scale;
    }

    private void OnValidate()
    {
        Time.timeScale = _scale;
    }

    void Update()
    {

        if (Input.GetKeyDown( KeyCode.P))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            return;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale = Time.timeScale == 2 ? 1 : 2;
            return;
        }
    }
}
