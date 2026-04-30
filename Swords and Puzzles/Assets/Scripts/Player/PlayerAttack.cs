using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public InputActionReference attackAction;

    private void OnEnable()
    {
        attackAction.action.started += OnAttackActionPerformed;
        attackAction.action.Enable();
    }

    private void OnAttackActionPerformed(InputAction.CallbackContext context)
    {
        print("attack");
    }

    private void Disable()
    {
        attackAction.action.started -= OnAttackActionPerformed;
        attackAction.action.Disable();
    }
}
