using System.Collections.Generic;
using UnityEngine;

public class DeactivateAllInputs : MonoBehaviour
{
    private List<MonoBehaviour> inputs;
    private PlayerMovementInputs playerMovementInputs;
    private PlayerAttackInputs playerAttackInputs;
    private PlayerInteractInputs playerInteractInputs;
    private PlayerItemInputs playerItemInputs;

    private void Start()
    {

        EventManager.deactivatingPlayerInputs += DeactivateAllPlayerInputs;
        EventManager.activatingPlayerInputs += ActivateAllPlayerInputs;

        EventManager.playerLoosing += DeactivateAllPlayerInputs;
        EventManager.restartingPlayer += ActivateAllPlayerInputs;


        playerMovementInputs = GetComponent<PlayerMovementInputs>();
        playerAttackInputs = GetComponent<PlayerAttackInputs>();
        playerInteractInputs = GetComponent<PlayerInteractInputs>();
        playerItemInputs = GetComponent<PlayerItemInputs>();

        inputs = new List<MonoBehaviour>() { playerMovementInputs, playerAttackInputs,
                                             playerInteractInputs, playerItemInputs };
    }

    private void DeactivateAllPlayerInputs()
    {
        foreach (MonoBehaviour input in inputs) {
            input.enabled = false;
        }
    }

    private void ActivateAllPlayerInputs()
    {

        foreach (MonoBehaviour input in inputs) {
            input.enabled = true;
        }
    }

    private void OnDestroy()
    {
        EventManager.deactivatingPlayerInputs -= DeactivateAllPlayerInputs;
        EventManager.activatingPlayerInputs -= ActivateAllPlayerInputs;

        EventManager.playerLoosing -= DeactivateAllPlayerInputs;
        EventManager.restartingPlayer -= ActivateAllPlayerInputs;
    }
}
