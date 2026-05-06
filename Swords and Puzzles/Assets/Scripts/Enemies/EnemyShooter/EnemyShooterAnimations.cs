using UnityEngine;

public class EnemyShooterAnimations : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer childrenSpriteRenderer;
    private EnnemyShooter ennemyShooter;

    private string shooterIdleDown = "ShooterIdleDown";
    private string shooterIdleUp = "ShooterIdleUp";
    private string shooterIdleRight = "ShooterIdleRight";


    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ennemyShooter = GetComponent<EnnemyShooter>();

        EventManager.shootingOnPlayer += ChoseSwordAttackDirection;
    }

    public void ChoseSwordAttackDirection()
    {
        Directions directionTowardsTarget = GetDirectionInRangeOfFour(ennemyShooter.shotDirection);
        switch (directionTowardsTarget)
        {
            case Directions.North:
                ChangeAnimState(shooterIdleUp, false);
                break;
            case Directions.South:
                ChangeAnimState(shooterIdleDown, false);
                break;
            case Directions.East:
                ChangeAnimState(shooterIdleRight, false);
                break;
            case Directions.West:
                ChangeAnimState(shooterIdleRight, true);
                break;
            default:
                ChangeAnimState(shooterIdleUp, false);
                break;
        }
    }    

    private void ChangeAnimState(string newState, bool isFlipX)
    {
        spriteRenderer.flipX = isFlipX;
        childrenSpriteRenderer.flipX = isFlipX;
        animator.CrossFadeInFixedTime(newState, 0f);
    }

    private Directions GetDirectionInRangeOfFour(Vector3 direction)
    {
        if (direction.x > -0.5f && direction.x < 0.5f && direction.y > 0) {
            return Directions.North;
        }
        if (direction.x > -0.5f && direction.x < 0.5f && direction.y < 0) {
            return Directions.South;
        }
        if (direction.x < 0) {
            return Directions.West;
        }
        if (direction.x > 0) {
            return Directions.East;
        }

        return Directions.North;
    }

    private void OnDestroy()
    {
        EventManager.shootingOnPlayer -= ChoseSwordAttackDirection;
    }
}
