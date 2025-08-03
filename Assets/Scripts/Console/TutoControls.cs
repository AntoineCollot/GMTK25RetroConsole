using UnityEngine;

public class TutoControls : MonoBehaviour
{
    [SerializeField] GameObject tuto;
    bool isDisplayed;
    bool hasBeenSeen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isDisplayed = false;
        hasBeenSeen = false;
        tuto.SetActive(false);

        Invoke("ShowDelayed", 2);
    }

    void ShowDelayed()
    {
        Show(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (RetroGameManager.Instance != null)
        {
            if (isDisplayed && RetroGameManager.Instance.gameHasStarted)
            {
                Show(false);
            }
        }
    }

    void Show(bool value)
    {
        if (isDisplayed == value)
            return;

        isDisplayed = value;
        tuto.SetActive(value);

        if (hasBeenSeen && !value)
            enabled = false;
    }
}
