using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CubeDetailsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cubeName;
    [SerializeField] private Image cubeIcon;
    [SerializeField] private TextMeshProUGUI cubeDurability;
    [SerializeField] private TextMeshProUGUI cubeDescription;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private TextMeshProUGUI woodAmountToFix;
    [SerializeField] private TextMeshProUGUI ironAmountToFix;
    [SerializeField] private TextMeshProUGUI totalWoodAmount;
    [SerializeField] private TextMeshProUGUI totalIronAmount;

    [Space]
    [SerializeField] private MaterialItemDataSO woodData;
    [SerializeField] private MaterialItemDataSO ironData;

    public void UpdateCurrentCubeDetails(CubeItemDataSO cubeDatail,int currentDurability)
    {
        if (currentDurability < 0) return;
        if (cubeDatail == null)
        {

            cubeName.text = "";
            cubeIcon.sprite = emptySprite;
            cubeDescription.text = "";
            cubeDurability.text = "ÄÍ¾Ã£º" + currentDurability.ToString() + "/2";
            woodAmountToFix.text = "x"+cubeDatail.materialsToRepair.woodCost;
            ironAmountToFix.text="x"+cubeDatail.materialsToRepair.IronCost;
            totalWoodAmount.text = "x" + StorageManager.Instance.inventoryStorage.GetAvaliableAmountOf(woodData);
            totalIronAmount.text = "x" + StorageManager.Instance.inventoryStorage.GetAvaliableAmountOf(ironData);
        }
        else
        {
            if (cubeDatail.itemType == ItemType.material)
            {
                return;
            }
    
            cubeName.text = cubeDatail.itemName;
            cubeIcon.sprite = cubeDatail.itemIcon;
            cubeDescription.text = cubeDatail.howToUse;
            cubeDurability.text = "ÄÍ¾Ã£º" + currentDurability.ToString() + "/"+cubeDatail.maxDurability.ToString();
            woodAmountToFix.text = "x" + cubeDatail.materialsToRepair.woodCost;
            ironAmountToFix.text = "x" + cubeDatail.materialsToRepair.IronCost;
            totalWoodAmount.text = "x" + StorageManager.Instance.inventoryStorage.GetAvaliableAmountOf(woodData);
            totalIronAmount.text = "x" + StorageManager.Instance.inventoryStorage.GetAvaliableAmountOf(ironData);
        }
    }

 
}
