using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSceneOnSpace : MonoBehaviour {

    public string scene;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GoToScene();
        }
    }

    void GoToScene()
    {
        SceneManager.LoadScene(scene);
    }
}
