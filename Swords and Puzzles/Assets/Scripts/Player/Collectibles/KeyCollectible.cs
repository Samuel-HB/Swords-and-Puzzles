using UnityEngine;

public class KeyCollectible : MonoBehaviour, IInteractable
{
    public void Interact(Inventory inv)
    {
        inv.AddItem(ref inv.keysCount, inv.key);
        Destroy(gameObject);
    }
}
