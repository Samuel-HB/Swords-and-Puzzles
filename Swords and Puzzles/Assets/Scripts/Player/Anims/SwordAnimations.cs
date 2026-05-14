using System.Collections;
using UnityEngine;

public class SwordAnimations : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Player player;

    private IEnumerator timer;

    private string swordAttack = "SwordAttack";
    private string empty = "Empty";

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GetComponentInParent<Player>();

        EventManager.swordAttacking += ChoseSwordAttackDirection;
    }

    public void ChoseSwordAttackDirection()
    {
        switch (player.direction)
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
        yield return new WaitForSeconds(player.swordDuration);
        animator.Play(empty);
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
        EventManager.swordAttacking -= ChoseSwordAttackDirection;
    }
}
