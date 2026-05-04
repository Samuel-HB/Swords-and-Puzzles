using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInputs : MonoBehaviour
{
    public InputActionReference moveAction;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        moveAction.action.performed += OnMoveActionPerformed;
        moveAction.action.canceled += OnMoveActionCanceled;
        moveAction.action.Enable();
    }

    private void OnMoveActionPerformed(InputAction.CallbackContext context)
    {
        playerMovement.OnMovePerformed(context);
    }
    private void OnMoveActionCanceled(InputAction.CallbackContext context)
    {
        playerMovement.OnMoveCanceled();
    }

    private void OnDisable()
    {
        moveAction.action.performed -= OnMoveActionPerformed;
        moveAction.action.canceled -= OnMoveActionCanceled;
        moveAction.action.Disable();
    }
}
