using System.Collections;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    private float radius = 0.5f;
    private int playerLayerMask = 0;

    private int hitDamage = 5;

    private Animator animator;
    private string bombState = "BombState";
    private string explosionEffect = "ExplosionEffect";


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        CheckCollision();
    }

    public void CheckCollision()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius, ~playerLayerMask);
        if (hitCollider != null)
        {
            if (hitCollider.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                BombExplosion();
            }
            else if (hitCollider.TryGetComponent<WallForEveryone>(out WallForEveryone wallForEveryone)) {
                BombExplosion();
            }
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
        Collider2D ennemyCollider = Physics2D.OverlapCircle(transform.position, radius * 2, ~playerLayerMask);
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
