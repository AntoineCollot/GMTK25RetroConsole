using System;
using UnityEngine;

public class LookInteractionNPC : MonoBehaviour, IMoveable
{
    public float MoveTime => 0;

    [SerializeField] Direction lookDirection;
    public Direction CurrentDirection => lookDirection;
    public GameObject GO => gameObject;

    public event Action<Direction> onMove;
    public event Action<Direction> onLook;

#if UNITY_EDITOR
    void OnValidate()
    {
        CharacterAnimations anims = GetComponentInChildren<CharacterAnimations>();
        if(anims!=null)
        {
            anims.EditorLookDirection(lookDirection);
        }
    }
#endif

    public void LookDirection(Direction direction)
    {
        lookDirection = direction;
        onLook?.Invoke(direction);
    }
}
