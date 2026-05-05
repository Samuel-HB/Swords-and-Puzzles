using UnityEngine;

public class ArrowCollectible : MonoBehaviour, IPickable
{
    public void PickItem(Inventory inv)
    {
        inv.AddItem(ref inv.arrowsCount, inv.bow);
        Destroy(gameObject);
    }
}
