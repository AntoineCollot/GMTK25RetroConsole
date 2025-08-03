using UnityEngine;

public class TutoGlitch : MonoBehaviour
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
    }

    // Update is called once per frame
    void Update()
    {
        if(GlitchManager.Instance != null)
        {
            if (GlitchManager.Instance.HasCrashed)
                Show(true);
        }

        if(isDisplayed && !RealConsole.Instance.isOn)
        {
            Show(false);
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
