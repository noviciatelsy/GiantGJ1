using TMPro;
using UnityEditor;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName; // 物品名称
    [SerializeField] private TextMeshProUGUI itemType; // 物品种类
    [SerializeField] private TextMeshProUGUI itemDesription; // 物品描述
    [SerializeField] private TextMeshProUGUI equipInstruction; // 使用说明

    public void ShowToolTip(bool show, RectTransform targetRect, InventoryItem itemToShow)
    {
        base.ShowToolTip(show,targetRect);
        if (show == false || itemToShow == null)
        {
            return;
        }

        // 更新物品面板文本
        itemName.text = itemToShow.ItemData.itemName;
        itemType.text = itemToShow.ItemData.GetItemNameByType();
        itemDesription.text=itemToShow.ItemData.itemDescription;

        // 根据是否为浮块显示使用说明
        if(itemToShow.ItemData.itemType==ItemType.cube)
        {
            equipInstruction.enabled = true;
        }
        else
        {
            equipInstruction.enabled = false;
        }
    }
}
