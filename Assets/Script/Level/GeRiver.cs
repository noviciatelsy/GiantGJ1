using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GeRiver : MonoBehaviour
{
    public GameObject riverPrefab;
    public List<GameObject> Rivers;

    private float riverLength = 600f;

    void Start()
    {
        //在(0,0,0)和（0,0,500）之间生成河流
        SpawnRiver(0f);
        SpawnRiver(600f);
        SpawnRiver(1200f);
    }

    void SpawnRiver(float zPos)
    {
        GameObject river = Instantiate(
            riverPrefab,
            new Vector3(0f, 0f, zPos),
            Quaternion.identity
        );

        Rivers.Add(river);
    }

    public void GeNewRiver()
    {
        Debug.Log("生成新的河流");
        // 找到最前面的河流
        float maxZ = float.MinValue;

        foreach (GameObject r in Rivers)
        {
            if (r.transform.position.z > maxZ)
                maxZ = r.transform.position.z;
        }

        SpawnRiver(maxZ + riverLength);
    }

    public void DestoryRiver(GameObject river)
    {
        Rivers.Remove(river);
        Destroy(river);
    }
}
