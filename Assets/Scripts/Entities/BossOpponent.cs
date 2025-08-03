using UnityEngine;
using System.Collections;
using UnityEngine.Events;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class BossOpponent : Opponent, IDualable
{
    [SerializeField] Theme battleTheme;
    [SerializeField, TextArea()] string[] dialogue;
    [SerializeField] float overrideCharacterTime = 0.25f;

    public UnityEvent onDefeated = new();

    public override void OnPlayerDetected(Vector2Int playerPosition)
    {
        PlayerState.Instance.freezeGameplayInputState.Add(freezePlayerToken);
        freezePlayerToken.SetOn(true);
        detectFX.SetActive(true);

        //Music
        MusicManager.Instance.EnqueueTheme(battleTheme);

        StartCoroutine(PreDuelAnim());
    }

    IEnumerator PreDuelAnim()
    {
        yield return new WaitForSeconds(detectFreezeTime);

        DialoguePanel.Instance.DisplayLinesWithCharacterTime(dialogue, overrideCharacterTime);

        //Wait for dialogue
        while (DialoguePanel.Instance.isOpen)
            yield return null;

        StartDuel();
    }

    public override void Die()
    {
        base.Die();

        onDefeated.Invoke();
    }
}