using UnityEngine;

public enum Direction { Top, Right, Left, Down }
public static class GameGrid
{
    //level layer 6
    public static LayerMask obstacleLayers = 1 << 6 | 1 << 8;
    public static LayerMask bridgeLayers = 1 << 11;

    public static Vector3 GridToWorldPos(in Vector2Int gridPos)
    {
        return new Vector3(gridPos.x + 0.5f, gridPos.y + 0.5f, 0);
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

    public static Vector3 ToVector3(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Top:
            default:
                return Vector3.up;
            case Direction.Right:
                return Vector3.right;
            case Direction.Left:
                return Vector3.left;
            case Direction.Down:
                return Vector3.down;
        }
    }

    public static Direction ToDirection(this Vector3 v)
    {
        return ((Vector2)v).ToDirection();
    }

    public static Direction ToDirection(this Vector2 v)
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
        //If obstacle, check if bridge could free the way
        if (col != null)
            return HasBridge(in gridPos);
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

    public static bool HasBridge(in Vector2Int gridPos)
    {
        Collider2D col = Physics2D.OverlapPoint(GridToWorldPos(in gridPos), bridgeLayers);
        return col != null;
    }

    public static bool CanSeeTarget(in Vector2Int fromPos, Direction lookDirection, in Vector2Int targetPos, int maxDistance = 100, bool ignoreLineOfSight = false)
    {
        //Make sure positions are aligned
        if (!IsAligned(in fromPos, lookDirection, in targetPos, maxDistance))
            return false;

        //If no check for obstacle, don't do the raycast after this point
        if (ignoreLineOfSight)
            return true;

        //Raycast to target (from next cell to avoi hitting ourselves)
        Vector2Int rayOrigin = fromPos + lookDirection.ToVector();
        RaycastHit2D hit = Physics2D.Raycast(GridToWorldPos(rayOrigin), lookDirection.ToVector(), maxDistance, obstacleLayers);
        //If nothing is hit, we can see the player
        if (hit.collider == null)
            return true;

        //Otherwise make sure the hit happens farther than target
        return hit.distance > GridDistance(in rayOrigin, in targetPos);
    }

    public static bool IsAligned(in Vector2Int fromPos, Direction lookDirection, in Vector2Int targetPos, int maxDistance = 100)
    {
        bool areAligned;
        switch (lookDirection)
        {
            case Direction.Top:
            default:
                areAligned = fromPos.x == targetPos.x && targetPos.y > fromPos.y;
                break;
            case Direction.Right:
                areAligned = fromPos.y == targetPos.y && targetPos.x > fromPos.x;
                break;
            case Direction.Left:
                areAligned = fromPos.y == targetPos.y && targetPos.x < fromPos.x;
                break;
            case Direction.Down:
                areAligned = fromPos.x == targetPos.x && targetPos.y < fromPos.y;
                break;
        }

        areAligned &= GridDistance(in fromPos, in targetPos) <= maxDistance;
        return areAligned;
    }

    public static int GridDistance(in Vector2Int p1, in Vector2Int p2)
    {
        return Mathf.Abs(p1.x - p2.x) + Mathf.Abs(p1.y - p2.y);
    }

    public static Direction Reverse(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Top:
            default:
                return Direction.Down;
            case Direction.Right:
                return Direction.Left;
            case Direction.Left:
                return Direction.Right;
            case Direction.Down:
                return Direction.Top;
        }
    }
}
