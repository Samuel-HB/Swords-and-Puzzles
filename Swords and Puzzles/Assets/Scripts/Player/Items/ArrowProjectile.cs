using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private int playerLayerMask = 0;
    [SerializeField] private BoxCollider2D colliderForEnemy;
    [SerializeField] private BoxCollider2D colliderForWall;
    ContactFilter2D contactFilter = new ContactFilter2D();

    private int hitDamage = 2;

    private void Start()
    {
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
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
        Physics2D.OverlapCollider(colliderForEnemy, contactFilter, hitColliders);

        // bigger collider for enemies
        foreach (Collider2D collider in hitColliders)
        {
            if (collider == null) continue;

            if (collider.TryGetComponent<IDamageable>(out IDamageable iDamageable))
            {
                iDamageable.TakeDamage(hitDamage);
                ArrowDeactivation();
            }
        }
        // smaller collider for wall, to avoid to collide on them as soon as launched if player near wall
        hitColliders = new Collider2D[10];
        Physics2D.OverlapCollider(colliderForWall, contactFilter, hitColliders);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider == null) continue;

            if (collider.TryGetComponent<WallForEveryone>(out WallForEveryone wallForEveryone)) {
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
