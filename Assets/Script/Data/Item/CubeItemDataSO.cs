using System;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Item Data/Cube", fileName = "CubeData - ")]
public class CubeItemDataSO : ItemDataSO
{
    public GameObject cubePrefab; // 用于从SO中提取的cube预制体
    public Sprite spriteToShow; // 用于展示的贴图
    [TextArea] public string howToUse; 
    public MaterialCost materialsToRepair;
    public cubePiece piece;
    public int maxDurability; // 最大耐久
}

[Serializable]
public class MaterialCost
{
    public int woodCost;
    public int IronCost;
}

[Serializable]
public class cubePiece
{
    public int woodCost;
    public int IronCost;
}