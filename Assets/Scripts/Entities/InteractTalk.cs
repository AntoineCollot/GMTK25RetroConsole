using UnityEngine;

public class InteractTalk : MonoBehaviour, IInteractable
{
    [TextArea, SerializeField] string[] lines;

    public void OnInteract(Direction lookDirection)
    {
        DialoguePanel.Instance.DisplayLines(lines);
    }
}