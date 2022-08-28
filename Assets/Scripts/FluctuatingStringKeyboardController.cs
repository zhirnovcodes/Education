using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluctuatingStringKeyboardController : MonoBehaviour
{
    [SerializeField] private FluctuatingString1D _str;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _str.TimeStart = Air.Time;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _str.TimeStart = 0;
        }
    }
}
