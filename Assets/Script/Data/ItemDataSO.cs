using System;
using UnityEngine;

public enum ItemType
{
    material,
    cube
}

public class ItemDataSO : ScriptableObject
{
    public string itemName; // 物品名称
    [TextArea] public string itemDescription; // 物品描述
    public string itemID=Guid.NewGuid().ToString(); // 随机ID
    public ItemType itemType; // 物品种类
    public Sprite itemIcon;
}


