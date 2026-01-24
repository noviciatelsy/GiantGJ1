using UnityEngine;

[ExecuteAlways]
public class SpriteRendererSorter : MonoBehaviour
{
    void Start()
    {
        SortSpritesByZ();
    }

    void SortSpritesByZ()
    {
        // 获取所有 SpriteRenderer 组件
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        // 遍历所有 SpriteRenderer，并根据其 Z 值排序
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            // 获取当前物体的位置的 Z 值
            float zPosition = sr.transform.position.z;

            // 设置 Sorting Order，Z 值越小，排序值越小，越在前面
            sr.sortingOrder = Mathf.RoundToInt(-zPosition * 100);
        }
    }
}
