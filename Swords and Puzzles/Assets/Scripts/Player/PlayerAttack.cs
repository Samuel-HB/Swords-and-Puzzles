using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player player;
    [SerializeField] private PolygonCollider2D swordCollider;

    private void Start()
    {
        EventManager.goingBackToIdle += DeactivateSword;

        player = GetComponent<Player>();

        swordCollider.enabled = false;
    }

    public void OnAttackPerformed()
    {
        switch (player.state)
        {
            case PlayerState.Idle:
                OnAttack();
                break;
            default:
                break;
        }
    }

    private void OnAttack()
    {
        swordCollider.enabled = true;
        player.CallStateTimer(PlayerState.SwordAttacking, player.swordDuration);
        EventManager.SwordAttack();
    }

    private void DeactivateSword()
    {
        swordCollider.enabled = false;
    }

    private void OnDestroy()
    {
        EventManager.goingBackToIdle -= DeactivateSword;
    }
}
