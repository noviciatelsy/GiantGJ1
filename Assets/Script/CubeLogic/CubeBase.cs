using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeBase : MonoBehaviour
{
    public Vector2Int cubePos;

    public PLControl currentPlayer {  get; private set; }
    private HandleFix handlefix;
    private ChosenImage chosenImage;
    public int landNumber {  get; private set; }

    public bool isInteracting = false;

    protected virtual void Awake()
    {
        handlefix = GetComponent<HandleFix>();
        chosenImage = GetComponentInChildren<ChosenImage>();
        landNumber = 0;
    }

    void Start()
    {
        //CubeCrush();
        CubeCrush();
    }

    void Update()
    {
        if(isInteracting)
        {
            //Debug.Log(currentPlayer.moveInput.x);
        }
    }


    public virtual void IsChoose()
    {
        Debug.Log(name + " 被选中");
    }

    public void OnLand()
    {
        landNumber++;
        if(landNumber==1)
        {
            StartCoroutine(MoveToCoroutine(transform, transform.position + Vector3.down * 0.8f, 0.1f));
        }
        chosenImage.ChooseCube();
    }
    public void EndLand()
    {
        landNumber--;
        if(landNumber==0)
        {
            StartCoroutine(MoveToCoroutine(transform, transform.position - Vector3.down * 0.8f, 0.1f));
        }
        chosenImage.EndChooseCube();
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
        //if (handlefix.isCrushed)
        //{
        //    SetCurrentPlayer(null); // 还原浮块所有权
        //    OnInteractExit();
            
        //    return; //完全损坏无法交互
        //}

        //将PLmove所代表的gameobject移动到自己位置
        Vector3 targetPos = transform.position + Vector3.up * 7.5f; // 向上5个单位
        StartCoroutine(MoveToCoroutine(currentPlayer.transform, targetPos, 0.2f));
        OnInteractEnter();
    }

    public virtual void OnInteractEnter()
    {

        Debug.Log("OnInteract");
        isInteracting = true;
    }

    public virtual void OnInteractExit()
    {
        Debug.Log("EndInteract");
        isInteracting = false;
    }

    public void OnRepairBegin(PLControl currentFixingPlayer)
    {
        handlefix.StartFix(currentFixingPlayer);

    }

    public void OnRepairEnd()
    {
        handlefix.EndFix();

    }

    public void OnCubeUse()
    {
        Debug.Log("Use"+gameObject.name);
    }

    public PLControl GetCurrentFixingPlayer()
    {
        return handlefix.currentFixingPlayer;
    }

    public void SetCurrentPlayer(PLControl currentPlayer)
    {
        this.currentPlayer = currentPlayer;
    }

    public void CubeCrush()
    {
        handlefix.HealthBarCrush();
    }

    public int CheckCubeHealth()
    {
        return handlefix.currentBarCount;
    }

    //旋转瞄准线用
    public Vector2 RotateVector2(Vector2 v, float deltaAngle)
    {
        float rad = deltaAngle * Mathf.Deg2Rad;

        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}