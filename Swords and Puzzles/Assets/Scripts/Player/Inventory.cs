using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<IUsable> items;
    public Key key;
    public Bow bow;
    public Bomb bomb;

    [NonSerialized] public bool hasKey = false;
    [NonSerialized] public int arrowsCount = 3;
    [NonSerialized] public int bombsCount = 3;


    private void Start()
    {
        items = new List<IUsable>() { key, bomb, bow };
    }

    public void AddItem(ref int countToIncrease, IUsable itemToAdd)
    {
        countToIncrease++;
        if (!items.Contains(itemToAdd)) {
            items.Add(itemToAdd);
        }
        EventManager.UpdateItems();
    }

    public void RemoveItem(ref int countToIncrease, IUsable item)
    {
        countToIncrease--;
        if (countToIncrease <= 0) {
            items.Remove(item);
        }
        EventManager.UpdateItems();
    }
}
