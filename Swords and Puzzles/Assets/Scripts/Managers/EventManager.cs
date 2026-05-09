using System;

public static class EventManager
{
    public static event Action playerMoving;
    public static event Action playerStopMoving;
    public static event Action firingArrow;
    public static event Action throwingBomb;
    public static event Action swordAttacking;
    public static event Action goingBackToIdle;
    public static event Action playerInvulnerability;

    public static event Action bombExploding;


    public static event Action updatingItems;

    public static event Action shootingOnPlayer;


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
    public static void PlayerInvulnerability()
    {
        playerInvulnerability?.Invoke();
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
}
