using System;
using TMPro;
using UnityEngine;
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
            if (CodePointDatabase.Instance.HasCode(candidateCode))
            {
                console.StartGame(candidateCode);
            }
            else
            {
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
            intCode += code[i] * Mathf.RoundToInt(Mathf.Pow(10, i));
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
