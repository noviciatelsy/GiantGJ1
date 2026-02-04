using UnityEngine;

public class EmptyCube : CubeBase
{
    public override void OnEasyInteract(PLControl interactPlayer)
    {
        base.OnEasyInteract(interactPlayer);
        //if(interactPlayer == null )
        //{
        //    return;
        //}
        //if(interactPlayer.cubeToEquip==null )
        //{
        //    return;
        //}
        //CubeItemDataSO cubeData = interactPlayer.cubeToEquip;
        //string equipMessage = "已装备：" + cubeData.itemName;
        //HintMessage.Instance.ShowQuickMessage( equipMessage );
        //interactPlayer.ResetCubeToEquip(); // 清除人物待装配的浮块
        //HintMessage.Instance.StopLongTimeMessage();
        //ReplaceSelf(cubeData.cubePrefab);
    }

    public override void ReplaceSelf(GameObject newPrefab)
    {
        base.ReplaceSelf(newPrefab);
        //if (newPrefab == null)
        //{
        //    return ;
        //}
        //CubeBase cubeBase = newPrefab.GetComponent<CubeBase>();
        //int boatsizey = BoatManager.Instance.cubesize.y;
        //BoatManager.Instance.cubeDatas[cubePos.x * boatsizey + cubePos.y] = cubeBase.cubeData;
        //cubeBase.cubePos = this.cubePos;
        //Transform oldTransform = transform;

        //// 记录旧对象的“场景信息”
        //Vector3 worldPos = oldTransform.position-positionOffset;
        //Quaternion worldRot = oldTransform.rotation;
        //Vector3 localScale = oldTransform.localScale;

        //Transform parent = oldTransform.parent;
        //int siblingIndex = oldTransform.GetSiblingIndex();
        //bool wasActive = gameObject.activeSelf;

        //// 生成新对象（先生成，再设置父级与位置）
        //GameObject newObj = Instantiate(newPrefab);

        //// 先设置父级（保持世界坐标不变）
        //newObj.transform.SetParent(parent, true);

        //// 对齐 Transform
        //newObj.transform.position = worldPos;
        //newObj.transform.rotation = worldRot;
        //newObj.transform.localScale = localScale;

        //// 保持层级顺序一致
        //newObj.transform.SetSiblingIndex(siblingIndex);


        //// 继承激活状态
        //newObj.SetActive(wasActive);

        //// 最后销毁旧对象
        //Destroy(gameObject);

        //return ;
    }

}
