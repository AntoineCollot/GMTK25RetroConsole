using System.Collections;
using System.Linq;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    const float DARK_GREY = 46 / 256.0f;
    const float LIGHT_GREY = 176 / 256.0f;
    static readonly Color DARK_GREY_COL = new Color(DARK_GREY, DARK_GREY, DARK_GREY, 1);
    static readonly Color LIGHT_GREY_COL = new Color(LIGHT_GREY, LIGHT_GREY, LIGHT_GREY, 1);
    const string THRESHOLD_PROPERTY = "_Threshold";
    const string ORIGIN_PROPERTY = "_Origin";

    bool hasBeenTriggered;
    bool hasFinished;
    bool hasBeenCanceled;

    [Header("Settings")]
    [SerializeField] Vector2Int[] triggerCells;

    [Header("Components")]
    [SerializeField] SpriteRenderer background;
    [SerializeField] CharacterAnimations dajus;
    [SerializeField] GameObject[] mages;
    [SerializeField] GameObject hitPrefab;
    [SerializeField] GameObject smokePrefab;
    [SerializeField] GameObject demon;
    [SerializeField] GameObject ritualScene;
    [SerializeField] GameObject destroyedScene;
    [SerializeField] CharacterAnimations mainMage;
    [SerializeField] GameObject idleGirl;

    CompositeStateToken freezePlayerToken;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        freezePlayerToken = new CompositeStateToken();
        hasBeenCanceled = false;

        if (RetroGameManager.loadedCode == 0)
            StartCoroutine(StartAnim());
        else
            Cancel();
    }

    private void Update()
    {
        if (GlitchManager.Instance.HasCrashed)
            Cancel();
    }

    private void OnDestroy()
    {
        if (PlayerState.Instance != null)
        {
            PlayerState.Instance.freezeGameplayInputState.Remove(freezePlayerToken);
            PlayerState.Instance.onPlayerPositionChanged -= OnPlayerPos;
        }
    }

    public void Cancel()
    {
        hasBeenCanceled = true;
        StopAllCoroutines();
        if (!hasFinished)
        {
            ritualScene.SetActive(false);
            destroyedScene.SetActive(false);
            idleGirl.SetActive(true);
            PlayerState.Instance.onPlayerPositionChanged -= OnPlayerPos;
        }
    }

    IEnumerator StartAnim()
    {
        ritualScene.SetActive(true);
        destroyedScene.SetActive(false);
        demon.SetActive(false);
        CharacterAnimations playerAnimations = PlayerState.Instance.GetComponentInChildren<CharacterAnimations>();
        PlayerState.Instance.freezeGameplayInputState.Add(freezePlayerToken);
        freezePlayerToken.SetOn(true);
        hasBeenTriggered = false;
        hasFinished = false;
        idleGirl.SetActive(false);
        background.gameObject.SetActive(true);
        playerAnimations.SetSortingLayerAsDuel();

        //White screen
        background.color = Color.white;
        background.sharedMaterial.SetFloat(THRESHOLD_PROPERTY, 1);

        yield return new WaitForSeconds(2);
        yield return StartCoroutine(FadeScreen(1));
        playerAnimations.ResetSortingLayer();
        //MusicManager.Instance.EnqueueTheme(Theme.Magicians);

        //Dialogue girl
        yield return new WaitForSeconds(1);
        DialoguePanel.Instance.DisplayLines("Finaly your are awake!",
            "Hurry, the other mages are waiting for you!");

        //Wait for entering trigger cells
        freezePlayerToken.SetOn(false);
        PlayerState.Instance.onPlayerPositionChanged += OnPlayerPos;
        while (RetroGameManager.Instance.GameIsPlaying)
        {
            yield return null;
            if (hasBeenTriggered)
                break;
        }

        //Intro dialogue
        freezePlayerToken.SetOn(true);
        mainMage.LookDirection(Direction.Down);
        yield return new WaitForSeconds(0.25f);

        DialoguePanel.Instance.DisplayLines("Ah, we are all here. The ritual can begin!",
            "Every 10 years, the Demon awakens",
            "We must banish him with our combined powers",
            "This cycle has been going for ages to keep the world at peace",
            "...He is coming, get rea...WHAT ?");
        //Wait dialogue
        while (DialoguePanel.Instance.isOpen)
        {
            yield return null;
        }

        MusicManager.Instance.EnqueueTheme(Theme.Dajus);

        yield return StartCoroutine(DajusAttackMage(0));

        DialoguePanel.Instance.DisplayLines("Dajus, what are you doing ?! TRAITOR!");
        //Wait dialogue
        while (DialoguePanel.Instance.isOpen)
        {
            yield return null;
        }

        //Anim kills
        for (int i = 1; i < mages.Length; i++)
        {
            yield return StartCoroutine(DajusAttackMage(i));
        }

        yield return new WaitForSeconds(0.5f);
        ScreenShakeSimple.Instance.Shake(1);
        yield return new WaitForSeconds(0.5f);

        //Spawn demon
        SFXManager.PlaySound(GlobalSFX.DemonLaugh);
        Instantiate(hitPrefab, demon.transform.position, Quaternion.identity, null);
        demon.SetActive(true);
        MusicManager.Instance.EnqueueTheme(Theme.FinalBoss);

        yield return new WaitForSeconds(1.5f);

        DialoguePanel.Instance.DisplayLines("RAAAAH FINALLY I'M AWAKE! THE WORLD IS MINE!");
        //Wait dialogue
        while (DialoguePanel.Instance.isOpen)
        {
            yield return null;
        }

        ScreenShakeSimple.Instance.Shake(1);
        SFXManager.PlaySound(GlobalSFX.MiscHit);
        yield return new WaitForSeconds(0.2f);

        //Black screen
        MusicManager.Instance.EnqueueTheme(Theme.None);
        background.gameObject.SetActive(true);
        background.color = Color.black;
        background.sharedMaterial.SetFloat(THRESHOLD_PROPERTY, 1);
        ritualScene.SetActive(false);
        destroyedScene.SetActive(true);
        yield return new WaitForSeconds(2.5f);

        yield return StartCoroutine(FadeColor());
        yield return StartCoroutine(FadeScreen(1));

        MusicManager.Instance.EnqueueTheme(Theme.Adventure);
        freezePlayerToken.SetOn(false);
        PlayerState.Instance.freezeGameplayInputState.Remove(freezePlayerToken);

        //End dialogue
        DialoguePanel.Instance.DisplayLines("Thanks god you are alive!",
            "Dajus And the Demon killed everyone!",
            "You must find and destroy them!");

        hasFinished = true;
    }

    private void OnPlayerPos(Vector2Int playerPos)
    {
        if (hasBeenTriggered || !RetroGameManager.Instance.GameIsPlaying || hasBeenCanceled)
            return;
        if (triggerCells.Contains(playerPos))
        {
            hasBeenTriggered = true;
            PlayerState.Instance.onPlayerPositionChanged -= OnPlayerPos;
        }
    }

    IEnumerator FadeColor()
    {
        const float FADE_INTERVAL = 0.3f;
        background.color = DARK_GREY_COL;
        yield return new WaitForSeconds(FADE_INTERVAL);
        background.color = LIGHT_GREY_COL;
        yield return new WaitForSeconds(FADE_INTERVAL);
        background.color = Color.white;
    }

    IEnumerator FadeScreen(float time)
    {
        background.sharedMaterial.SetVector(ORIGIN_PROPERTY, PlayerState.Instance.transform.position);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            background.sharedMaterial.SetFloat(THRESHOLD_PROPERTY, 1 - t);

            yield return null;
        }

        background.gameObject.SetActive(false);
    }

    IEnumerator DajusAttackMage(int id)
    {
        Direction dir = (mages[id].transform.position - dajus.transform.position).ToDirection();
        dajus.moveable.LookDirection(dir);
        yield return new WaitForSeconds(0.3f);
        dajus.Attack();
        SFXManager.PlaySound(GlobalSFX.AttackEnemy);
        ScreenShakeSimple.Instance.Shake(0.25f);
        yield return new WaitForSeconds(0.6f);

        KillMage(id);
        yield return new WaitForSeconds(0.3f);
    }

    void KillMage(int id)
    {
        mages[id].SetActive(false);
        Vector3 pos = mages[id].transform.position;
        Instantiate(smokePrefab, pos, Quaternion.identity, null);
        Instantiate(hitPrefab, pos, Quaternion.identity, null);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (triggerCells == null)
            return;
        for (int i = 0; i < triggerCells.Length; i++)
        {
            Gizmos.DrawWireCube(GameGrid.GridToWorldPos(triggerCells[i]), Vector2.one);
        }
    }
#endif
}
