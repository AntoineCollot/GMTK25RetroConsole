using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;

    InputMap inputs;
    public bool isOn { get; private set; }
    CompositeStateToken freezePlayerToken;

    void Start()
    {
        inputs = new InputMap();
        inputs.Enable();
        inputs.Gameplay.Start.performed += OnStartPerformed;

        freezePlayerToken = new CompositeStateToken();
        PlayerState.Instance.freezeGameplayInputState.Add(freezePlayerToken);

        isOn = false;
        panel.SetActive(isOn);
    }

    private void OnDestroy()
    {
        if (inputs != null)
        {
            inputs.Gameplay.Start.performed -= OnStartPerformed;
            inputs.Gameplay.A.performed -= OnAPerformed;
            inputs.Disable();
            inputs.Dispose();
        }

        if (PlayerState.Instance != null)
        {
            PlayerState.Instance.freezeGameplayInputState.Remove(freezePlayerToken);
        }
    }

    private void OnStartPerformed(InputAction.CallbackContext context)
    {
        if (!RetroGameManager.Instance.GameIsPlaying)
            return;
        ToggleStartMenu();
    }

    public void ToggleStartMenu()
    {
        isOn = !isOn;

        freezePlayerToken.SetOn(isOn);
        panel.SetActive(isOn);
        inputs.Gameplay.Start.performed -= OnAPerformed;
        inputs.Gameplay.Start.performed += OnAPerformed;
    }

    private void OnAPerformed(InputAction.CallbackContext context)
    {
        //Load main menu
    }
}
