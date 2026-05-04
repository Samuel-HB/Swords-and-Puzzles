using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemInputs : MonoBehaviour
{
    public InputActionReference useObjectAction;
    public InputActionReference shiftItemLeftAction;
    public InputActionReference shiftItemRightAction;

    private PlayerUseItem playerUseItem;

    private void Awake()
    {
        playerUseItem = GetComponent<PlayerUseItem>();
    }

    private void OnEnable()
    {
        useObjectAction.action.started += OnUseItemtActionPerformed;
        useObjectAction.action.Enable();
        shiftItemLeftAction.action.started += OnShiftItemtLeftActionPerformed;
        shiftItemLeftAction.action.Enable();
        shiftItemRightAction.action.started += OnShiftItemtRightActionPerformed;
        shiftItemRightAction.action.Enable();
    }

    private void OnUseItemtActionPerformed(InputAction.CallbackContext context)
    {
        playerUseItem.UseItem();
    }
    private void OnShiftItemtLeftActionPerformed(InputAction.CallbackContext context)
    {
        playerUseItem.ShiftItemLeft();
    }
    private void OnShiftItemtRightActionPerformed(InputAction.CallbackContext context)
    {
        playerUseItem.ShiftItemRight();
    }

    private void OnDisable()
    {
        useObjectAction.action.started -= OnUseItemtActionPerformed;
        useObjectAction.action.Disable();
        shiftItemLeftAction.action.started -= OnShiftItemtLeftActionPerformed;
        shiftItemLeftAction.action.Disable();
        shiftItemRightAction.action.started -= OnShiftItemtRightActionPerformed;
        shiftItemRightAction.action.Disable();
    }
}
