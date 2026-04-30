using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Player player;

    public InputActionReference moveAction;
    private Vector2 currentInputValue;
    private Vector3 movementDirection;
    private float speed = 5f;

    private Vector2 boxLength = new Vector2(0.95f, 0.95f);
    private Vector3 lastPosition = Vector3.zero;
    private int wallLayerMask = 0;
    private int wallOnlyForPlayerLayerMask = 0;


    private void OnEnable()
    {
        moveAction.action.performed += OnMoveActionPerformed;
        moveAction.action.canceled += OnMoveActionCanceled;
        moveAction.action.Enable();
    }

    private void OnMoveActionPerformed(InputAction.CallbackContext context)
    {
        currentInputValue = context.ReadValue<Vector2>().normalized;
        movementDirection = GetDirection(currentInputValue);
    }

    private void OnMoveActionCanceled(InputAction.CallbackContext context)
    {
        movementDirection = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, boxLength);
    }

    private void Start()
    {
        player = GetComponent<Player>();

        wallLayerMask = 1 << LayerMask.NameToLayer("Default");
        wallOnlyForPlayerLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
    }

    private void Update()
    {
        transform.position += movementDirection * speed * Time.deltaTime;
        //transform.position = new Vector3(((int)transform.position.x)/4, ((int)transform.position.y)/4);

                                                                                  // check all layers wall tiles can have in project
        Collider2D wallCollider = Physics2D.OverlapBox(transform.position, boxLength, 0f, wallLayerMask | wallOnlyForPlayerLayerMask);
        if (wallCollider != null)
        {
            if (wallCollider.TryGetComponent<WallTile>(out WallTile wallTile))
            {
                Vector3 difference = transform.position - lastPosition;
                //difference = new Vector3((int)difference.x, (int)difference.y);
                //transform.position -= difference * speed * Time.deltaTime;
                transform.position -= difference;
            }
        }
        lastPosition = transform.position;
        //lastPosition = new Vector3((int)transform.position.x, (int)transform.position.y);
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
        //else {
        //    finalDirection = Vector2.zero;
        //}
        return finalDirection;
    }

    private void OnDisable()
    {
        moveAction.action.performed -= OnMoveActionPerformed;
        moveAction.action.canceled -= OnMoveActionCanceled;
        moveAction.action.Disable();
    }
}
