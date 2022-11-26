using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeKeyboardController : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private float _scale = 1;

    private void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {

            if (Input.mouseScrollDelta.y != 0)
            {
                _source.volume = Mathf.Clamp01( _source.volume + _scale * Input.mouseScrollDelta.y * Time.deltaTime);
            }
        }
    }
}
