using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player player;
    [SerializeField] private PolygonCollider2D swordCollider;
    [SerializeField] protected GameObject sword;

    private float durationBeforeSwordActivation = 0.1f;
    private int swordStrength = 1;

    ContactFilter2D contactFilter = new ContactFilter2D();
    private int playerLayerMask = 0;


    private void Start()
    {
        player = GetComponent<Player>();

        playerLayerMask = 1 << LayerMask.NameToLayer("Player");

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
        StartCoroutine(WaitBeforePhysicalAttack());
        player.CallStateTimer(PlayerState.SwordAttacking, player.swordDuration);
        EventManager.SwordAttack();
    }

    IEnumerator WaitBeforePhysicalAttack()
    {
        yield return new WaitForSeconds(durationBeforeSwordActivation);

        ChoseSwordDirection();

        swordCollider.enabled = true;

        contactFilter.useLayerMask = true;
        contactFilter.layerMask = ~playerLayerMask;

        Collider2D[] enemyColliders = new Collider2D[10];
        Physics2D.OverlapCollider(swordCollider, contactFilter, enemyColliders);

        foreach (Collider2D collider in enemyColliders)
        {
            if (collider == null) continue;

            if (collider.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                iDamageable.TakeDamage(swordStrength);
            }
        }
        swordCollider.enabled = false;
    }

    public void ChoseSwordDirection()
    {
        switch (player.direction)
        {
            case Directions.North:
                ChangeSwordDirection(180);
                break;
            case Directions.South:
                ChangeSwordDirection(0);
                break;
            case Directions.East:
                ChangeSwordDirection(90);
                break;
            case Directions.West:
                ChangeSwordDirection(270);
                break;
            default:
                ChangeSwordDirection(180);
                break;
        }
    }

    private void ChangeSwordDirection(int zRotation)
    {
        sword.transform.eulerAngles = new Vector3(0, 0, zRotation);
    }
}
