using System.Collections;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    private int playerLayerMask = 0;
    [SerializeField] private CircleCollider2D colliderForEnemy;
    [SerializeField] private CircleCollider2D colliderForWall;
    ContactFilter2D contactFilter = new ContactFilter2D();

    private int hitDamage = 5;
    private bool hasBombExploded = false;

    private Animator animator;
    private string bombState = "BombState";
    private string explosionEffect = "ExplosionEffect";


    private void Start()
    {
        animator = GetComponent<Animator>();

        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        if (!hasBombExploded) {
            CheckCollision();
        }
    }

    public void CheckCollision()
    {
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = ~playerLayerMask;

        // bigger collider for enemies
        Collider2D[] hitColliders = new Collider2D[10];
        Physics2D.OverlapCollider(colliderForEnemy, contactFilter, hitColliders);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider == null) continue;

            if (collider.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                BombExplosion();
            }
        }
        // smaller collider for wall, to avoid to collide on them as soon as launched if player near wall
        hitColliders = new Collider2D[10];
        Physics2D.OverlapCollider(colliderForWall, contactFilter, hitColliders);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider == null) continue;

            if (collider.TryGetComponent<WallForEveryone>(out WallForEveryone wallForEveryone)) {
                BombExplosion();
            }
        }
    }

    public void StartWaitBeforeExplodeTimer()
    {
        StartCoroutine(WaitBeforeExplodeTimer());
    }

    IEnumerator WaitBeforeExplodeTimer()
    {
        yield return new WaitForSeconds(1f);
        BombExplosion();
    }

    private void BombExplosion()
    {
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = ~playerLayerMask;

        Collider2D[] hitColliders = new Collider2D[10];
        Physics2D.OverlapCollider(colliderForEnemy, contactFilter, hitColliders);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider == null) continue;

            if (collider.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                iDamageable.TakeDamage(hitDamage);
            }
        }
        hasBombExploded = true;
        StartCoroutine(WaitBeforeDeactivate());
    }

    IEnumerator WaitBeforeDeactivate()
    {
        animator.CrossFadeInFixedTime(explosionEffect, 0f);
        yield return new WaitForSeconds(0.35f);
        animator.CrossFadeInFixedTime(bombState, 0f);

        hasBombExploded = false;
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }
}
