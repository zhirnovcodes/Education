using UnityEngine;

public class TimeController : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown( KeyCode.P))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale = Time.timeScale == 2 ? 1 : 2;
        }
    }
}
