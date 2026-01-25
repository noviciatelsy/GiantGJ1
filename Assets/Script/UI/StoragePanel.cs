using UnityEngine;

public class StoragePanel : MonoBehaviour
{
    public UI_ItemToolTip itemToolTip {  get; private set; }

    private void Awake()
    {
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
    }

}
