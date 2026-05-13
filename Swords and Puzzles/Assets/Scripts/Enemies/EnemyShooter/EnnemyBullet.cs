using UnityEngine;

public class EnnemyBullet : MonoBehaviour
{
    private float radius = 0.5f;
    private int playerLayerMask = 0;

    private int hitDamage = 1;

    private void Start()
    {
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Update()
    {
        CheckCollision();
    }


    private bool canAttack = true;
    private void CheckCollision()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radius, playerLayerMask);
        if (playerCollider != null)
        {
            if (playerCollider.TryGetComponent<IDamageable>(out IDamageable iDamageable) && canAttack)
            {
                iDamageable.TakeDamage(hitDamage);
                // just check if should take damage
                //EventManager.PlayerRemoveCollider();
                EventManager.PlayerInvulnerability();
                canAttack = false;
            }
            BulletDeactivation();
        }
        else
        {
            Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius);
            if (hitCollider != null)
            {
                if (hitCollider.TryGetComponent<WallForEveryone>(out WallForEveryone wallForEveryone)) {
                    BulletDeactivation();
                }
            }
        }
        canAttack = true;
    }

    private void BulletDeactivation()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }
}
