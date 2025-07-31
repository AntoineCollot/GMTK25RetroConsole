using UnityEngine;

public enum Direction { Top, Right, Left, Down }
public static class GameGrid
{
    //level layer 6
    public static LayerMask obstacleLayers = 1<<6;

    public static Vector3 GridToWorldPos(in Vector2Int gridPos)
    {
        return new Vector3(gridPos.x+0.5f, gridPos.y + 0.5f, 0);
    }

    public static Vector2Int WordPosToGrid(in Vector3 wPos)
    {
        return new Vector2Int(Mathf.FloorToInt(wPos.x), Mathf.FloorToInt(wPos.y));
    }

    public static Vector2Int ToVector(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Top:
            default:
                return Vector2Int.up;
            case Direction.Right:
                return Vector2Int.right;
            case Direction.Left:
                return Vector2Int.left;
            case Direction.Down:
                return Vector2Int.down;
        }
    }

    public static Direction ToDirection(this Vector2 v)
    {
        if(Mathf.Abs(v.x)>Mathf.Abs(v.y))
        {
            if (v.x >= 0)
                return Direction.Right;
            return Direction.Left;
        }
        if (v.y >= 0)
            return Direction.Top;
        return Direction.Down;
    }

    public static Direction ToDirection(this Vector2Int v)
    {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
        {
            if (v.x >= 0)
                return Direction.Right;
            return Direction.Left;
        }
        if (v.y >= 0)
            return Direction.Top;
        return Direction.Down;
    }

    public static bool IsWalkable(in Vector2Int gridPos)
    {
        Collider2D col = Physics2D.OverlapPoint(GridToWorldPos(in gridPos), obstacleLayers);
        return col == null;
    }

    public static bool TryGetObjectInFront(Direction lookDir, in Vector2Int currentPos, out GameObject go)
    {
        go = GetObjectAt(currentPos + lookDir.ToVector());
        return go != null;
    }

    public static GameObject GetObjectAt(in Vector2Int gridPos)
    {
        Collider2D col = Physics2D.OverlapPoint(GridToWorldPos(in gridPos));
        if (col == null)
            return null;
        return col.gameObject;
    }
}
