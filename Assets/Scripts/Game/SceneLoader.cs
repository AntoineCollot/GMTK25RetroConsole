using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    static bool isLoading = false;

    const string RETRO_SCENE_NAME = "RetroGame";
    const string MAIN_SCENE_NAME = "MainScene";

    static public void LoadMenu(LoadSceneMode mode)
    {
        if (isLoading)
            return;

        isLoading = true;
        SceneManager.LoadScene(1, mode);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static public void LoadRetroGame()
    {
        SceneManager.LoadScene(RETRO_SCENE_NAME, LoadSceneMode.Additive);
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        isLoading = false;

        if (scene.name == MAIN_SCENE_NAME)
            LoadRetroGame();
    }
}
