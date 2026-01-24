using UnityEngine;

public class StorageCube : CubeBase
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnInteractEnter()
    {
        base.OnInteractEnter();
        Debug.Log("储物箱：开始控制储物箱");

    }

    public override void OnInteractExit()
    {
        base.OnInteractExit();
        Debug.Log("储物箱：停止控制储物箱");

    }

    public override void OnCubeUse()
    {
        base.OnCubeUse();
        //other function
    }

}
