using UnityEngine;

public class Key : MonoBehaviour, IUsable
{
    private Inventory inventory;

    [SerializeField] private float radius = 1.5f;
    private int playerLayerMask = 0;
    private int ignoreRaycastLayerMask = 0;


    private void Start()
    {
        inventory = GetComponent<Inventory>();

        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        ignoreRaycastLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
    }

    public void UseItem()
    {        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, ~playerLayerMask & ~ignoreRaycastLayerMask);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider != null && collider.TryGetComponent<Door>(out Door door))
            {
                print("interact");
                door.OpenDoor(inventory);
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
