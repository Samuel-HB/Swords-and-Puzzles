using UnityEngine;

public class Door : MonoBehaviour
{
    public void OpenDoor(Inventory inv)
    {
        if (inv.keysCount > 0)
        {
            inv.RemoveItem(ref inv.keysCount, inv.key);
            Destroy(gameObject);
        }
    }
}
