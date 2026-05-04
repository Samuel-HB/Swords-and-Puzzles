using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    private List<Image> itemsImages;
    [SerializeField] private Image arrow;
    [SerializeField] private Image bomb;
    [SerializeField] private Image key;

    private List<Vector2> itemPosition;
    private Vector2 pos1 = new Vector2(-700, 400);
    private Vector2 pos2 = new Vector2(-625, 400);
    private Vector2 pos3 = new Vector2(-800, 400);


    private void Start()
    {
        EventManager.updatingItems += UpdateItems;

        itemsImages = new List<Image>() { arrow, bomb, key };
        itemPosition = new List<Vector2>() { pos1, pos2, pos3 };

        foreach (Image image in itemsImages ) {
            //image.enabled = false;
            image.enabled = true;
        }
    }

    private void UpdateItemsImage()
    {
        foreach (Image image in itemsImages)        {
            image.enabled = false;
        }
        //for (int i = 0; i < inventory.items.Count; i++) {
        //    itemsImages[i].enabled = true;
        //}

        // how to associate to the items their images and made a real algorthm
        // without breaking the rule of separate gameplay and UI ?

        if (inventory.items.Contains(inventory.key)) {
            key.enabled = true;
        }
        if (inventory.items.Contains(inventory.bow)) {
            arrow.enabled = true;
        }
        if (inventory.items.Contains(inventory.bomb)) {
            bomb.enabled = true;
        }
    }

    private void UpdateItems()
    {
        UpdateItemsImage();
        UpdateItemsPositions();
    }

    public void UpdateItemsPositions()
    {
        switch (inventory.items.Count)
        {
            case 1:
                pos1 = new Vector2(-700, 400);
                break;
            case 2:
                pos2 = new Vector2(-625, 400);
                pos1 = new Vector2(-750, 400);
                break;
            case 3:
                pos3 = new Vector2(-800, 400);
                pos1 = new Vector2(-700, 400);
                pos2 = new Vector2(-600, 400);
                break;
            default:
                break;
        }
        ShiftItemImagePositions();
    }

    public void ShiftItemImagePositions()
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            itemsImages[i].rectTransform.anchoredPosition = itemPosition[i];
        }
    }

    private void OnDestroy()
    {
        EventManager.updatingItems -= UpdateItems;
    }
}
