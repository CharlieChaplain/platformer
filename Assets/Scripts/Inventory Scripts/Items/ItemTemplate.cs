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
    public bool stackable;
    public int maxStackSize;
    public GameObject prefab;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
}
