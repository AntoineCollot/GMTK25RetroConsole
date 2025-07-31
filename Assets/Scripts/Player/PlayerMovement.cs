using System;
using System.Collections;
using UnityEngine;

public interface IMoveable
{
    float MoveTime { get; }
    event Action<Direction> onMove;
}

public class PlayerMovement : MonoBehaviour, IMoveable
{
    InputMap inputs;
    public Vector2Int gridPos { get; private set; }
    public bool isMoving { get; private set; }

    [SerializeField] float moveTime;
    public float MoveTime => moveTime;

    const float MIN_INPUT_MOVEMENT = 0.5f;

    public event Action<Direction> onMove;

    void Start()
    {
        inputs = new InputMap();
        inputs.Enable();

        gridPos = GameGrid.WordPosToGrid(transform.position);
        transform.position = GameGrid.GridToWorldPos(gridPos);
    }

    private void OnDestroy()
    {
        if (inputs != null)
        {
            inputs.Disable();
            inputs.Dispose();
        }
    }

    void Update()
    {
        Vector2 movementInputs = inputs.Gameplay.Move.ReadValue<Vector2>();
        if (movementInputs.magnitude>MIN_INPUT_MOVEMENT)
        {
            TryMove(movementInputs.ToDirection());
        }
    }

    public void TryMove(Direction direction)
    {
        if (isMoving)
            return;

        if (!GameGrid.IsWalkable(gridPos + direction.ToVector()))
            return;

        Vector2Int nextPos = gridPos + direction.ToVector();
        StartCoroutine(MoveAnim(gridPos, nextPos));
        onMove?.Invoke(direction);
    }

    IEnumerator MoveAnim(Vector2Int from, Vector2Int to)
    {
        isMoving = true;

        Vector2 fromPos = GameGrid.GridToWorldPos(in from);
        Vector2 toPos = GameGrid.GridToWorldPos(in to);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / moveTime;

            transform.position = Vector2.Lerp(fromPos, toPos, t);

            yield return null;
        }

        gridPos = to;
        isMoving = false;
    }
}
