using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStorage : MonoBehaviour
{
    [SerializeField] private int maxMaterialSize = 8; // 材料储存容量
    [SerializeField] private int maxCubeSize = 16; // 浮块储存容量
    public List<InventoryItemSlot> materialItemSlotList = new(); // 材料物品槽位列表
    public List<InventoryItemSlot> cubeItemSlotList = new(); // 浮块物品槽位列表
    public event Action onStorageChanged;

    public void AddItem(ItemDataSO itemData, int amount)
    {
        if (amount <= 0 || itemData == null)
        {
            return;
        }


        while (amount > 0)
        {
            // 优先把数量塞进已有堆叠
            var stackSlot = FindStackableItem(itemData); // 查找物品栏中已有的同种物品

            if (stackSlot != null)
            {
                int space = itemData.maxStackSize - stackSlot.itemInSlot.stackSize; // 该槽位剩余可堆叠数量
                int take = Mathf.Min(space, amount); // 填入数量
                stackSlot.itemInSlot.stackSize += take; // 在此格填入该数量
                amount -= take;
            }
            else // 如果没有可堆叠的已有的同种物品
            {
                var empty = FindFirstEmptyItemSlot(itemData); // 查找第一个空格
                if (empty == null)
                {
                    break; // 如果没格子
      
                }
                int take = Mathf.Min(itemData.maxStackSize, amount);

                var newItem = new InventoryItem(itemData); // 新建一条独立的 Inventory_Item
                newItem.stackSize = take;

                empty.itemInSlot = newItem;
                amount -= take;
            }
        }
        onStorageChanged?.Invoke();
    }

    public void DiscardMaterialAtSlot(int slotIndex)
    {
        var slot=materialItemSlotList[slotIndex];
        slot.itemInSlot=null; // 清空该格子
        onStorageChanged?.Invoke();
    }
    
    public void DiscardCubeAtSlot(int slotIndex)
    {
        var slot = cubeItemSlotList[slotIndex];
        slot.itemInSlot = null; // 清空该格子
        onStorageChanged?.Invoke();
    }

    public CubeItemDataSO equipCubeAtSlot(int slotIndex)
    {
        var slot = cubeItemSlotList[slotIndex];
        var cubeData=slot.itemInSlot;
        slot.itemInSlot = null; // 清空该格子
        onStorageChanged?.Invoke();
        return (CubeItemDataSO)cubeData.ItemData;
    }

    private void ConsumeMaterialsOf(ItemDataSO itemToConsume, int requiredAmount)
    {
        if (itemToConsume.itemType != ItemType.material)
        {
            return;
        }
        if (!HasEnoughMaterialsOf(itemToConsume, requiredAmount)) // 如果该材料不足
        {
            return;
        }
        while (requiredAmount > 0)
        {
            InventoryItemSlot avaliableItemSlot = FindStackableItem(itemToConsume);
            // 获取物品栏内一个堆叠未满的该物品槽位
            if (avaliableItemSlot != null)
            {
                int avaliableAmount = avaliableItemSlot.itemInSlot.stackSize; // 当前格中物品剩余数
                if (avaliableAmount <= requiredAmount) // 如果剩余数不足提供所需数或者刚好提供所需数
                {
                    avaliableItemSlot.itemInSlot = null; // 该格物品被全部消耗
                    requiredAmount -= avaliableAmount; // 获得该格提供的消耗所需
                }
                else // 如果剩余数大于所需数
                {
                    avaliableItemSlot.itemInSlot.stackSize -= requiredAmount; // 剩余数消耗掉所需数
                    requiredAmount = 0; // 不再需要消耗
                }
            }
            else
            {
                avaliableItemSlot = FindFullstackableitem(itemToConsume);
                // 如果堆叠未满的物品已用完，则开始消耗满堆叠物品
                int avaliableAmount = avaliableItemSlot.itemInSlot.stackSize; // 当前格中物品剩余数
                if (avaliableAmount <= requiredAmount) // 如果剩余数不足提供所需数或者刚好提供所需数
                {
                    avaliableItemSlot.itemInSlot = null; // 该格物品被全部消耗
                    requiredAmount -= avaliableAmount; // 获得该格提供的消耗所需
                }
                else // 如果剩余数大于所需数
                {
                    avaliableItemSlot.itemInSlot.stackSize -= requiredAmount; // 剩余数消耗掉所需数
                    requiredAmount = 0; // 不再需要消耗
                }
            }
        }
        onStorageChanged?.Invoke();
    }

    public bool HasEnoughMaterialsOf(ItemDataSO itemToConsume, int requiredAmount)
    {
        if (itemToConsume.itemType != ItemType.material)
        {
            return false;
        }

        if (GetAvaliableAmountOf(itemToConsume) < requiredAmount)
        {
            return false;
        }

        return true; // 是否材料足够
    }

    private InventoryItemSlot FindStackableItem(ItemDataSO itemData)
    {

        // 查找列表内该物品的同种且可继续堆叠的物品 
        if (itemData.itemType == ItemType.material)
        {
            return materialItemSlotList.Find(itemSlot => itemSlot.HasItem()
        && itemSlot.itemInSlot.ItemData == itemData
        && itemSlot.itemInSlot.CanAddStack());
        }
        else
        {
            return cubeItemSlotList.Find(itemSlot => itemSlot.HasItem()
&& itemSlot.itemInSlot.ItemData == itemData
&& itemSlot.itemInSlot.CanAddStack());
        }

    }

    private InventoryItemSlot FindFullstackableitem(ItemDataSO itemData)
    {
        // 查找列表内该物品的同种且满堆叠的物品 
        if (itemData.itemType == ItemType.material)
        {
            return materialItemSlotList.Find(itemSlot => itemSlot.HasItem()
&& itemSlot.itemInSlot.ItemData == itemData
&& (!itemSlot.itemInSlot.CanAddStack()));
        }
        else
        {
            return cubeItemSlotList.Find(itemSlot => itemSlot.HasItem()
&& itemSlot.itemInSlot.ItemData == itemData
&& (!itemSlot.itemInSlot.CanAddStack()));
        }


    }

    private InventoryItemSlot FindFirstEmptyItemSlot(ItemDataSO itemData)
    {
        if (itemData.itemType == ItemType.material)
        {
            foreach (var itemSlot in materialItemSlotList) // 遍历材料物品槽位列表
            {
                if (itemSlot.HasItem() == false) // 找到第一个空槽位
                {
                    return itemSlot;
                }
            }
            return null;
        }
        else
        {
            foreach (var itemSlot in cubeItemSlotList) // 遍历浮块物品槽位列表
            {
                if (itemSlot.HasItem() == false) // 找到第一个空槽位
                {
                    return itemSlot;
                }
            }
            return null;
        }

    }

    private int GetAvaliableAmountOf(ItemDataSO itemData)
    {
        int avaliableAmount = 0;
        if (itemData.itemType == ItemType.material)
        {
            foreach (var itemSlot in materialItemSlotList)
            {
                if (!itemSlot.HasItem())
                {
                    continue;
                }
                if (itemSlot.itemInSlot.ItemData == itemData)
                {
                    avaliableAmount += itemSlot.itemInSlot.stackSize;
                }
            }
        }
        else
        {
            foreach (var itemSlot in cubeItemSlotList)
            {
                if (!itemSlot.HasItem())
                {
                    continue;
                }
                if (itemSlot.itemInSlot.ItemData == itemData)
                {
                    avaliableAmount += itemSlot.itemInSlot.stackSize;
                }
            }
        }

        return avaliableAmount;
    }

    private void OnValidate()
    {
        EnsureSlotListSize();
    }

    private void EnsureSlotListSize()
    {
        if (materialItemSlotList == null) materialItemSlotList = new List<InventoryItemSlot>();
        while (materialItemSlotList.Count < maxMaterialSize)
            materialItemSlotList.Add(new InventoryItemSlot());  // 补空槽
        if (materialItemSlotList.Count > maxMaterialSize)
            materialItemSlotList.RemoveRange(maxMaterialSize, materialItemSlotList.Count - maxMaterialSize);

        if (cubeItemSlotList == null) cubeItemSlotList = new List<InventoryItemSlot>();
        while (cubeItemSlotList.Count < maxCubeSize)
            cubeItemSlotList.Add(new InventoryItemSlot());  // 补空槽
        if (cubeItemSlotList.Count > maxCubeSize)
            cubeItemSlotList.RemoveRange(maxCubeSize, cubeItemSlotList.Count - maxCubeSize);
    }
}
