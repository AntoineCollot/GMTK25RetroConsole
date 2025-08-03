using System;
using System.Linq;
using UnityEngine;

public class DetectPlayerOnCells : MonoBehaviour
{
    [SerializeField] Vector2Int[] cells;

    private void Awake()
    {
    }

    void Start()
    {
        PlayerState.Instance.onPlayerPositionChanged += OnPlayerPositionChanged;
    }

    private void OnDestroy()
    {
        if (PlayerState.Instance != null)
            PlayerState.Instance.onPlayerPositionChanged -= OnPlayerPositionChanged;
    }

    private void OnPlayerPositionChanged(Vector2Int playerPos)
    {
        if (cells.Contains(playerPos))
        {
            SendMessage("OnPlayerDetected", playerPos);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (cells == null)
            return;
        for (int i = 0; i < cells.Length; i++)
        {
            Gizmos.DrawWireCube(GameGrid.GridToWorldPos(cells[i]),Vector2.one);
        }
    }
#endif
}
