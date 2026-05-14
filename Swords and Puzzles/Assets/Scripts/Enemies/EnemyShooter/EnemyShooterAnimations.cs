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

    private Directions GetDirectionInRangeOfFour(Vector3 direction)
    {
        direction = (Quaternion.AngleAxis(45, Vector3.forward) * direction).normalized;

        if (direction.x < 0 && direction.y > 0) {
            return Directions.North;
        }
        else if (direction.x > 0 && direction.y < 0) {
            return Directions.South;
        }
        else if (direction.x > 0 && direction.y > 0) {
            return Directions.East;
        }
        else if (direction.x < 0 && direction.y < 0) {
            return Directions.West;
        }
        else {
            return Directions.North;
        }
    }

    private void ChangeAnimState(string newState, bool isFlipX)
    {
        spriteRenderer.flipX = isFlipX;
        childrenSpriteRenderer.flipX = isFlipX;
        animator.CrossFadeInFixedTime(newState, 0f);
    }

    private void OnDestroy()
    {
        EventManager.shootingOnPlayer -= ChoseSwordAttackDirection;
    }
}
