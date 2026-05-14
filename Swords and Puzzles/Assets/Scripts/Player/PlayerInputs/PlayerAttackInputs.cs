using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackInputs : MonoBehaviour
{
    public InputActionReference attackAction;

    private PlayerAttack playerAttack;

    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        attackAction.action.started += OnAttackActionPerformed;
        attackAction.action.Enable();
    }

    private void OnAttackActionPerformed(InputAction.CallbackContext context)
    {
        playerAttack.OnAttackPerformed();
    }

    private void OnDisable()
    {
        attackAction.action.started -= OnAttackActionPerformed;
        attackAction.action.Disable();
    }
}
