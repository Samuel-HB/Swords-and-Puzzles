using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private float radius = 0.5f;
    private int playerLayerMask = 0;

    private int hitDamage = 2;

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

    private void CheckCollision()
    {
        Collider2D ennemyCollider = Physics2D.OverlapCircle(transform.position, radius, ~playerLayerMask); // add every layer mask ennemy can have
        if (ennemyCollider != null)
        {
            if (ennemyCollider.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                iDamageable.TakeDamage(hitDamage);
            }
            ArrowDeactivation();
        }
        else
        {
            Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius);
            if (hitCollider != null)
            {
                if (hitCollider.TryGetComponent<WallForEveryone>(out WallForEveryone wallForEveryone)) {
                    ArrowDeactivation();
                }
            }
        }
    }

    private void ArrowDeactivation()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }
}
