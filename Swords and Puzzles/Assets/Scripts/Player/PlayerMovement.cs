using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Player player;

    private Vector2 currentInputValue = new Vector3();
    private Vector3 movementDirection = new Vector3();
    private Vector3 desiredMovementDirection = Vector2.zero;
    private Vector3 lastPosition = Vector3.zero;
    private float speed = 5f;

    private Vector2 boxCornerLeftUp = new Vector2(-0.3125f, -0.125f);
    private Vector2 boxCornerRightDown = new Vector2(0.3125f, -0.625f);
    private int wallLayerMask = 0;
    private int wallOnlyForPlayerLayerMask = 0;


    private void Start()
    {
        EventManager.firingArrow += StopMovementTemporarly;
        EventManager.throwingBomb += StopMovementTemporarly;
        EventManager.swordAttacking += StopMovementTemporarly;
        EventManager.goingBackToIdle += RegainMovement;

        player = GetComponent<Player>();

        wallLayerMask = 1 << LayerMask.NameToLayer("Default");
        wallOnlyForPlayerLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
    }

    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        currentInputValue = context.ReadValue<Vector2>().normalized;
        desiredMovementDirection = GetDirection(currentInputValue);

        switch (player.state)
        {
            case PlayerState.Idle:
                OnMove(context);
                break;
            default:
                break;
        }
    }

    private Vector2 GetDirection(Vector2 inputValue)
    {
        Vector2 finalDirection = Vector2.zero;

        if (inputValue.y > 0.01f){
            finalDirection = new Vector2(0, 1);
            player.direction = Directions.North;
        }
        else if (inputValue.y < -0.01f) {
            finalDirection = new Vector2(0, -1);
            player.direction = Directions.South;
        }
        else if (inputValue.x > 0.01f) {
            finalDirection = new Vector2(1, 0);
            player.direction = Directions.East;
        }
        else if (inputValue.x < -0.01f) {
            finalDirection = new Vector2(-1, 0);
            player.direction = Directions.West;
        }
        return finalDirection;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movementDirection = desiredMovementDirection;
        EventManager.PlayerMove();
    }

    public void OnMoveCanceled()
    {
        desiredMovementDirection = Vector2.zero;

        movementDirection = Vector2.zero;
        EventManager.PlayerMoveStop();
    }

    private void StopMovementTemporarly()
    {
        movementDirection = Vector2.zero;
    }

    private void RegainMovement()
    {
        movementDirection = desiredMovementDirection;
    }


    private void Update()
    {
        transform.position += movementDirection * speed * Time.deltaTime;

        Collider2D wallCollider = Physics2D.OverlapArea((Vector2)transform.position + boxCornerLeftUp,
                                                        (Vector2)transform.position + boxCornerRightDown,
                                                        wallLayerMask);
        if (wallCollider != null)
        {
            if (wallCollider.TryGetComponent<WallTile>(out WallTile wallTile))
            {
                Vector3 difference = transform.position - lastPosition;
                transform.position -= difference;
            }
        }
        lastPosition = transform.position;
    }

    private void OnDestroy()
    {
        EventManager.firingArrow -= StopMovementTemporarly;
        EventManager.throwingBomb -= StopMovementTemporarly;
        EventManager.swordAttacking -= StopMovementTemporarly;
        EventManager.goingBackToIdle -= RegainMovement;
    }
}
