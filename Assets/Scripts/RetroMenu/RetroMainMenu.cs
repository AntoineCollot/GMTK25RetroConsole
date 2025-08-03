using System;
using UnityEngine;

public class RetroMainMenu : MonoBehaviour
{
    enum Button { NewGame, LoadCode }
    Button selectedButton;

    [SerializeField] GameObject newGameCursor;
    [SerializeField] GameObject loadCodeCursor;
    [SerializeField] GameObject panel;

    RetroConsoleManager console;

    bool hasUsedDirection;

    private void Awake()
    {
        console = GetComponentInParent<RetroConsoleManager>();
    }

    private void OnEnable()
    {
        MenuInputs.OnAPressed += OnAPerformed;
        Clear();
    }

    private void OnDisable()
    {
        MenuInputs.OnAPressed -= OnAPerformed;
    }

    private void Update()
    {
        Vector2 inputs = MenuInputs.Crosspad;
        Direction inputDir = inputs.ToDirection();

        //wait to return to neutral and big enough input
        if (inputs.magnitude < 0.5f || hasUsedDirection)
        {
            if (inputs.magnitude < 0.2f)
                hasUsedDirection = false;
            return;
        }

        hasUsedDirection = true;
        SwitchSelectedButton();
    }

    private void OnAPerformed()
    {
        switch (selectedButton)
        {
            case Button.NewGame:
                console.StartGame(0);
                break;
            case Button.LoadCode:
                console.LoadCodeMenu();
                SFXManager.PlaySound(GlobalSFX.UIValidate);
                break;
        }
        SFXManager.PlaySound(GlobalSFX.UIValidate);
    }

    private void Clear()
    {
        selectedButton = Button.NewGame;
        UpdateCursor();
        hasUsedDirection = false;
    }

    void SwitchSelectedButton()
    {
        if (selectedButton == Button.NewGame)
            selectedButton = Button.LoadCode;
        else
            selectedButton = Button.NewGame;

        UpdateCursor();

        SFXManager.PlaySound(GlobalSFX.CursorMove);
    }

    void UpdateCursor()
    {
        newGameCursor.SetActive(selectedButton == Button.NewGame);
        loadCodeCursor.SetActive(selectedButton == Button.LoadCode);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        MusicManager.Instance.EnqueueTheme(Theme.MainTheme);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
