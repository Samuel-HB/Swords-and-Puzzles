using UnityEngine;

public class PlayerHurtBox : MonoBehaviour
{
    private Vector2 boxLength = new Vector2(0.5f, 0.5f);
    private int wallLayerMask = 0;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, boxLength);
    }

    private void Start()
    {
        wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
    }

    private void FixedUpdate()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        Collider2D wallCollider = Physics2D.OverlapBox(transform.position, boxLength, 0f, wallLayerMask);
        if (wallCollider != null)
        {
            print("collide with wall");
        }
    }
}
