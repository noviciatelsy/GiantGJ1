using UnityEngine;

public class EmptyCube : CubeBase
{
    public override void OnEasyInteract(PLControl interactPlayer)
    {
        base.OnEasyInteract(interactPlayer);
        if(interactPlayer == null )
        {
            return;
        }
        if(interactPlayer.cubeToEquip==null )
        {
            return;
        }
        CubeItemDataSO cubeData = interactPlayer.cubeToEquip;  
        interactPlayer.ResetCubeToEquip(); // 清除人物待装配的浮块
        ReplaceSelf(cubeData.cubePrefab);
    }

    private void ReplaceSelf(GameObject newPrefab)
    {
        if (newPrefab == null)
        {
            return ;
        }

        Transform oldTransform = transform;

        // 记录旧对象的“场景信息”
        Vector3 worldPos = oldTransform.position-positionOffset;
        Quaternion worldRot = oldTransform.rotation;
        Vector3 localScale = oldTransform.localScale;

        Transform parent = oldTransform.parent;
        int siblingIndex = oldTransform.GetSiblingIndex();
        bool wasActive = gameObject.activeSelf;

        // 生成新对象（先生成，再设置父级与位置）
        GameObject newObj = Instantiate(newPrefab);

        // 先设置父级（保持世界坐标不变）
        newObj.transform.SetParent(parent, true);

        // 对齐 Transform
        newObj.transform.position = worldPos;
        newObj.transform.rotation = worldRot;
        newObj.transform.localScale = localScale;

        // 保持层级顺序一致
        newObj.transform.SetSiblingIndex(siblingIndex);


        // 继承激活状态
        newObj.SetActive(wasActive);

        // 最后销毁旧对象
        Destroy(gameObject);

        return ;
    }

}
