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

    public bool devModeEnabled { get; private set; }

    [Header("DEBUG")]
    [SerializeField] int debugCode;
    [SerializeField] bool debugAutoStart;
    [SerializeField] bool debugEnableDevMode;

    public static RetroConsoleManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RealConsole.Instance.onPoweredStateChanged += OnConsolePoweredChanged;
        CloseInterface();

#if UNITY_EDITOR
        DebugAutoStart();
#endif
    }

    private void OnDestroy()
    {
        if (RealConsole.Instance != null)
        {
            RealConsole.Instance.onPoweredStateChanged -= OnConsolePoweredChanged;
        }
    }

    void Update()
    {

    }

#if UNITY_EDITOR
    void DebugAutoStart()
    {
        if (debugAutoStart)
        {
            RetroGameManager.loadedCode = debugCode;
            SceneLoader.LoadRetroGame();
            CloseInterface();
            if (debugEnableDevMode)
                devModeEnabled = true;
        }
    }
#endif

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
        devModeEnabled = false;
        MusicManager.Instance.Stop();
    }

    private void OnConsolePoweredChanged(bool isOn)
    {
#if UNITY_EDITOR
        if (debugAutoStart && Time.time < 1)
            return;
#endif

        if (isOn)
            OpenInterface(true);
        else
            Shutdown();
    }

    IEnumerator BootAnim(bool displayLogo)
    {
        CloseAll();
        devModeEnabled = false;

        yield return null;

        if (displayLogo)
        {
            consoleLogo.gameObject.SetActive(true);
            yield return new WaitForSeconds(LOGO_TIME);
            consoleLogo.gameObject.SetActive(false);
        }

        //Dev mode
        if (MenuInputs.DevModePressed)
        {
            devModeEnabled = true;
            SFXManager.PlaySound(GlobalSFX.DevMode);
        }

        MusicManager.Instance.EnqueueTheme(Theme.MainTheme);
        mainMenuArtwork.SetActive(true);
        yield return new WaitForSeconds(MAIN_MENU_ANIM);

        LoadMainMenu();
    }
}
