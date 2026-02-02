using UnityEngine;
using System.Collections;

public class MaterialBase : MonoBehaviour
{
    public ItemDataSO MaterialData;
    public int ItemNum;

    private Coroutine fishCoroutine;

    public void OnDestroy()
    {
        
    }

    public void isFished()
    {
        if (fishCoroutine != null)
            return;

        fishCoroutine = StartCoroutine(FishMoveCoroutine());
    }

    private IEnumerator FishMoveCoroutine()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = Vector3.zero;

        float duration = 0.2f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            // 到达距离 8 时直接销毁
            if (Vector3.Distance(transform.position, targetPos) <= 8f)
            {
                StorageManager.Instance.GetItem(MaterialData, ItemNum);
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }

        StorageManager.Instance.GetItem(MaterialData,ItemNum);
        Destroy(gameObject);
    }
}
