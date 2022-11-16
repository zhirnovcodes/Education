using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;

    private int _index = 0;

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            return;
        }

        if (_instance != this)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var indBefore = _index;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _index = Mathf.Min(_index + 1, SceneManager.sceneCountInBuildSettings - 1);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _index = Mathf.Max(_index - 1, 0);
        }

        if (indBefore != _index) 
        { 
            SceneManager.LoadScene(_index, LoadSceneMode.Single); 
        }
    }
}
