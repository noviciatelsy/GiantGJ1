using UnityEngine;

public class AutoFixCube : CubeBase
{
    private float timeToAutoFix = 10f;  // 自动修复的速度
    private float autoFixTimer = 0f;  // 自动修复的计时器
    private bool isAutoFixing = false; // 是否正在进行自动修复

    protected override void Awake()
    {
        base.Awake();
        handlefix.canAutoFix = true;
    }

}
