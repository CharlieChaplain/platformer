using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject itemInvPrefab;
    public TextMeshProUGUI subMenuTitle;
    public Inventory inventory;
    int currentInvIndex = 0;

    public int DELTA_X_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMNS;
    public int DELTA_Y_BETWEEN_ITEM;
    public float X_START;
    public float Y_START;
    Dictionary<InventorySlot, GameObject> displayedItems = new Dictionary<InventorySlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            Debug.Log(slot.item.Name);

            var obj = Instantiate(itemInvPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.ID].UIDisplay;
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            if (slot.item.stackable)
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            else
                obj.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

            displayedItems.Add(slot, obj);
        }
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];

            if (displayedItems.ContainsKey(slot))
            {
                displayedItems[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(itemInvPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.ID].UIDisplay;
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                if (slot.item.stackable)
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
                else
                    obj.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                displayedItems.Add(slot, obj);
            }
        }
    }

    public void ClearDisplay()
    {
        foreach (KeyValuePair <InventorySlot, GameObject> kvp in displayedItems){
            Destroy(kvp.Value);
        }

        displayedItems.Clear();
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (DELTA_X_BETWEEN_ITEM * (i % NUMBER_OF_COLUMNS)), Y_START +
            (-DELTA_Y_BETWEEN_ITEM * (i / NUMBER_OF_COLUMNS)), 0);
    }

    public void ChangeInventories(int direction)
    {
        InventoryAnchor playerInvAnchor = PlayerManager.Instance.inventoryAnchor;
        currentInvIndex += direction;
        if (currentInvIndex >= playerInvAnchor.inventories.Count)
            currentInvIndex = 0;
        else if (currentInvIndex < 0)
            currentInvIndex = playerInvAnchor.inventories.Count - 1;
        inventory = playerInvAnchor.inventories[currentInvIndex];

        subMenuTitle.text = inventory.name;

        ClearDisplay();
    }
}
