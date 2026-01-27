using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CubeDetails : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cubeName;
    [SerializeField] private Image cubeIcon;
    [SerializeField] private TextMeshProUGUI cubeDurability;
    [SerializeField] private TextMeshProUGUI cubeDescription;
    [SerializeField] private Sprite emptySprite;

    public void UpdateCurrentCubeDetails(CubeItemDataSO cubeDatail,int currentDurability)
    {
        if (currentDurability < 0) return;
        if (cubeDatail == null)
        {

            cubeName.text = "";
            cubeIcon.sprite = emptySprite;
            cubeDescription.text = "";
            cubeDurability.text = "ÄÍ¾Ã£º" + currentDurability.ToString() + "/2";
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
            cubeDurability.text = "ÄÍ¾Ã£º" + currentDurability.ToString() + "/2";
        }
    }

 
}
