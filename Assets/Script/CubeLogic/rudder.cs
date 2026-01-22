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
        Debug.Log("¶æ£º¿ªÊ¼¿ØÖÆ´¬");
        boatMove.SetCurrentDriver(currentPlayer);
    }

    public override void OnInteractExit()
    {
        base.OnInteractExit();
        Debug.Log("¶æ£ºÍ£Ö¹¿ØÖÆ´¬");
        boatMove.ResetCurrentDriver();
    }

}