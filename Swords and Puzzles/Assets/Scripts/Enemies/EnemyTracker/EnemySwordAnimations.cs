using System.Collections;
using UnityEngine;

public class EnemySwordAnimations : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private EnemyTracker enemyTracker;

    private IEnumerator timer;

    private string swordAttack = "SwordAttack";
    private string empty = "Empty";

    private void Start()
    {
        //animator = GetComponentInParent<Animator>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyTracker = GetComponentInParent<EnemyTracker>();

        CallCheckAnimationTimer();
    }

    protected void CallCheckAnimationTimer()
    {
        StartCoroutine(CheckAnimationTimer());
    }

    IEnumerator CheckAnimationTimer()
    {
        switch (enemyTracker.state)
        {
            case EnemyState.Attack:
                ChoseSwordAttackDirection();
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(0.05f);
        CallCheckAnimationTimer();
    }

    public void ChoseSwordAttackDirection()
    {
        switch (enemyTracker.direction)
        {
            case Directions.East:
                ChangeAnimState(false);
                break;
            case Directions.West:
                ChangeAnimState(true);
                break;
            default:
                ChangeAnimState(false);
                break;
        }
    }

    private void ChangeAnimState(bool isFlipX)
    {
        spriteRenderer.flipX = isFlipX;

        animator.Play(swordAttack);
        CallAnimTimer();
    }   

    public void CallAnimTimer()
    {
        timer = AnimTimer();
        StartCoroutine(timer);
    }

    IEnumerator AnimTimer()
    {
        yield return new WaitForSeconds(enemyTracker.attackDuration); // maybe not the correct value
        animator.Play(empty);
    }

    public void StopTimer()
    {
        if (timer != null) {
            StopCoroutine(timer);
            timer = null;
        }
    }
}
