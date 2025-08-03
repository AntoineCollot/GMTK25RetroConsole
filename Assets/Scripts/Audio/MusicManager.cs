using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Theme
{
    MainTheme = 0,
    MainThemeLoop = 1,
    Adventure = 2,
    Combat = 3,
    CombatBoss = 4,
    Cave = 5,
    Magicians = 6,
    FinalArea = 7,
    Dajus = 8,
    FinalBoss = 9,
    Victory = 10,
    Defeat = 11,
    None = 100
}
public class MusicManager : MonoBehaviour
{
    AudioSource source;
    bool isMuted;

    //Have all tracks set up in editor in enum order
    [SerializeField] AudioClip[] musicTracks;
    Queue<Theme> themeQueue;
    Theme currentTheme;
    Theme lastAreaTheme;

    public static MusicManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        source = GetComponent<AudioSource>();
        themeQueue = new Queue<Theme>();
        currentTheme = Theme.None;
        lastAreaTheme = Theme.None;
    }

    private void Update()
    {
        //If we finished playing the main theme, enqueue loop version
        if (currentTheme == Theme.MainTheme && !source.isPlaying)
            EnqueueTheme(Theme.MainThemeLoop);

        if (themeQueue.Count == 0)
            return;

        //If current can be interrupted, cancel it right away
        if (CanBeInterupted(currentTheme))
        {
            PlayNextTheme();
            return;
        }

        //If still have theme to play and we finished the previous one, play next
        if (!source.isPlaying)
        {
            PlayNextTheme();
        }
    }

    public void Mute(bool value)
    {
        isMuted = value;
        source.mute = isMuted;
    }

    public void ToggleMute()
    {
        Mute(!isMuted);
    }

    public void EnqueueTheme(Theme theme)
    {
        if (currentTheme == theme || themeQueue.Contains(theme))
        {
#if UNITY_EDITOR
            Debug.Log($"Theme {theme} couldn't be enqueued as it's already in queue or playing");
#endif
            return;
        }
#if UNITY_EDITOR
        Debug.Log("Enqueuing "+theme.ToString());
#endif

        themeQueue.Enqueue(theme);
    }

    bool IsLoop(Theme theme)
    {
        switch (theme)
        {
            case Theme.MainThemeLoop:
            case Theme.Adventure:
            case Theme.Combat:
            case Theme.CombatBoss:
            case Theme.Cave:
            case Theme.Magicians:
            case Theme.FinalArea:
            case Theme.Dajus:
            case Theme.FinalBoss:
            case Theme.None:
            default:
                return true;
            case Theme.MainTheme:
            case Theme.Victory:
            case Theme.Defeat:
                return false;
        }
    }

    bool CanBeInterupted(Theme theme)
    {
        //Specific setting for main theme, doesn't loop but can be interupted (move to loop version)
        if (theme == Theme.MainTheme)
            return true;

        return IsLoop(theme);
    }

    void PlayNextTheme()
    {
        if (themeQueue.Count == 0)
            return;

        PlayTheme(themeQueue.Dequeue());
    }

    public void PlayTheme(Theme theme)
    {
        if (theme == Theme.None)
        {
            source.clip = null;
            source.Stop();
            return;
        }

        currentTheme = theme;
        source.loop = IsLoop(theme);
        source.clip = musicTracks[(int)theme];
        if (!source.isPlaying)
            source.Play();

        if (IsAreaTheme(theme))
            lastAreaTheme = theme;
    }

    public void ClearLastAreaTheme()
    {
        lastAreaTheme = Theme.None;
    }

    public void EnqueueLastAreaTheme()
    {
        if (lastAreaTheme == Theme.None)
            return;
        EnqueueTheme(lastAreaTheme);
    }

    bool IsAreaTheme(Theme theme)
    {
        switch (theme)
        {
            case Theme.Adventure:
            case Theme.Cave:
            case Theme.FinalArea:
            default:
                return true;
            case Theme.Combat:
            case Theme.CombatBoss:
            case Theme.Magicians:
            case Theme.Dajus:
            case Theme.FinalBoss:
            case Theme.MainThemeLoop:
            case Theme.MainTheme:
            case Theme.Victory:
            case Theme.Defeat:
            case Theme.None:
                return false;
        }
    }

    public void Stop()
    {
        source.Stop();
        themeQueue.Clear();
        currentTheme = Theme.None;
        lastAreaTheme = Theme.None;
    }
}
