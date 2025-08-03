using UnityEngine;

public class TutoCartridge : MonoBehaviour
{
    [SerializeField] GameObject tuto;
    bool isDisplayed;

    float timeOff;
    const float TIME_SHOWING = 1;

    bool hasConsoleBeenOnOnce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasConsoleBeenOnOnce = false;
        isDisplayed = false;
        tuto.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (RealConsole.Instance != null)
        {
            if (!RealConsole.Instance.isOn && !isDisplayed && hasConsoleBeenOnOnce)
            {
                timeOff += Time.deltaTime;
                if (timeOff > TIME_SHOWING)
                {
                    Show(true);
                }
            }
            else
                timeOff = 0;

            if (RealConsole.Instance.isOn)
                hasConsoleBeenOnOnce = true;
        }

        if (isDisplayed && (RealConsole.Instance.isOn || RealConsole.Instance.isCartridgeOut))
        {
            Show(false);
            if (RealConsole.Instance.isCartridgeOut)
                enabled = false;
        }
    }

    void Show(bool value)
    {
        if (isDisplayed == value)
            return;

        isDisplayed = value;
        tuto.SetActive(value);
    }
}
