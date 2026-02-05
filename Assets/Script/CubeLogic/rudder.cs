using Unity.Multiplayer.PlayMode;
using UnityEngine;

public class rudder : CubeBase
{
    private BoatMove boatMove;

    protected override void Awake()
    {
        base.Awake();
        boatMove= GetComponentInParent<BoatMove>();
    }

    public override void OnInteractEnter()
    {
        base.OnInteractEnter();
        Debug.Log("舵：开始控制船");
        boatMove.SetCurrentDriver(currentPlayer);
    }

    public override void OnInteractExit()
    {
        base.OnInteractExit();
        Debug.Log("舵：停止控制船");
        boatMove.ResetCurrentDriver();
    }

    public void FixedUpdate()
    {
        if (handlefix.currentBarCount == 0)
        {
            Debug.Log("来自与舵rudder的消息：舵已损坏，游戏结束！");
            GlobalGameData.isEnd = true;
            LevelUI.Instance.losePanel.gameObject.SetActive(true);
        }
    }

    public override void OnEasyInteract(PLControl interactPlayer)
    {
        //base.OnEasyInteract(interactPlayer);    不能替换
    }
}