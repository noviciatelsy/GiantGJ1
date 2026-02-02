using UnityEngine;

public class StoreItem : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject itemTooltip;

    private void Awake()
    {
        itemTooltip.SetActive(false);
    }

    public void Interact()
    {
        
    }

    public void SetInterable(bool isInteractable)
    {
        itemTooltip.SetActive(isInteractable);
    }
}
