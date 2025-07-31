using UnityEngine;

public class Opponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject detectFX;
    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] float detectFreezeTime = 1;

    CompositeStateToken freezePlayerToken;

    public Vector2 Position => transform.position;
    public Vector2Int GridPos => GameGrid.WordPosToGrid(transform.position);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        freezePlayerToken = new CompositeStateToken();
    }

    private void OnDestroy()
    {
    }

    public void OnPlayerDetected(Vector2Int playerPosition)
    {
        PlayerState.Instance.freezeInputsState.Add(freezePlayerToken);
        freezePlayerToken.SetOn(true);
        detectFX.SetActive(true);

        Invoke("StartDuel", detectFreezeTime);
    }

    void StartDuel()
    {
        detectFX.SetActive(false);
        SetSortingLayerAsDuel();
        DuelManager.Instance.StartDuel(this);
    }

    public void OnDuelFinished()
    {
        freezePlayerToken.SetOn(false);
        PlayerState.Instance.freezeInputsState.Remove(freezePlayerToken);
        ResetSortingLayer();
    }

    void SetSortingLayerAsDuel()
    {
        spriteRenderer.sortingLayerName = "Duel";
    }

    void ResetSortingLayer()
    {
        spriteRenderer.sortingLayerName = "Characters";
    }
}
