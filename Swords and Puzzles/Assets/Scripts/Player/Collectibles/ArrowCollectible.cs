using UnityEngine;

public class ArrowCollectible : MonoBehaviour, IPickable
{
    public void PickItem(Inventory inventory)
    {
        inventory.AddItem(inventory.arrowsCount, inventory.bow);
        Destroy(gameObject);
    }
}
