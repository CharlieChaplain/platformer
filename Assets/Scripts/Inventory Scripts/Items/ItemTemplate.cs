using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Key,
    Default
}

public class ItemTemplate : ScriptableObject
{
    public int ID;
    public bool stackable;
    public int maxStackSize;
    public Sprite UIDisplay;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
}

[System.Serializable]
public class Item
{
    public string Name;
    public int ID;
    public bool stackable;

    public Item(ItemTemplate item)
    {
        Name = item.name;
        ID = item.ID;
        stackable = item.stackable;
    }
}
