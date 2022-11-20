using UnityEngine;

public class GraphKeyboardController : MonoBehaviour
{
    private GraphDrawerBase _drawer;

    void Start()
    {
        _drawer = _drawer ?? GetComponent<GraphDrawerBase>();
        _drawer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _drawer.enabled = !_drawer.enabled;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Start();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                var name = _drawer.gameObject.name;
                var p = _drawer.Texture.SaveRTToFile(name);

                if (p != null)
                {
                    FilesExtensions.ShowInExplorer(p);
                }
            }
        }
    }

}
