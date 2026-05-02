using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private Player player;
    private Bow bow;

    private string currentState = "IdleDown";

    private string idleDown = "IdleDown";
    private string idleUp = "IdleUp";
    private string idleLeft = "IdleLeft";
    private string idleRight = "IdleRight";
    private string walkDown = "WalkDown";
    private string walkUp = "WalkUp";
    private string walkLeft = "WalkLeft";
    private string walkRight = "WalkRight";

    private string firingArrowDown = "FireArrowDown";
    private string firingArrowUp = "FireArrowUp";
    private string firingArrowLeft = "FireArrowLeft";
    private string firingArrowRight = "FireArrowRight";

    private string throwingBombDown = "ThrowBombDown";
    private string throwingBombUp = "ThrowBombUp";
    private string throwingBombLeft = "ThrowBombLeft";
    private string throwingBombRight = "ThrowBombRight";


    private void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        if (TryGetComponent<Bow>(out Bow b)) {
            bow = b;
        }
        bow.firingArrow += GetBow;
    }

    private void ChangeAnimState(string newState)
    {
        if (currentState == newState) return;

        //animator.Play(newState);
        animator.CrossFadeInFixedTime(newState, 0.2f);
        currentState = newState;
    }

    public void GetBow()
    {
        switch (player.direction)
        {
            case Directions.North:
                ChangeAnimState(firingArrowUp);
                break;
            case Directions.South:
                ChangeAnimState(firingArrowDown);
                break;
            case Directions.East:
                ChangeAnimState(firingArrowLeft);
                break;
            case Directions.West:
                ChangeAnimState(firingArrowRight);
                break;
            default:
                ChangeAnimState(firingArrowUp);
                break;
        }
    }

    private void OnDestroy()
    {
        bow.firingArrow -= GetBow;
    }
}
