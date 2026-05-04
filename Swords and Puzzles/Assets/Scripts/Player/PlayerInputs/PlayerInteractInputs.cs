using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractInputs : MonoBehaviour
{
    public InputActionReference interactAction;

    private PlayerInteract playerInteract;

    private void Awake()
    {
        playerInteract = GetComponent<PlayerInteract>();
    }

    private void OnEnable()
    {
        interactAction.action.started += OnInteractkActionPerformed;
        interactAction.action.Enable();
    }

    private void OnInteractkActionPerformed(InputAction.CallbackContext context)
    {
        playerInteract.OnInteract();
    }

    private void OnDisable()
    {
        interactAction.action.started -= OnInteractkActionPerformed;
        interactAction.action.Disable();
    }
}
