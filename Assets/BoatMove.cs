using UnityEngine;

public class BoatMove : MonoBehaviour
{
    public float baseForwardSpeed = 15f;    // 纵向默认速度
    public float maxForwardSpeed = 20f;     // W键加速最大纵向速度
    public float minForwardSpeed = 12f;     // S键减速最小纵向速度
    public float maxHorizontalSpeed = 10f;  // 横向最大速度（A,D控制）

    public float acceleration = 20f;        // 加速度
    public float deceleration = 15f;        // 减速度（无输入时恢复用）

    private Vector3 velocity = Vector3.zero;
    private float currentForwardSpeed;
    private float currentHorizontalSpeed;
    
    public bool Drive = false;
    void Start()
    {
        currentForwardSpeed = baseForwardSpeed;  // 初始纵向速度
        currentHorizontalSpeed = 0f;              // 初始横向速度
        Drive = false;
    }

    void FixedUpdate()
    {

        if (Drive)
        {
            HandleInputExternally(); // 有人操控，读输入
        }
        else
        {
            ApplyInput(0f, 0);       // 无输入 → 回到基础速度
        }

        GameData.BoatVelocity = velocity;
    }


    public void HandleInputExternally()
    {
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;

        int verticalInput = 0;
        if (Input.GetKey(KeyCode.W)) verticalInput = 1;
        else if (Input.GetKey(KeyCode.S)) verticalInput = -1;

        ApplyInput(horizontalInput, verticalInput);
    }

    private void ApplyInput(float horizontalInput, int verticalInput)
    {
        // 横向输入（-1到1）
        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;
        // 纵向输入 -1 (S), 0 (无输入), 1 (W)
        if (Input.GetKey(KeyCode.W)) verticalInput = 1;
        else if (Input.GetKey(KeyCode.S)) verticalInput = -1;

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

}