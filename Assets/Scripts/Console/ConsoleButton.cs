using UnityEngine;

public class ConsoleButton : MonoBehaviour
{
    public enum ButtonType { A, B, Start }
    [SerializeField] ButtonType buttonType;
    [SerializeField] Transform button;
    [SerializeField] float pressedDistance;

    float pressedAmount;
    const float PRESSED_TRANSITION_TIME = 0.12f;

    // Update is called once per frame
    void Update()
    {
        UpdateButtonPressed();
    }

    void UpdateButtonPressed()
    {
        bool isPressed;
        switch (buttonType)
        {
            case ButtonType.A:
            default:
                isPressed = RealConsole.Instance.inputs.Gameplay.A.IsPressed();
                break;
            case ButtonType.B:
                isPressed = RealConsole.Instance.inputs.Gameplay.B.IsPressed();
                break;
            case ButtonType.Start:
                isPressed = RealConsole.Instance.inputs.Gameplay.Start.IsPressed();
                break;
        }

        if (isPressed)
            pressedAmount += Time.deltaTime / PRESSED_TRANSITION_TIME;
        else
            pressedAmount -= Time.deltaTime / PRESSED_TRANSITION_TIME;

        pressedAmount = Mathf.Clamp01(pressedAmount);

        button.localPosition = -Vector3.forward * Curves.QuadEaseInOut(0, pressedDistance, pressedAmount);
    }
}
