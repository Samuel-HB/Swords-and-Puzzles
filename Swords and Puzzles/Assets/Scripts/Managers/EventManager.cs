using System;

public static class EventManager
{
    public static event Action playerMoving;
    public static event Action playerStopMoving;
    public static event Action firingArrow;
    public static event Action throwingBomb;
    public static event Action swordAttacking;
    public static event Action goingBackToIdle;

    public static event Action bombExploding;
    public static event Action updatingItems;

    public static event Action shootingOnPlayer;
    public static event Action hittingOnPlayer;

    public static event Action pressingPauseGameButton;
    public static event Action pausingGame;
    public static event Action resumingGame;

    public static event Action settingPlayerHealth;
    public static event Action savingPlayerPosition;
    public static event Action playerInvulnerability;
    public static event Action playerRemovingCollider;
    public static event Action playerLoosing;
    public static event Action restartingPlayer;

    public static event Action enteringDialogue;
    public static event Action exitingDialogue;

    public static event Action deactivatingPlayerInputs;
    public static event Action activatingPlayerInputs;


    public static void PlayerMove()
    {
        playerMoving?.Invoke();
    }
    public static void PlayerMoveStop()
    {
        playerStopMoving?.Invoke();
    }
    public static void FireArrow()
    {
        firingArrow?.Invoke();
    }
    public static void ThrowBomb()
    {
        throwingBomb?.Invoke();
    }
    public static void SwordAttack()
    {
        swordAttacking?.Invoke();
    }
    public static void BackToIdle()
    {
        goingBackToIdle?.Invoke();
    }


    public static void BombExplosion()
    {
        bombExploding?.Invoke();
    }
    public static void UpdateItems()
    {
        updatingItems?.Invoke();
    }


    public static void ShootOnPlayer()
    {
        shootingOnPlayer?.Invoke();
    }
    public static void HitOnPlayer()
    {
        hittingOnPlayer?.Invoke();
    }


    public static void PressPauseGameButton()
    {
        pressingPauseGameButton?.Invoke();
    }
    public static void PauseGame()
    {
        pausingGame?.Invoke();
    }
    public static void ResumeGame()
    {
        resumingGame?.Invoke();
    }


    public static void SetPlayerHealth()
    {
        settingPlayerHealth?.Invoke();
    }
    public static void SavePlayerPosition()
    {
        savingPlayerPosition?.Invoke();
    }
    public static void PlayerInvulnerability()
    {
        playerInvulnerability?.Invoke();
    }
    public static void PlayerRemoveCollider()
    {
        playerRemovingCollider?.Invoke();
    }
    public static void PlayerLose()
    {
        playerLoosing?.Invoke();
    }
    public static void RestartPlayer()
    {
        restartingPlayer?.Invoke();
    }


    public static void EnterDialogue()
    {
        enteringDialogue?.Invoke();
    }
    public static void ExitDialogue()
    {
        exitingDialogue?.Invoke();
    }


    public static void DeactivatePlayerInputs()
    {
        deactivatingPlayerInputs?.Invoke();
    }
    public static void ActivatePlayerInputs()
    {
        activatingPlayerInputs?.Invoke();
    }
}
