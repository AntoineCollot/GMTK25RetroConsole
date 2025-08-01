using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public bool gameIsOver { get; private set; }
    public bool gameHasStarted { get; private set; }
    public bool GameIsPlaying => !gameIsOver && gameHasStarted;
    public bool autoStart = true;

    public UnityEvent onGameStart = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();
    public UnityEvent onGameWin = new UnityEvent();

    public static GameManager Instance;

    [Header("CodePoints")]
    public List<CodePoint> codePoints;

    void Awake()
    {
        Instance = this;
        if (autoStart)
            StartGame(0);
    }

    public void StartGame(int code)
    {
        Vector3 spawnPos = codePoints[0].target.position;
        if (codePoints.Any(c => c.code == code))
        {
            CodePoint selectedPoint = codePoints.First(c => c.code == code);
            spawnPos = selectedPoint.target.position;
        }

        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        player.TeleportAtPos(GameGrid.WordPosToGrid(spawnPos));

        if (gameHasStarted)
            return;
        gameHasStarted = true;
        onGameStart.Invoke();

        SFXManager.PlaySound(GlobalSFX.StartGame);
    }

    public void GameOver()
    {
        if (gameIsOver)
            return;
        gameIsOver = true;
        onGameOver.Invoke();
    }

    public void ClearLevel()
    {
        if (gameIsOver)
            return;

        gameIsOver = true;
        onGameWin.Invoke();
    }

    public bool HasCode(int code)
    {
        return codePoints.Any(c => c.code == code);
    }
}

[System.Serializable]
public struct CodePoint
{
    public int code;
    public Transform target;
}