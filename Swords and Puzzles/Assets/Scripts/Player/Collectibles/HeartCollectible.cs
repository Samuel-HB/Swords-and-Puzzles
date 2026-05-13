using UnityEngine;

public class HeartCollectible : MonoBehaviour, IInteractable
{
    public void Interact(Inventory inv)
    {
        Locator.player.AddHeart();
        Destroy(gameObject);
    }
}
