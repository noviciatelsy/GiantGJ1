using System;
using UnityEngine;

public class Cannon : CubeBase
{
    [Header("Audio")]
    [SerializeField] private AudioEventSO cannonFireSFX;

    private BoatMove boatMove;

    //瞄准线
    private Vector2 aim = Vector2.up;        // 初始朝向（向前）
    public float rotateSpeed = 180f;        // 每秒旋转角度（度）
    [SerializeField] private Transform CannonSprite;
    [SerializeField] private Transform aimLineDots;
    [SerializeField] private Transform aimLineDots2; //子弹发射点
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float fireCooldown = 0.5f; // 发射冷却时间，0.5秒
    private float lastFireTime = -Mathf.Infinity; // 上次发射时间，初始化为负无穷大

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
        if(isInteracting&&currentPlayer!=null)
        {
            float inputX = currentPlayer.moveInput.x;
            if (Mathf.Abs(inputX) < 0.01f) return;
            // 计算本帧旋转角度
            float deltaAngle = inputX * rotateSpeed * Time.deltaTime;
            //CannonSprite.transform.Rotate(0,0,deltaAngle);
            CannonSprite.transform.Rotate(0, deltaAngle, 0);
        }
    }


    public override void OnInteractEnter()
    {
        base.OnInteractEnter();
        //aimLine.enabled=true;
        aimLineDots.gameObject.SetActive(true);
        Debug.Log("加农炮：开始控制加农炮");
        
    }

    public override void OnInteractExit()
    {
        base.OnInteractExit();
        //aimLine.enabled = false;
        aimLineDots.gameObject.SetActive(false);
        Debug.Log("加农炮：停止控制加农炮");
        
    }

    public override void OnCubeUse()
    {
        base.OnCubeUse();

        // 判断冷却时间
        if (Time.time - lastFireTime < fireCooldown)
        {
            // 冷却中，不能发射
            return;
        }

        Debug.Log("加农炮：发射！");

        // 从aimLineDots第一个点位置发射炮弹
        if (bulletPrefab != null && aimLineDots.childCount > 0)
        {
            //Transform firePoint = aimLineDots2.GetChild(0);
            GameObject bulletGO = Instantiate(bulletPrefab, aimLineDots2.transform.position, Quaternion.identity);

            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                // direction沿CannonSprite的forward（Z轴方向）
                bullet.direction = CannonSprite.forward;
            }

            cannonFireSFX.Play();
        }

        lastFireTime = Time.time;
    }

}
