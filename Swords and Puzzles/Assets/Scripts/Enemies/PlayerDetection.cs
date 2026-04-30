using UnityEngine;

public class PlayerDetection : Ennemy
{

    protected Transform transformDetected;

    [SerializeField] private float radius = 10f;
    private int playerLayerMask = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.25f, 0.25f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    protected void SetPlayerLayerMask()
    {
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
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
        RaycastHit2D ray = Physics2D.Raycast(transform.position, targetPosition - transform.position);
        if (ray.collider != null)
        {
            return ray.collider.TryGetComponent<Player>(out Player player) ? true : false;
        }
        else {
            return false;
        }
    }
}
