using System.Collections;
using UnityEngine;

public class PlayerAnimations : PlayerAnimationNames
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Player player;

    private IEnumerator timer;

    private string currentState;


    private void Awake()
    {
        currentState = idleDown;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();

        EventManager.playerMoving += Move;
        EventManager.playerStopMoving += BackToIdle;
        EventManager.firingArrow += FireArrow;
        EventManager.throwingBomb += ThrowBow;
        EventManager.swordAttacking += SwordAttack;
    }

    private void Update()
    {
        switch (player.direction)
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

    public void FireArrow()
    {
        ChoseAnim(firingArrowUp, firingArrowDown, firingArrowRight, player.bowDuration);
    }
    public void ThrowBow()
    {
        ChoseAnim(throwingBombUp, throwingBombDown, throwingBombRight, player.bombDuration);
    }
    public void SwordAttack()
    {
        ChoseAnim(swordUp, swordDown, swordRight, player.swordDuration);
    }
    public void Move()
    {
        ChoseAnim(walkUp, walkDown, walkRight, 0f);
    }
    public void BackToIdle()
    {
        switch (player.state)
        {
            case PlayerState.Idle:
                ChoseAnim(idleUp, idleDown, idleRight, 0f);
                break;
            default:
                break;
        }
    }

    public void ChoseAnim(string animUp, string animDown, string animRight, float animTime)
    {
        //float timeToWait = animTime; // useless ?
        switch (player.direction)
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

        //animator.Play(newState);
        animator.CrossFadeInFixedTime(newState, 0f); // 0.2f before
        currentState = newState;

        if (animTime != 0) {
            CallAnimTimer(animTime);
        }
    }   

    public void CallAnimTimer(float timeToWait)
    {
        timer = AnimTimer(timeToWait);
        StartCoroutine(timer);
    }

    IEnumerator AnimTimer(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        BackToIdle();
    }

    public void StopTimer()
    {
        if (timer != null) {
            StopCoroutine(timer);
            timer = null;
        }
    }

    private void OnDestroy()
    {
        EventManager.playerMoving -= Move;
        EventManager.playerStopMoving -= BackToIdle;
        EventManager.firingArrow -= FireArrow;
        EventManager.throwingBomb -= ThrowBow;
        EventManager.swordAttacking -= SwordAttack;
    }
}
