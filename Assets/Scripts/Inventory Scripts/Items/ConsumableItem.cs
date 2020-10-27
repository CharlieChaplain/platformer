using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Inventory System/Items/Consumable")]
public class ConsumableItem : ItemTemplate
{
    public float restoreHP;
    public void Awake()
    {
        type = ItemType.Consumable;
    }
}
