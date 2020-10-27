using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Use this class to give an actor an inventory. Just add this class to the actor in question
public class InventoryAnchor : MonoBehaviour
{
    public Inventory inventory; //change this to the scriptable object of the correct inventory

    public void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<ItemAnchor>();
        if (item)
        {
            inventory.AddItem(item.item, 1);
            other.GetComponent<ItemAnchor>().Collected();
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
