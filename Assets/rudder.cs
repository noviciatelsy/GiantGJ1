using Unity.Multiplayer.PlayMode;
using UnityEngine;

public class rudder : CubeBase
{
    public BoatMove boatMove;

    public override void OnInteractEnter()
    {
        base.OnInteractEnter();
        Debug.Log("¶æ£º¿ªÊ¼¿ØÖÆ´¬");
        boatMove.Drive = true;
    }

    public override void OnInteractExit()
    {
        base.OnInteractExit();
        Debug.Log("¶æ£ºÍ£Ö¹¿ØÖÆ´¬");
        boatMove.Drive = false;
    }

    public void OnInputUpdate()
    {
        boatMove.HandleInputExternally();
    }
}