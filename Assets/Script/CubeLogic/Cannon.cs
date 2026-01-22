using UnityEngine;

public class Cannon : CubeBase
{
    private BoatMove boatMove;

    protected override void Awake()
    {
        base.Awake();
        boatMove = GetComponentInParent<BoatMove>();
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
