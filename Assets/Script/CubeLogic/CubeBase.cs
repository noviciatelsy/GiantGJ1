using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeBase : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioEventSO floorSqueakSFX;
    [SerializeField] private AudioEventSO waterSplashSFX;
    [SerializeField] private AudioEventSO woodBreakSFX;
    [Space]

    public Vector2Int cubePos;
    public CubeItemDataSO cubeData;

    public PLControl currentPlayer {  get; private set; }
    public HandleFix handlefix;
    private ChosenImage chosenImage;
    public int landNumber {  get; private set; }

    public bool isInteracting = false;
    public bool shouldMoveCurrentPlayerToCentre = false;
    public bool allowInteract = false;

    protected Vector3 positionOffset = Vector3.down * 0.8f;
    protected virtual void Awake()
    {
        handlefix = GetComponent<HandleFix>();
        chosenImage = GetComponentInChildren<ChosenImage>();
        landNumber = 0;
    }

    void Update()
    {
        if(isInteracting)
        {
            //Debug.Log(currentPlayer.moveInput.x);
        }
    }

    public void OnLand()
    {
        landNumber++;
        if(landNumber==1)
        {
            StartCoroutine(MoveToCoroutine(transform, transform.position + positionOffset, 0.1f));
        }
        chosenImage.ChooseCube();
        floorSqueakSFX.Play();
        waterSplashSFX.Play(); 
    }
    public void EndLand()
    {
        landNumber--;
        if(landNumber==0)
        {
            StartCoroutine(MoveToCoroutine(transform, transform.position - positionOffset, 0.1f));
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


    public void MoveCurrentPlayerToCentre()
    {
        Vector3 targetPos = transform.position + Vector3.up * 7.5f; // 向上5个单位
        StartCoroutine(MoveToCoroutine(currentPlayer.transform, targetPos, 0.2f));
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

    public virtual void OnEasyInteract(PLControl interactPlayer)
    {
        if (interactPlayer == null)
        {
            return;
        }
        if (interactPlayer.cubeToEquip == null)
        {
            return;
        }
        CubeItemDataSO cubeData = interactPlayer.cubeToEquip;
        string equipMessage = "已装备：" + cubeData.itemName;
        HintMessage.Instance.ShowQuickMessage(equipMessage);
        interactPlayer.ResetCubeToEquip(); // 清除人物待装配的浮块
        HintMessage.Instance.StopLongTimeMessage();
        ReplaceSelf(cubeData.cubePrefab);
    }

    public void OnRepairBegin(PLControl currentFixingPlayer)
    {
        if(handlefix.canAutoFix)
        {
            return;
        }
        if(StorageManager.Instance.inventoryStorage.GetCurrentAmountOfWood()<cubeData.materialsToRepair.woodCost
            || StorageManager.Instance.inventoryStorage.GetCurrentAmountOfIron() < cubeData.materialsToRepair.IronCost) // 如果wood或iron不够
        {
            return ;
        }
        handlefix.StartFix(currentFixingPlayer);

    }

    public void OnRepairEnd()
    {
        if(handlefix.canAutoFix)
        {
            return;
        }
        handlefix.EndFix();

    }

    public virtual void OnCubeUse()
    {
        Debug.Log("Use"+gameObject.name);
    }

    public virtual void EndCubeUse()
    {
        Debug.Log("EndUse" + gameObject.name);
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
        woodBreakSFX.Play();
        if(CheckCubeHealth()==1&&currentPlayer!=null) // 如果撞坏时（被撞时剩1血）刚好有玩家处于交互
        {
            currentPlayer.SetPlayerInteractingState(false); // 强制人物退出交互
            SetCurrentPlayer(null);
            OnInteractExit();
        }
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

    public virtual void ReplaceSelf(GameObject newPrefab)
    {
        if (newPrefab == null)
        {
            return;
        }
        StorageManager.Instance.GetItem(this.cubeData, 1); //替换入仓库中

        CubeBase cubeBase = newPrefab.GetComponent<CubeBase>();
        int boatsizey = BoatManager.Instance.cubesize.y;
        BoatManager.Instance.cubeDatas[cubePos.x * boatsizey + cubePos.y] = cubeBase.cubeData;
        cubeBase.cubePos = this.cubePos;
        Transform oldTransform = transform;

        // 记录旧对象的“场景信息”
        Vector3 worldPos = oldTransform.position - positionOffset;
        Quaternion worldRot = oldTransform.rotation;
        Vector3 localScale = oldTransform.localScale;

        Transform parent = oldTransform.parent;
        int siblingIndex = oldTransform.GetSiblingIndex();
        bool wasActive = gameObject.activeSelf;

        // 生成新对象（先生成，再设置父级与位置）
        GameObject newObj = Instantiate(newPrefab);

        // 先设置父级（保持世界坐标不变）
        newObj.transform.SetParent(parent, true);

        // 对齐 Transform
        newObj.transform.position = worldPos;
        newObj.transform.rotation = worldRot;
        newObj.transform.localScale = localScale;

        // 保持层级顺序一致
        newObj.transform.SetSiblingIndex(siblingIndex);

        // 继承激活状态
        newObj.SetActive(wasActive);

        // 最后销毁旧对象
        Destroy(gameObject);

        return;
    }
}