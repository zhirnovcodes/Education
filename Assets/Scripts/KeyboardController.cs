using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardController : MonoBehaviour
{
    [SerializeField] private MetalString _string;
    [SerializeField] private Slider _amplitudeSlider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _string.Hit(_amplitudeSlider.value);
        }
    }
}
