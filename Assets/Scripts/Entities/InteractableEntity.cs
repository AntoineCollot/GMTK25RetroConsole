using UnityEngine;

public class InteractableEntity : MonoBehaviour
{
    protected IMoveable moveable;
    IInteractable[] interactables;

    protected virtual bool CanBeInteractedWith => true;

    protected void Start()
    {
        moveable = GetComponent<IMoveable>();
        interactables = GetComponents<IInteractable>();
    }

    public void OnInteract(Vector2Int fromPos)
    {
        if(CanBeInteractedWith)
        {
            Vector2Int gridPos = GameGrid.WordPosToGrid(transform.position);
            Direction dir = (fromPos - gridPos).ToDirection();
            Interact(dir);
        }
    }

    protected void Interact(Direction dir)
    {
        //Look Source
        if(moveable != null)
        {
            moveable.LookDirection(dir);
        }

        foreach (IInteractable interactable in interactables)
        {
            interactable.OnInteract(dir);
        }
    }
}

public interface IInteractable
{
    public void OnInteract(Direction lookDirection);
}