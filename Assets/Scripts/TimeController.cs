using UnityEngine;

public class TimeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
