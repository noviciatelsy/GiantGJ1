using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;
    public InventoryStorage inventoryStorage { get; private set; }
    [SerializeField] private AudioEventSO getItemSFX;


    // 临时测试用
    [SerializeField] private ItemDataSO wood;
    [SerializeField] private ItemDataSO iron;
    [SerializeField] private ItemDataSO cannon;
    [SerializeField] private ItemDataSO fishing;


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

    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            GetItem(wood, 1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            GetItem(iron, 1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            GetItem(cannon, 1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            GetItem(fishing, 1);
        }
    }

    public void GetItem(ItemDataSO itemToGet, int amount)
    {
        inventoryStorage.AddItem(itemToGet, amount);
        string getItemMessage = "已获得：" + itemToGet.itemName + "x" + amount;
        LevelUI.Instance.hintMessage.ShowQuickMessage(getItemMessage);
        getItemSFX.Play();
    }
}
