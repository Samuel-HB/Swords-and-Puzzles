using UnityEngine;

public class EnnemyBullet : MonoBehaviour
{

    private float radius = 0.5f;
    private int playerLayerMask = 0;
    private int wallLayerMask = 0;

    private int hitDamage = 2;


    private void Start()
    {
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
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
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radius, playerLayerMask);
        if (playerCollider != null)
        {
            print("bullet explosion");
            if (playerCollider.TryGetComponent<Health>(out Health health)) {
                health.TakeDamage(hitDamage);
            }
            BulletDeactivation();
        }
        else {
            Collider2D wallCollider = Physics2D.OverlapCircle(transform.position, radius, wallLayerMask);
            if (wallCollider != null)
            {
                print("bullet explosion");
                BulletDeactivation();
            }
        }
    }

    private void BulletDeactivation()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }
}
