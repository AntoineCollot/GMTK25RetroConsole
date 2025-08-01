using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    static bool isLoading = false;
    static bool isUnloadingGame = false;

    public static bool IsBusy => isLoading || isUnloadingGame;

    const string RETRO_MENU_SCENE_NAME = "RetroMenu";
    const string RETRO_GAME_SCENE_NAME = "RetroGame";
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
        SceneManager.LoadScene(RETRO_GAME_SCENE_NAME, LoadSceneMode.Additive);
    }

    public static void LoadRetroMenu()
    {
        SceneManager.LoadScene(RETRO_MENU_SCENE_NAME, LoadSceneMode.Additive);
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        isLoading = false;

        //Load retro menu on main scene loaded
        if (scene.name == MAIN_SCENE_NAME)
            LoadRetroMenu();
    }

    public static void UnloadRetroGame()
    {
        //make sure the scene is loaded
        if (!SceneManager.GetSceneByName(RETRO_GAME_SCENE_NAME).IsValid())
            return;

        isUnloadingGame = true;

        AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(RETRO_GAME_SCENE_NAME);
        asyncOp.completed += UnloadingCompleted;
    }

    private static void UnloadingCompleted(AsyncOperation obj)
    {
        isUnloadingGame = false;
    }
}
