using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RetroGameManager : MonoBehaviour
{
    public bool gameIsOver { get; private set; }
    public bool gameHasStarted { get; private set; }
    public bool GameIsPlaying => !gameIsOver && gameHasStarted;
    public bool autoStart = true;
    public bool useCodePoint = true;

    public UnityEvent onGameStart = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();
    public UnityEvent onGameWin = new UnityEvent();

    [System.Serializable]
    public class CodeLoadedEvent : UnityEvent<int> { }
    public CodeLoadedEvent onCodeLoaded;

    public static int loadedCode;

    public static RetroGameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (autoStart)
            StartGame(useCodePoint);
    }

    public void StartGame(bool loadCode = true)
    {
        if (gameHasStarted)
            return;

        Theme areaTheme = Theme.Adventure;
        if(loadCode && loadedCode>=0)
        {
            Vector3 spawnPos = CodePointDatabase.Instance.GetSpawnForCode(loadedCode, out areaTheme);
            PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
            player.TeleportAtPos(GameGrid.WordPosToGrid(spawnPos));
            onCodeLoaded.Invoke(loadedCode);
        }
        MusicManager.Instance.EnqueueTheme(areaTheme);

        gameHasStarted = true;
        onGameStart.Invoke();

        if (RetroConsoleManager.Instance.devModeEnabled)
            DialoguePanel.Instance.DisplayLines("Dev Mode Enabled!");
    }

    public void GameOver()
    {
        if (gameIsOver)
            return;
        gameIsOver = true;
        onGameOver.Invoke();

        MusicManager.Instance.EnqueueTheme(Theme.Defeat);
    }

    public void WinGame()
    {
        if (gameIsOver)
            return;

        gameIsOver = true;
        onGameWin.Invoke();
    }

    public void Crash()
    {
        gameIsOver = true;
    }
}
