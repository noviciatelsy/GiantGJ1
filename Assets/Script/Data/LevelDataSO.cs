using UnityEngine;

[CreateAssetMenu(menuName = "Level/Level Option", fileName = "LevelOption - ")]
public class LevelDataSO : ScriptableObject
{
    public string levelSceneName; // 用于切换至对应关卡的场景名
    public string levelDisplayName; // 用于展示给玩家看的关卡名
    public string levelDescription; // 用于展示给玩家看的关卡描述
    public bool isNormalRoute; // 是否为普通路线
}
