using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    //[SerializeField] private Inventory inventory; // needs drag and drop
    private Inventory inventory; // needs drag and drop

    private List<Image> itemsImages;
    [SerializeField] private Image arrow;
    [SerializeField] private Image bomb;
    [SerializeField] private Image key;

    private List<Vector2> itemPosition;
    private Vector2 pos1 = new Vector2(-680, 410);
    private Vector2 pos2 = new Vector2(-540, 410);
    private Vector2 pos3 = new Vector2(-820, 410);


    private void Start()
    {
        EventManager.updatingItems += UpdateItems;

        inventory = Locator.player.GetComponent<Inventory>();

        itemsImages = new List<Image>() { arrow, bomb, key };
        itemPosition = new List<Vector2>() { pos1, pos2, pos3 };

        // new
        UpdateItems();
    }

    private void UpdateItems()
    {
        UpdateItemsImages();
        UpdateItemsPositions();
    }

    private void UpdateItemsImages()
    {
        foreach (Image image in itemsImages ) { 
            image.enabled = false;
        }
        itemsImages.Clear();

        for (int i = 0; i < inventory.items.Count; i++) {
            itemsImages.Add(null);
        }

        CheckIfContainsItem(inventory.key, key);
        CheckIfContainsItem(inventory.bow, arrow);
        CheckIfContainsItem(inventory.bomb, bomb);
    }

    private void CheckIfContainsItem(IUsable item, Image itemImage)
    {
        if (inventory.items.Contains(item))
        {
            itemsImages[inventory.items.IndexOf(item)] = itemImage;
            itemsImages[inventory.items.IndexOf(item)].enabled = true;
        }
    }

    public void UpdateItemsPositions()
    {
        switch (inventory.items.Count)
        {
            case 1:
                itemPosition[0] = new Vector2(-680, 410);
                break;
            case 2:
                itemPosition[0] = new Vector2(-680, 410);
                itemPosition[1] = new Vector2(-820, 410);
                break;
            case 3:
                itemPosition[0] = new Vector2(-680, 410);
                itemPosition[1] = new Vector2(-540, 410);
                itemPosition[2] = new Vector2(-820, 410);
                break;
            default:
                break;
        }
        for (int i = 0; i < inventory.items.Count; i++) {
            itemsImages[i].rectTransform.anchoredPosition = itemPosition[i];
        }
    }

    private void OnDestroy()
    {
        EventManager.updatingItems -= UpdateItems;
    }
}
