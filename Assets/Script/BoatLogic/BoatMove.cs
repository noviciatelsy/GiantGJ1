using UnityEngine;

public class BoatMove : MonoBehaviour
{
   public static BoatMove Instance { get; private set; }

    //船移速参数
    public float baseForwardSpeed = 15f;    // 纵向默认速度
    public float maxForwardSpeed = 20f;     // W键加速最大纵向速度
    public float minForwardSpeed = 12f;     // S键减速最小纵向速度
    public float maxHorizontalSpeed = 10f;  // 横向最大速度（A,D控制）
    public float acceleration = 20f;        // 加速度
    public float deceleration = 15f;        // 减速度（无输入时恢复用）

    //船倾斜参数
    private float maxTiltAngle = 4f;         // 最大倾斜角度
    private float targetTiltAngle;          // 目标倾斜角度
    private float currentTiltAngle;         // 当前船的倾斜角度

    //船受击参数
    [Header("Impulse")]
    public float impulseDamping = 40f; // 冲量衰减速度（越大衰减越快）
    private Vector3 impulseVelocity = Vector3.zero;


    private Vector3 velocity = Vector3.zero;
    private float currentForwardSpeed;
    private float currentHorizontalSpeed;

    private PLControl currentDriver; // 当前驾驶的玩家

    private void Awake()
    {
        // -------- 单例处理 --------
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // 如果船需要跨场景，打开这行
        // DontDestroyOnLoad(gameObject);
    }

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

        //GameData.BoatVelocity = velocity;
        // 冲量衰减
        impulseVelocity = Vector3.MoveTowards(
            impulseVelocity,
            Vector3.zero,
            impulseDamping * Time.deltaTime
        );

        // 最终速度 = 控制速度 + 冲量
        Vector3 finalVelocity = velocity + impulseVelocity;
        GameData.BoatVelocity = finalVelocity;
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
        if (currentHorizontalSpeed != 0)
        {
            targetTiltAngle = Mathf.Clamp(-currentHorizontalSpeed / maxHorizontalSpeed * maxTiltAngle, -maxTiltAngle, maxTiltAngle);
        }
        else
        {
            targetTiltAngle = 0f;  // 无横向速度时，船回到水平
        }
        currentTiltAngle = targetTiltAngle;
        transform.rotation = Quaternion.Euler(0f, -currentTiltAngle/2, currentTiltAngle);
    }

    public void AddImpulse(Vector3 impulse)
    {
        impulseVelocity += impulse;
    }

}