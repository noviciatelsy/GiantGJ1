using TMPro;
using UnityEngine;

public class MaterialsRemain : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI woodRemain;
    [SerializeField] private TextMeshProUGUI ironRemain;

    private void OnEnable()
    {
        StorageManager.Instance.inventoryStorage.onStorageChanged += UpdateMaterialsRemain;
        UpdateMaterialsRemain();
    }

    private void OnDisable()
    {
        StorageManager.Instance.inventoryStorage.onStorageChanged -= UpdateMaterialsRemain;
    }

    private void UpdateMaterialsRemain()
    {
        woodRemain.text = "x"+StorageManager.Instance.inventoryStorage.GetCurrentAmountOfWood().ToString();
        ironRemain.text="x"+StorageManager.Instance.inventoryStorage.GetCurrentAmountOfIron().ToString();
    }
}
