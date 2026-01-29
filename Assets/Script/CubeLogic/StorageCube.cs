using UnityEngine;

public class StorageCube : CubeBase
{
    public InventoryStorage inventoryStorage {  get; private set; }
    public static StorageCube Instance;

    // 临时测试用
    [SerializeField] private ItemDataSO wood;
    [SerializeField] private ItemDataSO iron;
    [SerializeField] private ItemDataSO cannon;
    [SerializeField] private ItemDataSO fishing;

    protected override void Awake()
    {
        base.Awake();
        inventoryStorage = GetComponent<InventoryStorage>();
        if(Instance!=null && Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        
        if(Input.GetKeyUp(KeyCode.Alpha1))
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
    public override void OnEasyInteract(PLControl interactPlayer)
    {
        base.OnEasyInteract(interactPlayer);
        LevelUI.Instance.ToggleStoragePanel(inventoryStorage,interactPlayer);
    }


    public void GetItem(ItemDataSO itemToGet,int amount)
    {
        inventoryStorage.AddItem(itemToGet, amount);
        string getItemMessage = "已获得：" + itemToGet.itemName + "x" + amount;
        LevelUI.Instance.hintMessage.ShowQuickMessage(getItemMessage);
    }
}
