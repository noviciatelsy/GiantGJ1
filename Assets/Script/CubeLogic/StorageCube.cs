using UnityEngine;

public class StorageCube : CubeBase
{

    protected override void Awake()
    {
        base.Awake();
    }


    public override void OnEasyInteract(PLControl interactPlayer)
    {
        //base.OnEasyInteract(interactPlayer);
        LevelUI.Instance.ToggleStoragePanel(StorageManager.Instance.inventoryStorage,interactPlayer);
    }



}
