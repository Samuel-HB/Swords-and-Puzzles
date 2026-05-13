using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGameInputs : MonoBehaviour
{
    public InputActionReference pauseGameAction;


    private void OnEnable()
    {
        pauseGameAction.action.started += OnAttackActionPerformed;
        pauseGameAction.action.Enable();
    }

    private void OnAttackActionPerformed(InputAction.CallbackContext context)
    {
        //EventManager.PauseGame();
        EventManager.PressPauseGameButton();
    }

    private void OnDisable()
    {
        pauseGameAction.action.started -= OnAttackActionPerformed;
        pauseGameAction.action.Disable();
    }
}
