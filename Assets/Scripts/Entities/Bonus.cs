using UnityEngine;

public class Bonus : MonoBehaviour
{
    Vector2Int gridPos;

    public enum Type { Heal, Str }
    public Type type = Type.Heal;
    public int amount = 1;

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
            GiveBonus();
    }

    void GiveBonus()
    {
        PlayerState.Instance.onPlayerPositionChanged -= OnPlayerPositionChanged;

        if (type == Type.Heal)
            PlayerState.Instance.AddHP(amount);
        else
            PlayerState.Instance.AddStr(amount);

        SFXManager.PlaySound(GlobalSFX.Bonus);

        Destroy(gameObject);
    }
}
