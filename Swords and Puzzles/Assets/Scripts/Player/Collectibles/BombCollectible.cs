using UnityEngine;

public class BombCollectible : MonoBehaviour, IInteractable
{
    public void Interact(Inventory inv)
    {
        inv.AddItem(ref inv.bombsCount, inv.bomb);
        Destroy(gameObject);
    }
}
