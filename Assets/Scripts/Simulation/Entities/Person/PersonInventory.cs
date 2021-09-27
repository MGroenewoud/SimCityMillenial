using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersonInventory : MonoBehaviour
{
    public Dictionary<ItemType, InventoryItem> Inventory;
    public bool BagIsFull => Inventory.Sum(i => i.Value.Amount * i.Value.Weight) >= _person.InventoryLimit;

    private Person _person;

    private void Awake()
    {
        _person = GetComponent<Person>();
        Inventory = new Dictionary<ItemType, InventoryItem>();
    }

    public void AddItem(ItemType type, int amount)
    {
        if (Inventory.ContainsKey(type))
        {
            var items = Inventory[type];
            items.Amount += amount;
            Inventory[type] = items;
        } else
        {
            Inventory.Add(type, new InventoryItem() { Amount = amount, Weight = 1 });
        }
    }

    public int RemoveItem(ItemType type)
    {
        var amount = 0;
        if (Inventory.ContainsKey(type))
        {
            var items = Inventory[type];
            amount = items.Amount;
            items.Amount = 0;
            Inventory[type] = items;
        }

        return amount;
    }
}

public struct InventoryItem
{
    public int Amount;
    public int Weight;
}

public enum ItemType
{
    // Raw resources
    Wood,
}