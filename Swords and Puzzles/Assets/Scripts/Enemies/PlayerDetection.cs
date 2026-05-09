using UnityEngine;

public class PlayerDetection : Ennemy
{
    protected Transform transformDetected;

    [SerializeField] private float radius = 10f;
    protected int playerLayerMask = 0;
    protected int enemyLayerMask = 0;

    // use offset because raycast goes in direction of the player center, but needs to get contact
    // with it's collider, which is at his foots so under the game object center
    private float yOffset = -0.375f;


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.25f, 0.25f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    
    protected void SetPlayerLayerMask()
    {
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        enemyLayerMask = 1 << LayerMask.NameToLayer("Ennemy");
    }

    protected bool TryToDetectPlayer()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius, playerLayerMask);

        if (hitCollider != null)
        {
            transformDetected = hitCollider.transform;
            return TryToDetectPlayerByRaycast(transformDetected.position) ? true : false;
        }
        return false;
    }

    private bool TryToDetectPlayerByRaycast(Vector3 targetPosition)
    {
        targetPosition = new Vector2(targetPosition.x, targetPosition.y + yOffset);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, targetPosition - transform.position, radius, ~enemyLayerMask);

        if (ray.collider != null) {
            return (ray.collider.GetComponent<Player>() != null) ? true : false;
        }
        return false;
    }
}
