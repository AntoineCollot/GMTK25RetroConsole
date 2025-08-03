using UnityEngine;

public class CancelCutsceneArea : MonoBehaviour
{
    [SerializeField] StartCutscene cutscene;
    Vector2Int gridPos;

    void Start()
    {
        PlayerState.Instance.onPlayerPositionChanged += OnPlayerPositionChanged;
        gridPos = GameGrid.WordPosToGrid(transform.position);
    }

    private void OnDestroy()
    {
        if (PlayerState.Instance != null)
            PlayerState.Instance.onPlayerPositionChanged -= OnPlayerPositionChanged;
    }

    private void OnPlayerPositionChanged(Vector2Int playerPos)
    {
        if (playerPos != gridPos)
            return;

        MusicManager.Instance.EnqueueTheme(Theme.Adventure);
        cutscene.Cancel();
    }
}
