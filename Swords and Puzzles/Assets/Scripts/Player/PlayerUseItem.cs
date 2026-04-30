using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUseItem : MonoBehaviour
{
    public InputActionReference useObjectAction;
    public InputActionReference shiftItemLeftAction;
    public InputActionReference shiftItemRightAction;

    private List<IUsable> items;
    [SerializeField] private Key key;
    [SerializeField] private Bomb bomb;
    [SerializeField] private Bow bow;


    private void Start()
    {
        items = new List<IUsable>() { key, bomb,  bow };
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
        if (items[0] != null) {
            items[0].UseItem();
        }
    }

    private void OnShiftItemtLeftActionPerformed(InputAction.CallbackContext context)
    {
        ShiftItemLeft();
    }

    private void OnShiftItemtRightActionPerformed(InputAction.CallbackContext context)
    {
        ShiftItemRight();
    }

    private void ShiftItemLeft()
    {
        IUsable tempItem = items[0];

        for (int i = 0; i < items.Count - 1; i++) {
            items[i] = items[i + 1];
        }
        items[items.Count - 1] = tempItem;
    }

    private void ShiftItemRight()
    {
        IUsable tempItem = items[items.Count - 1];

        for (int i = items.Count - 1; i > 0; i--) {
            items[i] = items[i - 1];
        }
        items[0] = tempItem;
    }

    //private void ShowList()
    //{
    //    foreach (IUsable item in items) {
    //        print(item);
    //    }
    //    print("");
    //}

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
