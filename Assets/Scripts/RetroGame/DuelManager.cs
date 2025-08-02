using System.Collections;
using UnityEngine;

public class DuelManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer duelBackground;
    [SerializeField] GameObject smokePrefab;

    public bool isInDuel { get; private set; }
    public Opponent currentOpponent { get; private set; }
    PlayerMovement playerMovement;
    CharacterAnimations playerAnimations;

    public static DuelManager Instance;

    const float BACKGROUND_GROW_TIME = 1.5f;
    const float ATTACK_INTERVAL_TIME = 1.5f;

    private void Awake()
    {
        Instance = this;
        duelBackground.gameObject.SetActive(false);
    }

    private void Start()
    {
        playerMovement = PlayerState.Instance.GetComponent<PlayerMovement>();
        playerAnimations = PlayerState.Instance.GetComponentInChildren<CharacterAnimations>();
    }

    public void StartDuel(Opponent opponent)
    {
        if (isInDuel)
            return;

        isInDuel = true;
        currentOpponent = opponent;

        StartCoroutine(StartDuelAnim());
    }

    IEnumerator StartDuelAnim()
    {
        duelBackground.gameObject.SetActive(true);
        duelBackground.sharedMaterial.SetVector("_Origin", currentOpponent.Position);

        Vector2Int opponentPosition = currentOpponent.GridPos;
        Direction directionToEnemy = (opponentPosition - playerMovement.gridPos).ToDirection();
        playerMovement.LookDirection(directionToEnemy);

        if (currentOpponent.TryGetComponent(out IMoveable opponentMoveable))
        {
            opponentMoveable.LookDirection(directionToEnemy.Reverse());
        }

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / BACKGROUND_GROW_TIME;
            duelBackground.sharedMaterial.SetFloat("_Threshold", t);
            yield return null;
        }

        StartCoroutine(Fight());
    }

    IEnumerator Fight()
    {
        bool someoneDied = false;
        int damages;
        bool firstAttack = true;

        while (!someoneDied)
        {
            if (!firstAttack)
                yield return new WaitForSeconds(ATTACK_INTERVAL_TIME);
            playerAnimations.Attack();
            damages = PlayerState.Instance.Strength;
            currentOpponent.TakeDamages(damages, out someoneDied);
            ScreenShakeSimple.Instance.Shake(damages * 0.25f);
            firstAttack = false;

            if (!someoneDied)
            {
                yield return new WaitForSeconds(ATTACK_INTERVAL_TIME);
                currentOpponent.Attack();
                damages = currentOpponent.Strength;
                PlayerState.Instance.TakeDamages(damages, out someoneDied);
                ScreenShakeSimple.Instance.Shake(damages * 0.25f);

                //Kill
                if (someoneDied)
                {
                    Instantiate(smokePrefab, currentOpponent.transform.position, Quaternion.identity, null);
                    Destroy(currentOpponent.gameObject);
                }
            }
        }

        yield return new WaitForSeconds(ATTACK_INTERVAL_TIME * 0.5f);
        StartCoroutine(EndDuelAnim());
    }

    IEnumerator EndDuelAnim()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / (BACKGROUND_GROW_TIME);
            duelBackground.sharedMaterial.SetFloat("_Threshold", 1 - t);
            yield return null;
        }

        EndDuel();
    }

    void EndDuel()
    {
        isInDuel = false;
        duelBackground.gameObject.SetActive(false);
        currentOpponent.OnDuelFinished();
    }
}
