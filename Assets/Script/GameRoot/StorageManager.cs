using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;
    public InventoryStorage inventoryStorage { get; private set; }
    [SerializeField] private AudioEventSO getItemSFX;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        inventoryStorage=GetComponent<InventoryStorage>();
    }


    public void GetItem(ItemDataSO itemToGet, int amount)
    {
        inventoryStorage.AddItem(itemToGet, amount);
        string getItemMessage = "ÒÑ»ñµÃ£º" + itemToGet.itemName + "x" + amount;
        HintMessage.Instance.ShowQuickMessage(getItemMessage);
        getItemSFX.Play();
    }
}
