using UnityEngine;

public class KeepBetweenObjects : MonoBehaviour
{
    [SerializeField] private Transform _t1;
    [SerializeField] private Transform _t2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (_t1.position + _t2.position) / 2;
        transform.localScale = new Vector3( (_t2.position - _t1.position).magnitude, transform.localScale.y, transform.localScale.z);
    }
}
