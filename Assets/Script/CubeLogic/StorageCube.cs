using UnityEngine;

public class StorageCube : CubeBase
{
    private InventoryStorage inventoryStorage;

    // ¡Ÿ ±≤‚ ‘”√
    [SerializeField] private ItemDataSO wood;
    [SerializeField] private ItemDataSO iron;
    [SerializeField] private ItemDataSO cannon;
    [SerializeField] private ItemDataSO fishing;

    protected override void Awake()
    {
        base.Awake();
        inventoryStorage = GetComponent<InventoryStorage>();
    }

    private void Update()
    {
        
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            inventoryStorage.AddItem(wood, 1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            inventoryStorage.AddItem(iron, 1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            inventoryStorage.AddItem(cannon, 1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            inventoryStorage.AddItem(fishing, 1);
        }
    }
    public override void OnEasyInteract(PLControl interactPlayer)
    {
        base.OnEasyInteract(interactPlayer);
        LevelUI.Instance.ToggleStoragePanel(inventoryStorage,interactPlayer);
    }

}
