using UnityEngine;

public class ArrowCollectible : MonoBehaviour, IInteractable
{
    public void Interact(Inventory inv)
    {
        inv.AddItem(ref inv.arrowsCount, inv.bow);
        Destroy(gameObject);
    }
}
