using System;
using System.Collections;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField] Sprite idleDown;
    [SerializeField] Sprite moveDown;
    [SerializeField] Sprite idleRight;
    [SerializeField] Sprite moveRight;
    [SerializeField] Sprite idleUp;
    [SerializeField] Sprite moveUp;

    public IMoveable moveable { get;private set; }
    public SpriteRenderer spriteRend { get; private set; }

    const float ATTACK_MOVE_TIME = 0.05f;
    const float ATTACK_FREEZE_TIME = 0.5f;

    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        moveable = GetComponentInParent<IMoveable>();
    }

    void Start()
    {
        if (moveable != null)
        {
            moveable.onLook += OnLook;
            moveable.onMove += OnMove;
            LookDirection(moveable.CurrentDirection);
        }
    }

    private void OnDestroy()
    {
        if (moveable != null)
        {
            moveable.onLook -= OnLook;
            moveable.onMove -= OnMove;
        }
    }

    private void OnLook(Direction direction)
    {
        LookDirection(direction);
    }

    private void OnMove(Direction direction)
    {
        StartCoroutine(MoveAnim(direction, moveable.MoveTime));
    }

#if UNITY_EDITOR
    public void EditorLookDirection(Direction direction)
    {
        if (spriteRend == null)
            spriteRend = GetComponent<SpriteRenderer>();
        LookDirection(direction);
    }
#endif

    public void LookDirection(Direction direction)
    {
        spriteRend.flipX = direction == Direction.Left;
        switch (direction)
        {
            case Direction.Top:
            default:
                spriteRend.sprite = idleUp;
                break;
            case Direction.Right:
            case Direction.Left:
                spriteRend.sprite = idleRight;
                spriteRend.sprite = idleRight;
                break;
            case Direction.Down:
                spriteRend.sprite = idleDown;
                break;
        }
    }

    void DisplayMoveSprite(Direction direction)
    {
        switch (direction)
        {
            case Direction.Top:
            default:
                spriteRend.sprite = moveUp;
                break;
            case Direction.Right:
            case Direction.Left:
                spriteRend.sprite = moveRight;
                break;
            case Direction.Down:
                spriteRend.sprite = moveDown;
                break;
        }
    }

    IEnumerator MoveAnim(Direction direction, float duration)
    {
        LookDirection(direction);

        yield return new WaitForSeconds(duration * 0.25f);

        DisplayMoveSprite(direction);

        yield return new WaitForSeconds(duration * 0.5f);

        LookDirection(direction);
    }

    public void Attack()
    {
        StartCoroutine(AttackAnim());
    }

    IEnumerator AttackAnim()
    {
        //Move sprite
        DisplayMoveSprite(moveable.CurrentDirection);
        float t = 0;
        Vector3 direction = moveable.CurrentDirection.ToVector3();
        while (t < 1)
        {
            t += Time.deltaTime / ATTACK_MOVE_TIME;

            transform.localPosition = Vector3.Lerp(Vector3.zero, direction * 0.5f, t);

            yield return null;
        }

        yield return new WaitForSeconds(ATTACK_FREEZE_TIME);

        //Reset sprite
        LookDirection(moveable.CurrentDirection);
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / ATTACK_MOVE_TIME;

            transform.localPosition = Vector3.Lerp(Vector3.zero, direction * 0.5f, 1 - t);

            yield return null;
        }
    }

    public void SetSortingLayerAsDuel()
    {
        spriteRend.sortingLayerName = "Duel";
    }

    public void ResetSortingLayer()
    {
        spriteRend.sortingLayerName = "Characters";
    }
}
