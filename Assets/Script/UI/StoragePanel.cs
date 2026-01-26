using System.Collections.Generic;
using UnityEngine;

public class StoragePanel : MonoBehaviour
{
    [SerializeField] private Transform materialItemSlotsParnet;
    [SerializeField] private Transform cubeItemSlotsParent;

    private InventoryStorage currentStorage; // 当前操作的InventoryStorage对象
    private SlotUI[] materialItemSlots;
    private SlotUI[] cubeItemSlots;
    public UI_ItemToolTip itemToolTip {  get; private set; }

    private void Awake()
    {
        materialItemSlots=materialItemSlotsParnet.GetComponentsInChildren<SlotUI>();
        cubeItemSlots=cubeItemSlotsParent.GetComponentsInChildren<SlotUI>();
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        enabled=gameObject.activeSelf;
    }

    private void OnEnable()
    {
        UpdateStoragePanel();
    }



    private void UpdateStoragePanel()
    {
        UpdateMaterialItemSlots();
        UpdateCubeItemSlots();
    }

    private void UpdateMaterialItemSlots()
    {
        List<InventoryItemSlot> materialItemSlotList=currentStorage.materialItemSlotList;
        for(int i=0; i<materialItemSlots.Length; i++)
        {
            InventoryItemSlot materialSlot=materialItemSlotList[i]; 
            if(materialSlot.HasItem()) // 如果该槽位有物品
            {
                materialItemSlots[i].UpdateSlot(materialSlot.itemInSlot); // 显示物品
            }
            else
            {
                materialItemSlots[i].UpdateSlot(null); // 显示为空
            }
        }
    }

    private void UpdateCubeItemSlots()
    {
        List<InventoryItemSlot> cubeItemSlotList=currentStorage.cubeItemSlotList;
        for( int i=0;i<cubeItemSlots.Length;i++)
        {
            InventoryItemSlot cubeSlot=cubeItemSlotList[i];
            if(cubeSlot.HasItem())
            {
                cubeItemSlots[i].UpdateSlot(cubeSlot.itemInSlot);
            }
            else
            {
                cubeItemSlots[i].UpdateSlot(null);
            }
        }
    }

    public void SetCurrentStorage(InventoryStorage storage)
    {
        currentStorage = storage;
    }

    public void ResetCurrentStorage()
    {
        currentStorage = null;
    }

}
