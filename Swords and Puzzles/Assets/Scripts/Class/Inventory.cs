using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory : IEnumerable<(IItem, Vector2Int)>
{

    public Vector2Int size;
    [SerializeField] List<InventoryItem> items = new List<InventoryItem>();

    public static event Action<Inventory> onInventoryOpen;

    public void Open()
    {
        onInventoryOpen?.Invoke(this);
    }

    public bool Add(IItem item, Vector2Int position)
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        for (int x = position.x; x < position.x + item.ItemSize.x; x++) {
            if (x >= (size.x - 1)) {
                return false;
            }
            for (int y = position.y; x < position.y + item.ItemSize.y; y++) {
                if (y >= (size.y - 1)) {
                    return false;
                }
                positions.Add(new Vector2Int(x, y));
            }
        }
        if (items.Any(i => i.positions.Intersect(positions).Count() > 0)) {
            return false;
        }
        items.Add(new InventoryItem() { count = 1, item = item, positions = positions, rootPosition = position });
        return true;
    }

    public bool Add(IItem item)
    {
        return false;
    }

    public IEnumerator<(IItem, Vector2Int)> GetEnumerator()
    {
        foreach (InventoryItem iItem in items) {
            yield return (iItem.item, iItem.rootPosition);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();



    [Serializable]
    private class InventoryItem : ISerializationCallbackReceiver
    {
        public IItem item;
        public int count;
        public Vector2Int rootPosition;
        public List<Vector2Int> positions;

        [SerializeField]
        private ScriptableObject serializedItem;

        public void OnBeforeSerialize()
        {
            serializedItem = item as ScriptableObject;
        }
        public void OnAfterDeserialize()
        {
            item = serializedItem as IItem;
        }
    }
}
