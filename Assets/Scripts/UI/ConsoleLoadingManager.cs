using UnityEngine;

public class ConsoleLoadingManager : MonoBehaviour
{
    [SerializeField] GameObject consoleInterface;
    [SerializeField] RetroMainMenu retroMainMenu;
    [SerializeField] LoadCodeMenu loadCodeMenu;

    void Start()
    {
        LoadMainMenu();
    }

    void Update()
    {
        
    }

    public void LoadMainMenu()
    {
        retroMainMenu.Open();
        loadCodeMenu.Close();
    }

    public void LoadCodeMenu()
    {
        loadCodeMenu.Open();
        retroMainMenu.Close();
    }

    public void StartGame(int code)
    {
        GameManager.Instance.StartGame(code);
    }

    public void CloseInterface()
    {
        consoleInterface.SetActive(false);
    }

    public void OpenInterface()
    {
        consoleInterface.SetActive(true);
        LoadMainMenu();
    }
}
