using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private float radius = 0.5f;
    private int playerLayerMask = 0;
    private BoxCollider2D boxCollider;
    ContactFilter2D contactFilter = new ContactFilter2D();

    private int hitDamage = 2;

    private void Start()
    {
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");

        boxCollider = GetComponent<BoxCollider2D>();
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
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = ~playerLayerMask;

        Collider2D[] hitColliders = new Collider2D[10];
        Physics2D.OverlapCollider(boxCollider, contactFilter, hitColliders);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider == null) continue;

            if (collider.TryGetComponent<IDamageable>(out IDamageable iDamageable))
            {
                iDamageable.TakeDamage(hitDamage);
                ArrowDeactivation();
            }
            else if (collider.TryGetComponent<WallForEveryone>(out WallForEveryone wallForEveryone)) {
                ArrowDeactivation();
            }
        }
    }

    private void ArrowDeactivation()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }
}
