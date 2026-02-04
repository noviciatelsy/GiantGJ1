using System.Collections.Generic;
using UnityEngine;

public class GeRiver : MonoBehaviour
{
    [Header("River Prefabs（按顺序配置）")]
    public List<GameObject> riverPrefabs;

    [Header("Runtime Rivers")]
    public List<GameObject> Rivers = new List<GameObject>();

    private float riverLength = 600f;

    // 当前生成到第几个 prefab
    private int spawnIndex = 0;

    void Start()
    {
        // 初始生成两个
        SpawnNextRiver(0, 300f);
        SpawnNextRiver(0, 900f);
    }

    void SpawnNextRiver(float x, float zPos)
    {
        if (riverPrefabs == null || riverPrefabs.Count == 0)
        {
            Debug.LogError("riverPrefabs 为空！");
            return;
        }

        GameObject prefabToSpawn = GetNextRiverPrefab();

        GameObject river = Instantiate(
            prefabToSpawn,
            new Vector3(x, 0f, zPos),
            Quaternion.identity
        );

        RiverLogic riverlogic = river.GetComponent<RiverLogic>();
        if (riverlogic != null)
            riverlogic.geRiver = this;

        Rivers.Add(river);
    }

    /// <summary>
    /// 核心：获取下一个要生成的 prefab
    /// </summary>
    GameObject GetNextRiverPrefab()
    {
        int count = riverPrefabs.Count;

        GameObject prefab;

        if (spawnIndex < count)
        {
            // 前期：1,2,3,4
            prefab = riverPrefabs[spawnIndex];
            spawnIndex++;
        }
        else
        {
            // 后期：在最后两个 prefab 间循环
            int lastIndex = count - 1;
            int secondLastIndex = count - 2;

            // spawnIndex = count -> secondLast
            // spawnIndex = count+1 -> last
            int loopIndex = (spawnIndex - count) % 2;
            prefab = riverPrefabs[secondLastIndex + loopIndex];

            spawnIndex++;
        }

        return prefab;
    }

    public void GeNewRiver(float x)
    {
        Debug.Log("生成新的河流");

        float maxZ = float.MinValue;
        foreach (GameObject r in Rivers)
        {
            if (r.transform.position.z > maxZ)
                maxZ = r.transform.position.z;
        }

        SpawnNextRiver(x, maxZ + riverLength);
    }

    public void DestoryRiver(GameObject river)
    {
        Rivers.Remove(river);
        Destroy(river);
    }
}
