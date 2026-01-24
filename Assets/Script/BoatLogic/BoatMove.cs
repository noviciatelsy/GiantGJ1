using UnityEngine;

public class BoatMove : MonoBehaviour
{
    public float baseForwardSpeed = 15f;    // 纵向默认速度
    public float maxForwardSpeed = 20f;     // W键加速最大纵向速度
    public float minForwardSpeed = 12f;     // S键减速最小纵向速度
    public float maxHorizontalSpeed = 10f;  // 横向最大速度（A,D控制）

    public float acceleration = 20f;        // 加速度
    public float deceleration = 15f;        // 减速度（无输入时恢复用）

    private float maxTiltAngle = 4f;         // 最大倾斜角度
    private float targetTiltAngle;          // 目标倾斜角度
    private float currentTiltAngle;         // 当前船的倾斜角度

    private Vector3 velocity = Vector3.zero;
    private float currentForwardSpeed;
    private float currentHorizontalSpeed;

    private PLControl currentDriver; // 当前驾驶的玩家
    void Start()
    {
        currentForwardSpeed = baseForwardSpeed;  // 初始纵向速度
        currentHorizontalSpeed = 0f;              // 初始横向速度
        ResetCurrentDriver(); // 重置当前驾驶的玩家
    }

    void FixedUpdate()
    {
        if (currentDriver!=null) // 如果当前有玩家驾驶
        {
            ApplyInput(currentDriver.moveInput.x, currentDriver.moveInput.y); // 读取当前玩家的移动输入
        }
        else
        {
            ApplyInput(0f, 0);       // 无输入 → 回到基础速度
        }

        // 更新船的倾斜角度（根据横向速度调整）
        UpdateBoatTilt();

        GameData.BoatVelocity = velocity;
    }



    private void ApplyInput(float horizontalInput, float verticalInput)
    {
        // ----------- 横向速度 -----------
        if (horizontalInput != 0f)
        {
            // 有横向输入，按加速度加速到目标横向速度
            currentHorizontalSpeed = Mathf.MoveTowards(
                currentHorizontalSpeed,
                horizontalInput * maxHorizontalSpeed,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            // 无横向输入，按减速度回到0
            currentHorizontalSpeed = Mathf.MoveTowards(
                currentHorizontalSpeed,
                0f,
                deceleration * Time.deltaTime
            );
        }

        // ----------- 纵向速度 -----------
        float targetForwardSpeed = baseForwardSpeed;

        if (verticalInput == 1) targetForwardSpeed = maxForwardSpeed;
        else if (verticalInput == -1) targetForwardSpeed = minForwardSpeed;

        if (verticalInput != 0)
        {
            // 有纵向输入，按加速度调整速度
            currentForwardSpeed = Mathf.MoveTowards(
                currentForwardSpeed,
                targetForwardSpeed,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            // 无纵向输入，按减速度回到基础速度
            currentForwardSpeed = Mathf.MoveTowards(
                currentForwardSpeed,
                baseForwardSpeed,
                deceleration * Time.deltaTime
            );
        }

        // 合成速度向量
        velocity = new Vector3(currentHorizontalSpeed, 0f, currentForwardSpeed);
    }

    public void SetCurrentDriver(PLControl currentDriver)
    {
        this.currentDriver = currentDriver;
    }

    public void  ResetCurrentDriver()
    {
        currentDriver = null;
    }

    // 更新船的倾斜角度
    private void UpdateBoatTilt()
    {
        // 计算横向速度对应的目标倾斜角度，横向速度越大，倾斜角度越大
        if (currentHorizontalSpeed != 0)
        {
            targetTiltAngle = Mathf.Clamp(-currentHorizontalSpeed / maxHorizontalSpeed * maxTiltAngle, -maxTiltAngle, maxTiltAngle);
        }
        else
        {
            targetTiltAngle = 0f;  // 无横向速度时，船回到水平
        }

        // 直接将目标倾斜角度设置为当前倾斜角度（减少惯性）
        currentTiltAngle = targetTiltAngle;

        // 更新船的旋转，倾斜角度应用到Z轴
        transform.rotation = Quaternion.Euler(0f, -currentTiltAngle/2, currentTiltAngle);
    }

}