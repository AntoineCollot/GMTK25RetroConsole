using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LoadCodeMenu : MonoBehaviour
{
    [Header("Code")]
    [SerializeField] TextMeshProUGUI codeTextInput;
    int[] code;
    const int CODE_LENGTH = 4;
    bool IsCodeCompleted => code[CODE_LENGTH - 1] >= 0;
    bool HasAnyDigit => code[0] >= 0;

    [Header("Circle")]
    [SerializeField] Image circleImage;
    [SerializeField] Sprite[] selectedSprites;
    [SerializeField] Sprite completedCodeSprite;
    Sprite idleSprite;

    int selectedValue;
    RetroConsoleManager console;
    int lastTryAddedDigitFrame;

    void Awake()
    {
        idleSprite = circleImage.sprite;
        code = new int[CODE_LENGTH];
        console = GetComponentInParent<RetroConsoleManager>();

    }

    private void OnEnable()
    {
        MenuInputs.OnAPressed += OnAPressed;
        MenuInputs.OnBPressed += OnBPressed;

        ClearCode();
    }

    private void OnDisable()
    {
        MenuInputs.OnAPressed -= OnAPressed;
        MenuInputs.OnBPressed -= OnBPressed;
        MenuInputs.OnBPressed -= OnBPressed;
    }

    private void Update()
    {
        if (Keyboard.current.digit0Key.isPressed || Keyboard.current.numpad0Key.isPressed)
        {
            AddNextDigit(0);
            return;
        }
        if (Keyboard.current.digit1Key.isPressed || Keyboard.current.numpad1Key.isPressed)
        {
            AddNextDigit(1);
            return;
        }
        if (Keyboard.current.digit2Key.isPressed || Keyboard.current.numpad2Key.isPressed)
        {
            AddNextDigit(2);
            return;
        }
        if (Keyboard.current.digit3Key.isPressed || Keyboard.current.numpad3Key.isPressed)
        {
            AddNextDigit(3);
            return;
        }
        if (Keyboard.current.digit4Key.isPressed || Keyboard.current.numpad4Key.isPressed)
        {
            AddNextDigit(4);
            return;
        }
        if (Keyboard.current.digit5Key.isPressed || Keyboard.current.numpad5Key.isPressed)
        {
            AddNextDigit(5);
            return;
        }

        Vector2 inputs = MenuInputs.Crosspad;
        if (!IsCodeCompleted && inputs.magnitude > 0.5f)
        {
            float angle = GetVectorAngle(inputs);
            int value = GetValueForAngle(angle);
            Select(value);
        }
        else
        {
            Deselect();
        }
    }

    void ClearCode()
    {
        for (int i = 0; i < CODE_LENGTH; i++)
        {
            code[i] = -1;
        }
        selectedValue = -1;
        UpdateCodeText();
    }

    private void OnAPressed()
    {
        if (IsCodeCompleted)
        {
            //Load level
            int candidateCode = GetCodeAsInt();
            Debug.Log("Loading Code " + candidateCode);
            if (CodePointDatabase.Instance.HasCode(candidateCode))
            {
                Debug.Log("Code found, starting...");
                console.StartGame(candidateCode);
            }
            else
            {
                Debug.Log("Code not found...");
                console.LoadMainMenu();
                SFXManager.PlaySound(GlobalSFX.UICancel);
            }
            return;
        }
        if (selectedValue >= 0)
        {
            AddNextDigit(selectedValue);
        }
    }

    private void OnBPressed()
    {
        if (HasAnyDigit)
            RemoveLastDigit();
        else
            console.LoadMainMenu();
    }

    void AddNextDigit(int value)
    {
        //Do not allow to add digits every frame, or even try to (avoid holding key)
        if (Time.frameCount <= lastTryAddedDigitFrame + 1)
        {
            lastTryAddedDigitFrame = Time.frameCount;
            return;
        }

        for (int i = 0; i < CODE_LENGTH; i++)
        {
            //Find first digit under 0
            if (code[i] < 0)
            {
                code[i] = value;
                break;
            }
        }
        UpdateCodeText();
        lastTryAddedDigitFrame = Time.frameCount;
        SFXManager.PlaySound(GlobalSFX.UIValidate);
    }

    void RemoveLastDigit()
    {
        for (int i = CODE_LENGTH - 1; i >= 0; i--)
        {
            //Find first digit above 0 to remove
            if (code[i] >= 0)
            {
                code[i] = -1;
                break;
            }
        }
        UpdateCodeText();
        SFXManager.PlaySound(GlobalSFX.UICancel);
    }

    float GetVectorAngle(Vector2 v)
    {
        float angle = Vector2.SignedAngle(Vector2.up, v);
        if (angle < 0)
            angle += 360;

        angle = 360 - angle;
        angle += 30;
        angle %= 360;
        return angle;
    }

    int GetValueForAngle(float angle)
    {
        angle /= 360;
        int value = Mathf.Clamp(Mathf.FloorToInt(angle * 6), 0, 6);
        return value;
    }

    void Select(int value)
    {
        circleImage.sprite = selectedSprites[value];
        selectedValue = value;
    }

    void Deselect()
    {
        if (IsCodeCompleted)
            circleImage.sprite = completedCodeSprite;
        else
            circleImage.sprite = idleSprite;
        selectedValue = -1;
    }

    void UpdateCodeText()
    {
        string text = "";
        for (int i = 0; i < CODE_LENGTH; i++)
        {
            if (code[i] >= 0)
                text += code[i];
            else
                text += "_";

            if (i < CODE_LENGTH - 1)
                text += " ";
        }
        codeTextInput.text = text;
    }

    int GetCodeAsInt()
    {
        int intCode = 0;
        for (int i = 0; i < CODE_LENGTH; i++)
        {
            intCode += code[i] * Mathf.RoundToInt(Mathf.Pow(10, (CODE_LENGTH - 1) - i));
        }
        return intCode;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}
