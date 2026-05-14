using UnityEngine;

public class EnnemyBullet : MonoBehaviour
{
    private float radius = 0.5f;

    private int hitDamage = 1;
    private bool canAttack = true;


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Update()
    {
        CheckCollision();
    }


    private void CheckCollision()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius);
        if (hitCollider != null)
        {
            if (hitCollider.TryGetComponent<Player>(out Player player) && canAttack &&
                hitCollider.TryGetComponent<IDamageable>(out IDamageable iDamageable) && canAttack)
            {
                iDamageable.TakeDamage(hitDamage);
                EventManager.PlayerInvulnerability();
                canAttack = false;
                BulletDeactivation();
            }
            else if (hitCollider.TryGetComponent<WallForEveryone>(out WallForEveryone wallForEveryone))  {
                BulletDeactivation();
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
