using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject itemInvPrefab;
    public Inventory inventory;

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

            var obj = Instantiate(itemInvPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.ID].UIDisplay;
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");

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
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");

                displayedItems.Add(slot, obj);
            }
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (DELTA_X_BETWEEN_ITEM * (i % NUMBER_OF_COLUMNS)), Y_START +
            (-DELTA_Y_BETWEEN_ITEM * (i / NUMBER_OF_COLUMNS)), 0);
    }
}
