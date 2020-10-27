using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory System/Items/Equipment")]
public class EquipmentItem : ItemTemplate
{
    public float attackBonus;
    public float defenseBonus;
    public float speedBonus;
    public float jumpBonus;
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
