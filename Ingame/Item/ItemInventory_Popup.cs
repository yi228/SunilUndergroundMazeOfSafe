using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory_Popup : UI_Popup
{
    public static bool inventoryActivated = false;
    [SerializeField]
    private GameObject go_InventoryBase;
    private Canvas canvas;
    //[SerializeField]
    //private GameObject go_SlotsParent;
    //슬롯들
    public Slot[] slots;
    private void Start()
    {
        if (GetComponent<Canvas>() != null) //터치 안되는 에러 수정 방법
        {
            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }
        
    }
    public void openInventory()
    {
        inventoryActivated = !inventoryActivated;
        if (inventoryActivated)
        {
            go_InventoryBase.SetActive(true);
        }
        else
        {
            go_InventoryBase.SetActive(false);
        }
    }
    public void AcquireItem(Items _item, int _count)
    {
        Managers.Sound.Play("Effect/Item/GetItem");
        if (Items.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {

                    slots[i].AddItem(_item, _count);
                    return;
                    
                }
            }
        }
    }

}
