using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [Header("   References")]
    public Image gridCellPrefab;
    public RectTransform containerRectTransform;
    public GridLayoutGroup gridCellParent;

    [Header("   Layout Data")]
    public RectOffset margins;
    public float cellSize;
    public float cellSpacing;

    [Header("   Items")]
    public Image itemPrefab;
    public RectTransform itemParent;


    private void Awake()
    {
        Inventory.onInventoryOpen += OnOpen;
    }

    private void OnOpen(Inventory inventory)
    {
        containerRectTransform.gameObject.SetActive(true);

        for (int i = 0; i < inventory.size.x; i++) {
            for (int j = 0; i < inventory.size.y; j++)
            {
                Instantiate(gridCellPrefab, gridCellParent.transform);
            }
        }
        Vector2 gridTotalSize = new Vector2(
            inventory.size.x * cellSize + (inventory.size.x - 1) * cellSpacing,
            inventory.size.y * cellSize + (inventory.size.y - 1) * cellSpacing);

        gridCellParent.cellSize = Vector2.one * cellSize;
        gridCellParent.spacing = Vector2.one * cellSpacing;
        gridCellParent.constraintCount = inventory.size.x;

        containerRectTransform.sizeDelta = gridTotalSize + new Vector2(margins.left + margins.right, margins.top + margins.bottom);

        RectTransform parentRectTransform = (RectTransform)gridCellParent.transform;

        parentRectTransform.sizeDelta = gridTotalSize;
        parentRectTransform.anchoredPosition = new Vector2(margins.left, -margins.top);

        itemParent.sizeDelta = parentRectTransform.sizeDelta;
        itemParent.anchoredPosition = parentRectTransform.anchoredPosition;

        PlaceItems(inventory);
    }

    private void PlaceItems(Inventory inventory)
    {
        foreach ((IItem item, Vector2Int position) in inventory)
        {
            Image itemInstance = Instantiate(itemPrefab, itemParent);

            itemInstance.sprite = item.Sprite;

            itemInstance.rectTransform.sizeDelta = new Vector2(
                item.ItemSize.x * cellSize + (item.ItemSize.x - 1) * cellSpacing,
                item.ItemSize.y * cellSize + (item.ItemSize.y - 1) * cellSpacing
                );

            itemInstance.rectTransform.anchoredPosition = new Vector2(
                position.x * (cellSize + cellSpacing),
                position.y * (cellSize + cellSpacing)
                );

            itemInstance.rectTransform.anchoredPosition +=
                Vector2.Scale(new Vector2(1, -1), itemInstance.rectTransform.sizeDelta / 2);
        }
    }

    private void OnDestroy()
    {
        Inventory.onInventoryOpen -= OnOpen;
    }
}
