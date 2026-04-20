using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionReference moveAction;

    private Vector2 direction = Vector2.zero;
    private float speed = 5f;

    private Vector2 currentInputValue;
    //private Vector2 movementDirection;
    private Vector3 movementDirection;
    private Vector3 lastPosition = Vector3.zero;

    private Vector2 boxLength = new Vector2(1, 1);
    private int wallLayerMask = 0;


    private void OnEnable()
    {
        moveAction.action.performed += OnMoveActionPerformed;
        moveAction.action.canceled += OnMoveActionCanceled;
        moveAction.action.Enable();
    }

    private void OnMoveActionPerformed(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();

        //new
        currentInputValue = context.ReadValue<Vector2>().normalized;
        movementDirection = GetDirection(currentInputValue);
    }

    private void OnMoveActionCanceled(InputAction.CallbackContext context)
    {
        //direction = Vector2.zero;

        //new
        movementDirection = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, boxLength);
    }

    private void Start()
    {
        wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
    }

    private void Update()
    {
        //transform.Translate(direction.normalized * speed * Time.deltaTime);

        //new
        //transform.Translate(movementDirection * speed * Time.deltaTime);

        transform.position += movementDirection * speed * Time.deltaTime;

        Collider2D wallCollider = Physics2D.OverlapBox(transform.position, boxLength, 0f, wallLayerMask);
        if (wallCollider != null)
        {
            Vector3 difference = transform.position - lastPosition;
            //transform.position -= difference * speed * Time.deltaTime;
            transform.position -= difference;
        }
        lastPosition = transform.position;

        //Transform finalPosition;

        //Vector3 endPos = Vector3.zero;

        //if (!isMoving)
        //    endPos = transform.position + new Vector3(5, 0, 0); 

        //if (isMoving)
        //{
        //    //Vector3 directionToGo = transform.position + movementDirection;
        //    transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime);
        //}

        //rb.linearVelocity = movementDirection * 1000 * Time.deltaTime;
    }

    private Vector2 GetDirection(Vector2 inputValue)
    {
        Vector2 finalDirection = Vector2.zero;

        if (inputValue.y > 0.01f){
            finalDirection = new Vector2(0, 1);
        }
        else if (inputValue.y < -0.01f) {
            finalDirection = new Vector2(0, -1);
        }
        else if (inputValue.x > 0.01f) {
            finalDirection = new Vector2(1, 0);
        }
        else if (inputValue.x < -0.01f) {
            finalDirection = new Vector2(-1, 0);
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
