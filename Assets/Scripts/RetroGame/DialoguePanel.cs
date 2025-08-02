using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject panel;
    [SerializeField] GameObject nextIcon;
    TextMeshProUGUI text;
    InputMap inputs;
    CompositeStateToken freezePlayerToken;

    [Header("Settings")]
    [SerializeField] float characterTime;

    Queue<string> linesToDisplay;
    bool isOpen;

    public static DialoguePanel Instance;

    void Awake()
    {
        Instance = this;
        text = GetComponentInChildren<TextMeshProUGUI>(true);
        linesToDisplay = new();
    }

    void Start()
    {
        inputs = new InputMap();
        inputs.Enable();

        freezePlayerToken = new CompositeStateToken();
        PlayerState.Instance.freezeGameplayInputState.Add(freezePlayerToken);

        ForceClose();
    }

    private void Update()
    {
        if(isOpen && !RetroGameManager.Instance.GameIsPlaying)
        {
            Close();
        }
    }

    private void OnDestroy()
    {
        if (inputs != null)
        {
            inputs.Gameplay.A.performed -= OnAPerformed;
            inputs.Disable();
            inputs.Dispose();
        }

        if (PlayerState.Instance != null)
        {
            PlayerState.Instance.freezeGameplayInputState.Remove(freezePlayerToken);
        }
    }

    public void DisplayLines(params string[] lines)
    {
        Open();
        foreach (string line in lines)
        {
            linesToDisplay.Enqueue(line);
        }
        DisplayNext();
    }

    void DisplayNext()
    {
        string line = linesToDisplay.Dequeue();
        StartCoroutine(Typewritting(line));
    }

    IEnumerator Typewritting(string str)
    {
        nextIcon.SetActive(false);
        text.text = str;
        int charCount = str.Length;
        float elapsedTime = 0;
        float nextCharacterTime = characterTime;
        for (int i = 0; i <= charCount; i++)
        {
            text.maxVisibleCharacters = i;
            nextCharacterTime += characterTime;

            while (elapsedTime < nextCharacterTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        ListenToNext();
    }

    void ListenToNext()
    {
        inputs.Gameplay.A.performed -= OnAPerformed;
        inputs.Gameplay.A.performed += OnAPerformed;
        nextIcon.SetActive(true);
    }

    private void OnAPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        inputs.Gameplay.A.performed -= OnAPerformed;
        if (linesToDisplay.Count > 0)
            DisplayNext();
        else
            Close();
    }

    public void Open()
    {
        if (isOpen)
            return;
        isOpen = true;
        panel.SetActive(true);
        freezePlayerToken.SetOn(true);
    }

    void Close()
    {
        if (!isOpen)
            return;
        ForceClose();
    }

    void ForceClose()
    {
        isOpen = false;
        linesToDisplay.Clear();
        panel.SetActive(false);
        freezePlayerToken.SetOn(false);
        PlayerState.Instance.FreezeInputsForCurrentFrame();
    }
}
