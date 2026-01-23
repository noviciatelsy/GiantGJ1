using System;
using UnityEngine;

public class Cannon : CubeBase
{
    private BoatMove boatMove;

    //瞄准线
    private Vector2 aim = Vector2.up;        // 初始朝向（向前）
    public float rotateSpeed = 180f;        // 每秒旋转角度（度）
    private LineRenderer aimLine;
    private float aimLineLength = 25f;

    protected override void Awake()
    {
        base.Awake();
        boatMove = GetComponentInParent<BoatMove>();

        if (aimLine == null)
            aimLine = GetComponentInChildren<LineRenderer>();
    }

    void Update()
    {
        if (!isInteracting || currentPlayer == null)
        {
            if (aimLine != null)
                aimLine.enabled = false;
            return;
        }

        if (aimLine != null)
            aimLine.enabled = true;

        float inputX = currentPlayer.moveInput.x;
        if (Mathf.Abs(inputX) < 0.01f) return;
        // 计算本帧旋转角度
        float deltaAngle = -inputX * rotateSpeed * Time.deltaTime;
        aim = RotateVector2(aim, deltaAngle);
        aim.Normalize();
        //Debug.Log(aim);

        // 绘制瞄准线
        Vector3 dir = new Vector3(aim.x, 0f, aim.y).normalized;
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y + 3.5f, transform.position.z);
        Vector3 endPos = startPos + dir * aimLineLength;
        aimLine.SetPosition(0, startPos);
        aimLine.SetPosition(1, endPos);
    }


    public override void OnInteractEnter()
    {
        base.OnInteractEnter();
        Debug.Log("加农炮：开始控制加农炮");
   
    }

    public override void OnInteractExit()
    {
        base.OnInteractExit();
        Debug.Log("加农炮：停止控制加农炮");

    }
}
