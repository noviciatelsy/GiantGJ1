using System.Collections.Generic;
using UnityEngine;

public class Fishing : CubeBase
{
    [SerializeField] private AudioEventSO fishingSFX;


    private BoatMove boatMove;

    //瞄准线
    private Vector2 aim = Vector2.up;        // 初始朝向（向前）
    public float rotateSpeed = 180f;        // 每秒旋转角度（度）
    [SerializeField] private Transform FishingSprite;
    [SerializeField] private Transform aimLineDots;
    [SerializeField] private Transform aimLineDots2; //子弹发射点

    private AimLine aimLine;
    private float chargeTimer = 0f;
    private float maxChargetime = 3.0f;
    private bool isCharging = false;
    private float mindist = 1.5f;
    private float maxdist = 10.0f;

    protected override void Awake()
    {
        base.Awake();
        boatMove = GetComponentInParent<BoatMove>();
        aimLineDots.gameObject.SetActive(false);
        if (aimLine == null)
            aimLine = GetComponent<AimLine>();
    }

    void Update()
    {
        if (isInteracting && currentPlayer != null)
        {
            float inputX = currentPlayer.moveInput.x;
            if (Mathf.Abs(inputX) >= 0.01f)
            {
                float deltaAngle = inputX * rotateSpeed * Time.deltaTime;
                FishingSprite.transform.Rotate(0, deltaAngle, 0);
            }
        }

        if (isCharging && aimLine != null)
        {
            chargeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(chargeTimer / maxChargetime);

            float dist = Mathf.Lerp(mindist, maxdist, t);
            aimLine.currentLength = dist;
            //Debug.Log(dist);
        }


    }


    public override void OnInteractEnter()
    {
        base.OnInteractEnter();
        //aimLine.enabled=true;
        aimLineDots.gameObject.SetActive(true);
        Debug.Log("钓鱼竿：开始控制钓鱼竿");
        aimLine.currentLength = mindist;
        if (aimLine == null)
        {
            return;
        }

    }

    public override void OnInteractExit()
    {
        base.OnInteractExit();
        //aimLine.enabled = false;
        aimLineDots.gameObject.SetActive(false);
        aimLine.currentLength = mindist;

        Debug.Log("钓鱼竿：停止控制钓鱼竿");

    }

    public override void OnCubeUse()
    {
        base.OnCubeUse();
        Debug.Log("使用钓鱼竿");
        if (aimLine == null)
        {
            return;
        }

        if (!isCharging)
        {
            aimLine.currentLength = mindist;
            aimLineDots.gameObject.SetActive(true); // 显示瞄准线
            isCharging = true;
            chargeTimer = 0f;
        }

    }

    public override void EndCubeUse()
    {
        base.EndCubeUse();

        LandPoint landPoint = GetComponentInChildren<LandPoint>();
        if (landPoint == null)
        {
            Debug.LogWarning("未找到 LandPoint 子物体！");
            return;
        }
        List<MaterialBase> materials = landPoint.GetMatInLandPoint();
        if (materials == null || materials.Count == 0)
        {
            Debug.Log("落点区域内没有材料");
            return;
        }

        foreach (MaterialBase mat in materials)
        {
            if (mat == null) continue;
            mat.isFished();
            //StorageManager.Instance.GetItem(
            //    mat.MaterialData,
            //    mat.ItemNum
            //);
            //Destroy(mat.gameObject);
        }

        aimLineDots.gameObject.SetActive(false); // 隐藏瞄准线
        aimLine.currentLength = mindist;
        isCharging = false;
        fishingSFX.Play(); // 播放甩杆音效
    }

}
