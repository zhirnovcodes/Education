using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    [SerializeField] private Workspace _workspace;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < _workspace.StringsCount; i++)
            {
                _workspace.StringById(i).Hit();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = 0; i < _workspace.StringsCount; i++)
            {
                Debug.Log(2);
                _workspace.StringById(i).Reset();
            }
        }
    }
}
