using System;

[Serializable]
public class InventoryItem 
{
    public ItemDataSO ItemData; // 物品信息
    public int stackSize = 1; // 当前堆叠数

    public InventoryItem(ItemDataSO ItemData)
    {
        this.ItemData = ItemData;
    }

    
}
