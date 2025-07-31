using System.Collections;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    Sprite idleDown;
    [SerializeField] Sprite moveDown;
    [SerializeField] Sprite idleRight;
    [SerializeField] Sprite moveRight;
    [SerializeField] Sprite idleUp;
    [SerializeField] Sprite moveUp;

    IMoveable moveable;
    SpriteRenderer spriteRend;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        idleDown = spriteRend.sprite;
        moveable = GetComponentInParent<IMoveable>();
        if(moveable != null)
            moveable.onMove += OnMove;
    }

    private void OnDestroy()
    {
        if (moveable != null)
            moveable.onMove -= OnMove;
    }

    private void OnMove(Direction direction)
    {
        StartCoroutine(MoveAnim(direction, moveable.MoveTime));
    }

    IEnumerator MoveAnim(Direction direction, float duration)
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

        yield return new WaitForSeconds(duration * 0.25f);

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

        yield return new WaitForSeconds(duration * 0.5f);

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
}
