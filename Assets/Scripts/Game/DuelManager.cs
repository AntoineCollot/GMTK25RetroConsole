using System.Collections;
using UnityEngine;

public class DuelManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer duelBackground;

    public bool isInDuel { get; private set; }
    public Opponent currentOpponent {  get; private set; }
    PlayerMovement playerMovement;

    public static DuelManager Instance;

    const float BACKGROUND_GROW_TIME = 1.5f;

    private void Awake()
    {
        Instance = this;
        duelBackground.gameObject.SetActive(false);
    }

    private void Start()
    {
        playerMovement = PlayerState.Instance.GetComponent<PlayerMovement>();
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

        EndDuel();
    }

    void EndDuel()
    {
        isInDuel = false;
        duelBackground.gameObject.SetActive(false);
        currentOpponent.OnDuelFinished();
    }
}
