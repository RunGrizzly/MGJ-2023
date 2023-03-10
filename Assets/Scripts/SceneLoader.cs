using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        LoadScenes(new List<string> { "MainMenu" });
    }

    void LoadScenes(List<string> scenes)
    {
        foreach (string name in scenes)
        {
            SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        }
    }

    void UnloadScenes(List<string> scenes)
    {
        foreach (string name in scenes)
        {
            SceneManager.UnloadSceneAsync(name);
        }
    }
}