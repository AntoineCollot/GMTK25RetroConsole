using UnityEngine;

public class HealHeart : MonoBehaviour
{
    Vector2Int gridPos;

    private void Start()
    {
        gridPos = GameGrid.WordPosToGrid(transform.position);
        PlayerState.Instance.onPlayerPositionChanged += OnPlayerPositionChanged;
    }

    private void OnDestroy()
    {
        if (PlayerState.Instance != null)
            PlayerState.Instance.onPlayerPositionChanged += OnPlayerPositionChanged;
    }

    private void OnPlayerPositionChanged(Vector2Int pos)
    {
        if (pos == gridPos)
            GiveHealth();
    }

    void GiveHealth()
    {
        PlayerState.Instance.onPlayerPositionChanged -= OnPlayerPositionChanged;
        PlayerState.Instance.AddHP(1);

        SFXManager.PlaySound(GlobalSFX.Bonus);

        Destroy(gameObject);
    }
}
