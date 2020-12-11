using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Use this class to give an actor an inventory. Just add this class to the actor in question
public class InventoryAnchor : MonoBehaviour
{
    public List<Inventory> inventories; //change this to the scriptable object of all inventories

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<ItemAnchor>();
        if (item)
        {
            switch (item.item.type)
            {
                case ItemType.Consumable:
                    inventories[0].AddItem(new Item(item.item), 1);
                    break;
                case ItemType.Key:
                    inventories[1].AddItem(new Item(item.item), 1);
                    break;
                default:
                    break;

            }
            other.GetComponent<ItemAnchor>().Collected();
        }
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.O))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            inventory.Load();
        }
        */
    }

    private void OnApplicationQuit()
    {
        foreach (Inventory inv in inventories)
        {
            inv.Container.Items.Clear();
        }
    }
}
