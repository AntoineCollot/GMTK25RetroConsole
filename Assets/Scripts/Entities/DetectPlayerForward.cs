using System;
using UnityEngine;

public class DetectPlayerForward : MonoBehaviour
{
    IMoveable moveable;
    [SerializeField] int maxDistance = 100;
    [SerializeField] bool ignoreLightOfSight = false;

    private void Awake()
    {
        moveable = GetComponent<IMoveable>();
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
        Vector2Int pos = GameGrid.WordPosToGrid(transform.position);
        if (GameGrid.CanSeeTarget(in pos, moveable.CurrentDirection, in playerPos, maxDistance, ignoreLightOfSight))
        {
            SendMessage("OnPlayerDetected", playerPos);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (moveable == null)
            moveable = GetComponent<IMoveable>();

        Vector2 dir = moveable.CurrentDirection.ToVector() * maxDistance;
        Vector3 targetPos = transform.position + new Vector3(dir.x, dir.y, 0);
        Gizmos.DrawLine(transform.position, targetPos);
    }
#endif
}
