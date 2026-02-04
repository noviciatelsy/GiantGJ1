using UnityEngine;
using System;

public class BoatManager : MonoBehaviour
{
    public static BoatManager Instance;

    [SerializeField] public CubeItemDataSO[] cubeDatas;
    [SerializeField] private int[] cubeDurationDatas;

    public CubeItemDataSO cubeempty;
    public CubeItemDataSO rudder;
    public CubeItemDataSO storageBox;

    // 船体尺寸：x = 长度（行），y = 宽度（列）
    public Vector2Int cubesize = new Vector2Int(3, 3);
    private float cubeSize = 10f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        if (cubeDatas == null || cubeDatas.Length != cubesize.x * cubesize.y)
        {
            cubeDatas = new CubeItemDataSO[cubesize.x * cubesize.y];
            cubeDatas[0] = storageBox;
            cubeDatas[1] = rudder;
        }
    }


    public void GenerateBoat()
    {
        GameObject boatGO = GameObject.Find("Boat");
        if (boatGO == null)
        {
            Debug.LogWarning("BoatManager: 未找到名为 'Boat' 的 GameObject，取消生成船体。");
            return;
        }

        Transform boatRoot = boatGO.transform;

        int width = cubesize.y;   // Z 方向
        int length = cubesize.x;   // X 方向

        // 用于居中对齐
        float halfWidthOffset = (width - 1) * 0.5f * cubeSize;

        for (int i = 0; i < length; i++)
        {
            float posZ = i * cubeSize;

            for (int j = 0; j < width; j++)
            {
                float posX = j * cubeSize - halfWidthOffset;


                CubeItemDataSO cubeData = GetCube(i, j);
                GameObject cube = cubeData.cubePrefab;
                if (cubeData == null)
                    cubeData = cubeempty;

                CubeBase cubebase = cube.GetComponent<CubeBase>();
                if (cubebase != null)
                {
                    cubebase.cubePos = new Vector2Int(i, j);
                }
                else Debug.Log("没有找到cubebase");

                //Debug.Log($"生成方块 [{i},{j}] worldPos=({posX}, -1, {posZ})");
                Instantiate(
                    cube,
                    new Vector3(posX, -1f, posZ),
                    Quaternion.identity,
                    boatRoot
                );

            }
        }
    }

    CubeItemDataSO GetCube(int x, int y)
    {
        int index = x * cubesize.y + y;
        return cubeDatas[index] != null ? cubeDatas[index] : cubeempty;
    }

    int GetCubeDuration(int x, int y)
    {
        int index = x * cubesize.y + y;
        return cubeDurationDatas[index];
    }


}
