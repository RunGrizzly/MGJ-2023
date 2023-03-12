using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        LoadMainMenu();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
        }
        else if (scene.name == "BattleScene")
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("BattleScene"));
        }
    }

    public void LoadMainMenu()
    {
        UnloadScenes(new List<string> { "UI", "BattleScene", "Camera" });
        LoadScenes(new List<string> { "MainMenu" });
    }
    public void LoadBattleScene()
    {
        UnloadScenes(new List<string> { "MainMenu" });
        LoadScenes(new List<string> { "Camera", "BattleScene", "UI" });
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