using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI stackSizeText; // 堆叠数文本


    private StoragePanel storagePanel;
    private SlotSelectedImage slotSelectedImage;
    private RectTransform rect;
    private int slotIndex; // UI的槽位序号，用于在InventoryStorage中反向定位InventoryItem
    public InventoryItem itemInSlot;// 槽内物品的InventoryItem对象

    private void Awake()
    {
        slotSelectedImage = GetComponentInChildren<SlotSelectedImage>(true);
        storagePanel = GetComponentInParent<StoragePanel>(true);
        rect = GetComponent<RectTransform>();
    }
    public void BindItemIndex(int index)
    {
        slotIndex = index;
    }
    public void UpdateSlot(InventoryItem item)
    {
        itemInSlot = item; // 获取Inventory_Item对象
        if (itemInSlot == null) // 如果传入空物品
        {
            stackSizeText.text = ""; // 不显示堆叠数
            itemIcon.sprite = emptySprite; // 显示为空槽位
            return;
        }
        itemIcon.sprite = itemInSlot.ItemData.itemIcon; // 更新UI图标为槽内物品图标
        stackSizeText.text = itemInSlot.stackSize > 1 ? itemInSlot.stackSize.ToString() : "";
        // 更新UI图标的物品堆叠数（大于1时才显示数字）
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        slotSelectedImage.SelectSlot();
        if(itemInSlot != null )
        {
            storagePanel.itemToolTip.ShowToolTip(true, rect,itemInSlot);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slotSelectedImage.EndSelectSlot();
        if(itemInSlot != null )
        {
            storagePanel.itemToolTip.ShowToolTip(false, rect,itemInSlot);
        }
    }
}
