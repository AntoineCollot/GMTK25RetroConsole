using UnityEngine;

public class InteractPushButton : MonoBehaviour, IInteractable
{
    [SerializeField] int range = 4;
    [SerializeField] int startOffset = 0;
    [SerializeField] Direction direction = Direction.Down;

    [Header("Items")]
    [SerializeField] Transform[] alignedItems;
    Vector2Int gridPos;
    Vector2Int originPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridPos = GameGrid.WordPosToGrid(transform.position);
        originPos = gridPos + direction.ToVector() * startOffset;
    }

    public void OnInteract(Direction lookDirection)
    {
        //Can't use from forward
        if (lookDirection == direction)
            return;
        Push();
    }

    public void Push()
    {
        Vector2Int itemPos;
        foreach (Transform item in alignedItems)
        {
            itemPos = GameGrid.WordPosToGrid(item.position);
            itemPos += direction.ToVector();
            if(GameGrid.GridDistance(itemPos, originPos)>=range)
            {
                itemPos = originPos;
            }
            item.position = GameGrid.GridToWorldPos(itemPos);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 dir = direction.ToVector3();
        Vector3 origin = transform.position + dir * startOffset;
        Gizmos.DrawLine(origin, transform.position + dir * range);
    }
#endif
}
