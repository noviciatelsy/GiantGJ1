using UnityEngine;

[CreateAssetMenu(menuName = "Item Data/Cube", fileName = "CubeData - ")]
public class CubeItemDataSO : ItemDataSO
{
    public GameObject cubePrefab; // 用于从SO中提取的cube预制体
    public Sprite spriteToShow; // 用于展示的贴图
    [TextArea] public string howToUse; 
}