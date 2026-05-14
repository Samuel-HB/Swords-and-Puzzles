using System.Collections;
using UnityEngine;

public class EnemyTrackerAnimations : EnemyTrackerAnimationsNames
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private EnemyTracker enemyTracker;

    private string currentState;


    private void Awake()
    {
        currentState = walkDown;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyTracker = GetComponent<EnemyTracker>();

        CallCheckAnimationTimer();
    }

    protected void CallCheckAnimationTimer()
    {
        StartCoroutine(CheckAnimationTimer());
    }

    IEnumerator CheckAnimationTimer()
    {
        yield return new WaitForSeconds(0.2f);

        switch (enemyTracker.state)
        {
            case EnemyState.Attack:
                SwordAttack();
                break;
            default:
                Move();
                break;
        }
        CheckRightOrLeftLook();

        CallCheckAnimationTimer();
    }

    private void CheckRightOrLeftLook()
    {
        switch (enemyTracker.direction)
        {
            case Directions.East:
                spriteRenderer.flipX = false;
                break;
            case Directions.West:
                spriteRenderer.flipX = true;
                break;
            default:
                spriteRenderer.flipX = false;
                break;
        }
    }

    public void SwordAttack()
    {
        ChoseAnim(swordUp, swordDown, swordRight, enemyTracker.attackDuration);
    }
    public void Move()
    {
        ChoseAnim(walkUp, walkDown, walkRight, 0f);
    }

    public void ChoseAnim(string animUp, string animDown, string animRight, float animTime)
    {
        switch (enemyTracker.direction)
        {
            case Directions.North:
                ChangeAnimState(animUp, animTime);
                break;
            case Directions.South:
                ChangeAnimState(animDown, animTime);
                break;
            case Directions.East:
                ChangeAnimState(animRight, animTime);
                break;
            case Directions.West:
                ChangeAnimState(animRight, animTime);
                break;
            default:
                ChangeAnimState(animUp, animTime);
                break;
        }
    }

    private void ChangeAnimState(string newState, float animTime)
    {
        if (currentState == newState) return;

        animator.CrossFadeInFixedTime(newState, 0f);
        currentState = newState;
    }    
}
