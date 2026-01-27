using System.Collections.Generic;
using UnityEngine;

public class StoragePanel : MonoBehaviour
{
    [SerializeField] private Transform materialItemSlotsParnet;
    [SerializeField] private Transform cubeItemSlotsParent;

    public InventoryStorage currentStorage { get; private set; } // 当前操作的InventoryStorage对象
    public PLControl interactPlayer { get; private set; } // 当前查看storage面板的玩家
    private SlotUI[] materialItemSlots;
    private SlotUI[] cubeItemSlots;
    public UI_ItemToolTip itemToolTip { get; private set; }

    private void Awake()
    {
        materialItemSlots = materialItemSlotsParnet.GetComponentsInChildren<SlotUI>();
        cubeItemSlots = cubeItemSlotsParent.GetComponentsInChildren<SlotUI>();
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();

        for (int i = 0; i < materialItemSlots.Length; i++)
        {
            materialItemSlots[i].BindItemIndex(i);
        }
        for (int i = 0; i < cubeItemSlots.Length; i++)
        {
            cubeItemSlots[i].BindItemIndex(i);
        }
    }

    private void OnEnable()
    {
        if (currentStorage != null)
        {
            currentStorage.onStorageChanged += UpdateStoragePanel;
        }

        UpdateStoragePanel();
    }

    private void OnDisable()
    {
        currentStorage.onStorageChanged -= UpdateStoragePanel;
    }



    private void UpdateStoragePanel()
    {
        UpdateMaterialItemSlots();
        UpdateCubeItemSlots();
    }

    private void UpdateMaterialItemSlots()
    {
        List<InventoryItemSlot> materialItemSlotList = currentStorage.materialItemSlotList;
        for (int i = 0; i < materialItemSlots.Length; i++)
        {
            InventoryItemSlot materialSlot = materialItemSlotList[i];
            if (materialSlot.HasItem()) // 如果该槽位有物品
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
        List<InventoryItemSlot> cubeItemSlotList = currentStorage.cubeItemSlotList;
        for (int i = 0; i < cubeItemSlots.Length; i++)
        {
            InventoryItemSlot cubeSlot = cubeItemSlotList[i];
            if (cubeSlot.HasItem())
            {
                cubeItemSlots[i].UpdateSlot(cubeSlot.itemInSlot);
            }
            else
            {
                cubeItemSlots[i].UpdateSlot(null);
            }
        }
    }

    public void SetupStoragePanel(InventoryStorage storage,PLControl player)
    {
        currentStorage = storage;
        interactPlayer = player;
    }

    public void ResetStoragePanel()
    {
        currentStorage = null;
        interactPlayer = null;
    }

}
