using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    InputMap inputs;
    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        inputs = new InputMap();
        inputs.Enable();
        inputs.Gameplay.A.performed += OnAPerformed;
    }

    private void OnDestroy()
    {
        if (inputs != null)
        {
            inputs.Gameplay.A.performed -= OnAPerformed;
            inputs.Disable();
            inputs.Dispose();
        }
    }

    private void OnAPerformed(InputAction.CallbackContext context)
    {
        if (PlayerState.Instance.AreGameplayInputsFrozen)
            return;

        if (GameGrid.TryGetObjectInFront(playerMovement.CurrentDirection, playerMovement.gridPos, out GameObject candidate))
        {
            if (candidate.TryGetComponent(out InteractableEntity interactable))
            {
                interactable.PlayerInteract(playerMovement.gridPos);
                SFXManager.PlaySound(GlobalSFX.Interact);
            }
        }

    }
}
