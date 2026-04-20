using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public InputActionReference attackAction;
    public InputActionReference chargedAttackAction; // also make simple attack for now

    private void OnEnable()
    {
        attackAction.action.started += OnAttackActionPerformed;
        attackAction.action.Enable();

        chargedAttackAction.action.started += OnChargedAttackActionPerformed;
        chargedAttackAction.action.Enable();
    }

    private void OnAttackActionPerformed(InputAction.CallbackContext context)
    {
        print("attack");
    }

    private void OnChargedAttackActionPerformed(InputAction.CallbackContext context)
    {
        print("charged attack");
    }

    //private void Start()
    //{

    //}

    private void Disable()
    {
        attackAction.action.started -= OnAttackActionPerformed;
        attackAction.action.Disable();

        chargedAttackAction.action.started -= OnChargedAttackActionPerformed;
        chargedAttackAction.action.Disable();
    }
}
