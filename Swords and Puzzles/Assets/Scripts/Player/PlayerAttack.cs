using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();        
    }

    public void OnAttackPerformed()
    {
        switch (player.state)
        {
            case PlayerState.UsingItem:
                break;
            case PlayerState.SwordAttacking:
                break;
            case PlayerState.Idle:
                OnAttack();
                break;
            default:
                break;
        }
    }

    private void OnAttack()
    {
        player.CallStateTimer(PlayerState.SwordAttacking, player.swordDuration);
        EventManager.SwordAttack();
    }
}
