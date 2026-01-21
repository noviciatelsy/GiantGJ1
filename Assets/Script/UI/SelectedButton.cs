using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image selectedImage;

    private void Awake()
    {
        selectedImage = GetComponent<Image>();
        selectedImage.enabled = false;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedImage.enabled = false;
    }

}
