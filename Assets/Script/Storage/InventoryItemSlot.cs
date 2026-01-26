using System;


[Serializable]
public class InventoryItemSlot
{
    public InventoryItem itemInSlot; // 槽位所填充的物品InventoryItem对象

    public bool HasItem() // 该槽位是否含InventoryItem对象
    {
        return itemInSlot != null && itemInSlot.ItemData != null;
    }
}