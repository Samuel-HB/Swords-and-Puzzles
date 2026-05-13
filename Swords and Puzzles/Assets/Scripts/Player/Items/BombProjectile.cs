using System.Collections;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    private float radius = 0.35f;
    private float radiusMultiplier = 2.5f;
    private int playerLayerMask = 0;

    private int hitDamage = 5;

    private Animator animator;
    private string bombState = "BombState";
    private string explosionEffect = "ExplosionEffect";


    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}

    private void Start()
    {
        animator = GetComponent<Animator>();
        //
        circleCollider = GetComponent<CircleCollider2D>();

        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        CheckCollision();
    }


    private CircleCollider2D circleCollider;
    ContactFilter2D contactFilter = new ContactFilter2D();
    public void CheckCollision()
    {
        //Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius, ~playerLayerMask);
        //if (hitCollider != null)
        //{

        contactFilter.useLayerMask = true;
        contactFilter.layerMask = ~playerLayerMask;

        Collider2D[] hitColliders = new Collider2D[10];
        Physics2D.OverlapCollider(circleCollider, contactFilter, hitColliders);

        //Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, ~playerLayerMask, -99f, 99f);
        foreach (Collider2D collider in hitColliders)
        {
            //
            if (collider == null) continue;


            if (collider.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                BombExplosion();
            }
            else if (collider.TryGetComponent<WallForEveryone>(out WallForEveryone wallForEveryone)) {
                BombExplosion();
            }
            //if (hitCollider.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
            //BombExplosion();
            //}
            //else if (hitCollider.TryGetComponent<WallForEveryone>(out WallForEveryone wallForEveryone)) {
            //    BombExplosion();
            //}
        }
        //else
        //{
        //    Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius);
        //    if (hitCollider != null)
        //    {
        //        if (hitCollider.TryGetComponent<WallForEveryone>(out WallForEveryone wallForEveryone)) {
        //            BombExplosion();
        //        }
        //    }
        //}
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
        Collider2D ennemyCollider = Physics2D.OverlapCircle(transform.position, radius * radiusMultiplier, ~playerLayerMask);
        if (ennemyCollider != null)
        {
            if (ennemyCollider.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                iDamageable.TakeDamage(hitDamage);
            }
        }
        StartCoroutine(WaitBeforeDeactivate());
    }

    IEnumerator WaitBeforeDeactivate()
    {
        animator.CrossFadeInFixedTime(explosionEffect, 0f);
        yield return new WaitForSeconds(0.35f);
        animator.CrossFadeInFixedTime(bombState, 0f);

        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }
}
