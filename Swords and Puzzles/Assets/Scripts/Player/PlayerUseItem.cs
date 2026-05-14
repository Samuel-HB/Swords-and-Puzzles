using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    private Player player;
    private Inventory inv;

    private void Start()
    {
        player = GetComponent<Player>(); 
        inv = GetComponent<Inventory>();
    }

    public void UseItem()
    {
        if (inv.items.Count > 0 && player.state != PlayerState.UsingItem) {
            inv.items[0].UseItem();
        }
    }

    public void ShiftItemLeft()
    {
        if (inv.items.Count < 2) return;

        IUsable tempItem = inv.items[0];

        for (int i = 0; i < inv.items.Count - 1; i++) {
            inv.items[i] = inv.items[i + 1];
        }
        inv.items[inv.items.Count - 1] = tempItem;

        EventManager.UpdateItems();
    }

    public void ShiftItemRight()
    {
        if (inv.items.Count < 2) return;

        IUsable tempItem = inv.items[inv.items.Count - 1];

        for (int i = inv.items.Count - 1; i > 0; i--) {
            inv.items[i] = inv.items[i - 1];
        }
        inv.items[0] = tempItem;

        EventManager.UpdateItems();
    }
}
