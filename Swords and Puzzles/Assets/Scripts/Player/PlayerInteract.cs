using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float radius = 1.5f;
    private int playerLayerMask = 0;

    private Inventory inventory;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.25f, 0.25f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        inventory = GetComponent<Inventory>();

        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
    }

    public void OnInteract()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius, ~playerLayerMask);
        if (hitCollider != null)
        {
            if (hitCollider.TryGetComponent<IPickable>(out IPickable iPickable)) {
                iPickable.PickItem(inventory);
            }
        }
    }
}
