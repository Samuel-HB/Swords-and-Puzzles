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
        animator = GetComponentInParent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GetComponentInParent<Player>();

        EventManager.swordAttacking += ChoseSwordAttackDirection;
    }

    public void ChoseSwordAttackDirection()
    {
        switch (player.direction)
        {
            case Directions.North:
                ChangeAnimState(180, false);
                break;
            case Directions.South:
                ChangeAnimState(0, false);
                break;
            case Directions.East:
                ChangeAnimState(90, false);
                break;
            case Directions.West:
                ChangeAnimState(270, true);
                break;
            default:
                ChangeAnimState(0, false);
                break;
        }
    }    

    private void ChangeAnimState(int zRotation, bool isFlipX)
    {
        spriteRenderer.flipX = isFlipX;
        transform.eulerAngles = new Vector3(0, 0, zRotation);

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
