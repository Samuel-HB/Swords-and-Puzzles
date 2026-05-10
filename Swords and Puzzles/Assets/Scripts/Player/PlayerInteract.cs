using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float radius = 1.5f;
    private int playerLayerMask = 0;
    private int ignoreRaycastLayerMask = 0;

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
        ignoreRaycastLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
    }

    public void OnInteract()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, ~playerLayerMask & ~ignoreRaycastLayerMask);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider != null && collider.TryGetComponent<IInteractable>(out IInteractable iInteractable)) {
                iInteractable.Interact(inventory);
            }
        }
        // avoid overlap with himself and sword collider
        //Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius, ~playerLayerMask & ~ignoreRaycastLayerMask);
        //if (hitCollider != null)
        //{
        //    print("try interact");
        //    print(hitCollider);
        //    if (hitCollider.TryGetComponent<IInteractable>(out IInteractable iInteractable)) {
        //        iInteractable.Interact(inventory);
        //    }
        //}
    }
}
