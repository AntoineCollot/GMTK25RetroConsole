using System.Collections;
using UnityEngine;

public class RetroConsoleManager : MonoBehaviour
{
    [SerializeField] GameObject consoleInterface;
    [SerializeField] GameObject mainMenuArtwork;
    [SerializeField] RetroMainMenu retroMainMenu;
    [SerializeField] LoadCodeMenu loadCodeMenu;
    [SerializeField] ConsoleLogoAnim consoleLogo;

    const float LOGO_TIME = 2.5f;
    const float MAIN_MENU_ANIM = 1;

    void Start()
    {
        LoadMainMenu();

        RealConsole.Instance.onPoweredStateChanged += OnConsolePoweredChanged;
    }

    private void OnDestroy()
    {
        if(RealConsole.Instance!=null)
        {
            RealConsole.Instance.onPoweredStateChanged -= OnConsolePoweredChanged;
        }
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

    public void CloseAll()
    {
        loadCodeMenu.Close();
        retroMainMenu.Close();
        mainMenuArtwork.SetActive(false);
        consoleLogo.gameObject.SetActive(false);
    }

    public void StartGame(int code)
    {
        if (SceneLoader.IsBusy)
            return;

        SFXManager.PlaySound(GlobalSFX.StartGame);
        RetroGameManager.loadedCode = code;
        SceneLoader.LoadRetroGame();
        CloseInterface();
    }

    public void CloseInterface()
    {
        consoleInterface.SetActive(false);
    }

    public void OpenInterface(bool displayLogo)
    {
        SceneLoader.UnloadRetroGame();

        consoleInterface.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(BootAnim(displayLogo));
    }

    void Shutdown()
    {
        CloseInterface();
        SceneLoader.UnloadRetroGame();
    }

    private void OnConsolePoweredChanged(bool isOn)
    {
        if (isOn)
            OpenInterface(true);
        else
            Shutdown();
    }

    IEnumerator BootAnim(bool displayLogo)
    {
        CloseAll();

        yield return null;

        if (displayLogo)
        {
            consoleLogo.gameObject.SetActive(true);
            yield return new WaitForSeconds(LOGO_TIME);
            consoleLogo.gameObject.SetActive(false);
        }

        mainMenuArtwork.SetActive(true);
        yield return new WaitForSeconds(MAIN_MENU_ANIM);

        LoadMainMenu();
    }
}
