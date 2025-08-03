using UnityEngine;

public class ConsoleCrosspad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateButtonPressed();
    }

    void UpdateButtonPressed()
    {
        Vector2 inputs = RealConsole.Instance.inputs.Gameplay.Move.ReadValue<Vector2>();
        inputs *= -6;
        transform.localEulerAngles = new Vector3(inputs.y, inputs.x, 0);
    }
}
