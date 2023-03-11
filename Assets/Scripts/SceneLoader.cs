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
        UnloadScenes(new List<string> { "UI", "LoopTest", "Camera" });
        LoadScenes(new List<string> { "MainMenu" });
    }
    public void LoadBattleScene()
    {
        UnloadScenes(new List<string> { "MainMenu" });
        LoadScenes(new List<string> { "Camera", "LoopTest", "UI" });
    }

    void LoadScenes(List<string> scenes)
    {
        foreach (string name in scenes)
        {
            if (!SceneManager.GetSceneByName(name).isLoaded)
            {
                SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            }
        }
    }

    void UnloadScenes(List<string> scenes)
    {
        foreach (string name in scenes)
        {
            if (SceneManager.GetSceneByName(name).isLoaded)
            {
                SceneManager.UnloadSceneAsync(name);
            }
        }
    }
}