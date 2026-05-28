using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private static SceneHandler _instance;

    public static SceneHandler Instance => _instance;

    private string _sceneName;

    private void Start()
    {
        if (_instance != null)
        {
            Debug.Log("There already exists an instance of this singleton!");
            return;
        }

        _instance = this;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        Debug.Log("Quitting...");
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }
}