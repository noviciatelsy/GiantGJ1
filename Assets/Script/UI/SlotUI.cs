using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private StoragePanel storagePanel;
    private SlotSelectedImage slotSelectedImage;
    private RectTransform rect;

    private void Awake()
    {
        slotSelectedImage = GetComponentInChildren<SlotSelectedImage>(true);
        storagePanel = GetComponentInParent<StoragePanel>(true);
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slotSelectedImage.SelectSlot();
        storagePanel.itemToolTip.ShowToolTip(true,rect);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slotSelectedImage.EndSelectSlot();
        storagePanel.itemToolTip.ShowToolTip(false, rect);
    }
}
