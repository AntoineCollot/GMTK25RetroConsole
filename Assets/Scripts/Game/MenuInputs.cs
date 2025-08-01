using System;
using UnityEngine;

public static class MenuInputs
{
    static InputMap inputMap;

    public static bool AIsPressed => inputMap.Gameplay.A.IsPressed();
    public static bool BIsPressed => inputMap.Gameplay.B.IsPressed();
    public static Action OnAPressed;
    public static Action OnBPressed;
    public static Vector2 Cross => inputMap.Gameplay.Move.ReadValue<Vector2>();

    static MenuInputs()
    {
    }

    /// <summary>
    /// Use this to init instead of constructor so that it's called everytime we hit play in uniter editor (constructor only called when domain reloaded)
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
#if UNITY_EDITOR
        //Clear anything remaining from previous play
        if (inputMap != null)
        {
            inputMap.Gameplay.A.performed -= OnAPerformed;
            inputMap.Gameplay.B.performed -= OnBPerformed;
            inputMap.Dispose();
        }
#endif

        inputMap = new InputMap();
        inputMap.Enable();
        inputMap.Gameplay.A.performed += OnAPerformed;
        inputMap.Gameplay.B.performed += OnBPerformed;
    }

    private static void OnAPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAPressed?.Invoke();
    }

    private static void OnBPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnBPressed?.Invoke();
    }
}