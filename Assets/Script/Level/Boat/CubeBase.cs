using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CubeBase : MonoBehaviour
{
    public Vector2Int cubePos;
    public PLControl currentPlayer {  get; private set; }

    public virtual void IsChoose()
    {
        Debug.Log(name + " 被选中");
    }

    public void OnLand()
    {
        StartCoroutine(MoveToCoroutine(transform, transform.position + Vector3.down * 0.8f, 0.1f));
    }
    public void EndLand()
    {
        StartCoroutine(MoveToCoroutine(transform, transform.position - Vector3.down * 0.8f, 0.1f));
    }

    private IEnumerator MoveToCoroutine(Transform target, Vector3 targetPos, float duration)
    {
        Vector3 startPos = target.transform.position;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            target.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        target.transform.position = targetPos;
    }

    public void OnInteractEnterBase()
    {
        //将PLmove所代表的gameobject移动到自己位置
        Vector3 targetPos = transform.position + Vector3.up * 7.5f; // 向上5个单位
        StartCoroutine(MoveToCoroutine(currentPlayer.transform, targetPos, 0.2f));
        OnInteractEnter();
    }
    public virtual void OnInteractEnter()
    {
        Debug.Log("OnInteract");
    }

    public virtual void OnInteractExit()
    {
        Debug.Log("EndInteract");
    }

    public void OnRepairStart()
    {
        Debug.Log("RepairStart");
    }

    public void OnRepairEnd()
    {
        Debug.Log("RepairEnd");
    }

    public void SetCurrentPlayer(PLControl currentPlayer)
    {
        this.currentPlayer = currentPlayer;
    }
}