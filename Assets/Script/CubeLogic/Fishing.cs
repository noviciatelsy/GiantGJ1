using UnityEngine;

public class Fishing : CubeBase
{
    private BoatMove boatMove;

    //瞄准线
    private Vector2 aim = Vector2.up;        // 初始朝向（向前）
    public float rotateSpeed = 180f;        // 每秒旋转角度（度）
    [SerializeField] private Transform FishingSprite;
    [SerializeField] private Transform aimLineDots;
    [SerializeField] private Transform aimLineDots2; //子弹发射点

    //private LineRenderer aimLine;
    //private float aimLineLength = 25f;

    protected override void Awake()
    {
        base.Awake();
        boatMove = GetComponentInParent<BoatMove>();
        aimLineDots.gameObject.SetActive(false);
        //if (aimLine == null)
        //    aimLine = GetComponentInChildren<LineRenderer>();
    }

    void Update()
    {
        if (isInteracting && currentPlayer != null)
        {
            float inputX = currentPlayer.moveInput.x;
            if (Mathf.Abs(inputX) < 0.01f) return;
            // 计算本帧旋转角度
            float deltaAngle = inputX * rotateSpeed * Time.deltaTime;
            //CannonSprite.transform.Rotate(0,0,deltaAngle);
            FishingSprite.transform.Rotate(0, deltaAngle, 0);
        }
    }


    public override void OnInteractEnter()
    {
        base.OnInteractEnter();
        //aimLine.enabled=true;
        aimLineDots.gameObject.SetActive(true);
        Debug.Log("钓鱼竿：开始控制钓鱼竿");

    }

    public override void OnInteractExit()
    {
        base.OnInteractExit();
        //aimLine.enabled = false;
        aimLineDots.gameObject.SetActive(false);
        Debug.Log("钓鱼竿：停止控制钓鱼竿");

    }

    public override void OnCubeUse()
    {
        base.OnCubeUse();
        Debug.Log("使用钓鱼竿");

    }
}
