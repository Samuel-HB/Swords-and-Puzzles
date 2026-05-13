using System.Collections;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    private int playerLayerMask = 0;
    private CircleCollider2D circleCollider;
    ContactFilter2D contactFilter = new ContactFilter2D();

    private int hitDamage = 5;
    private bool hasBombExploded = false;

    private Animator animator;
    private string bombState = "BombState";
    private string explosionEffect = "ExplosionEffect";


    private void Start()
    {
        animator = GetComponent<Animator>();

        circleCollider = GetComponent<CircleCollider2D>();

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

        Collider2D[] hitColliders = new Collider2D[10];
        Physics2D.OverlapCollider(circleCollider, contactFilter, hitColliders);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider == null) continue;

            if (collider.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                BombExplosion();
            }
            else if (collider.TryGetComponent<WallForEveryone>(out WallForEveryone wallForEveryone)) {
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
        Physics2D.OverlapCollider(circleCollider, contactFilter, hitColliders);

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
