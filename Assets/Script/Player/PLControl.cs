using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PLControl : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioEventSO footStepSFX;
    public AudioSource footStepAudioSource {  get; private set; }
    [SerializeField] private AudioEventSO interactSFX;

    // 移动
    private float playerSpeed = 15f;
    //public Vector2Int boatSize = new Vector2Int(3, 3); // x = 宽，y = 长
    //public Vector2 moveLimitX = new Vector2(3f, 3f);
    //public Vector2 moveLimitY = new Vector2(3f, 3f);
    private Rigidbody rb;

    // 交互
    private bool isInteracting = false;
    private bool longPressTriggered;
    private InteractableScanner interactableScanner;

    private Animator animator;
    public Vector2 moveInput { get; private set; } // 缓存的移动输入（来自 PlayerInput 的回调）

    // currentCube判定
    private CubeBase currentCube;
    private readonly List<CubeBase> nearbyCubes = new List<CubeBase>(); // 所有进入交互范围的 Cube
    private float refreshCurrentCubeInterval = 0.1f;
    private float refreshCurrentCubeITimer = 0f;

    // 局内UI
    private CubeDetailsUI currentCubeDetailsUI;

    // 浮块替换
    public CubeItemDataSO cubeToEquip { get; private set; }
    [SerializeField] private SpriteRenderer cubeToEquipSprite;
    [SerializeField] private Sprite emptySprite;

    // 场景切换
    private bool isOnBoat;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb=GetComponent<Rigidbody>();
        interactableScanner = GetComponent<InteractableScanner>();
    }

    //void Start()
    //{
    //    TransMoveLimit();
    //}

    void FixedUpdate()
    {
        HandlePLMove();
    }

    void Update()
    {
        if(isOnBoat)
        {
            FindNearestCube();
        }

    }

    //public void TransMoveLimit()
    //{
    //    int isodd = boatSize.y % 2;
    //    moveLimitY.x = 2f;
    //    moveLimitY.y = 2f + boatSize.x * 10;
    //    moveLimitX.x = -boatSize.y / 2 * 10 - isodd * 5;
    //    moveLimitX.y = boatSize.y / 2 * 10 + isodd * 5;
    //    //Debug.Log("PL Move Limit X: " + moveLimitX);
    //    Debug.Log("PL Move Limit Y: " + moveLimitY);
    //}


    //private void HandlePLMove()
    //{
    //    if (isInteracting) // 交互阶段强制静止
    //    {
    //        animator.SetBool("isMoving", false);
    //        if (footStepAudioSource != null)
    //        {
    //            footStepSFX.StopLoop(footStepAudioSource);
    //            footStepAudioSource = null;
    //        }
    //        return;
    //    }
    //    if (moveInput != Vector2.zero)
    //    {
    //        animator.SetBool("isMoving", true);
    //        if (footStepAudioSource == null)
    //        {
    //            footStepAudioSource = footStepSFX.PlayLoop2D();
    //        }
    //    }
    //    else
    //    {
    //        animator.SetBool("isMoving", false);
    //        if (footStepAudioSource != null)
    //        {
    //            footStepSFX.StopLoop(footStepAudioSource);
    //            footStepAudioSource = null;
    //        }
    //    }
    //    Vector3 delta = new Vector3(
    //        moveInput.x * playerSpeed * Time.fixedDeltaTime,
    //        0f,
    //        moveInput.y * playerSpeed * Time.fixedDeltaTime
    //    );


    //    Vector3 targetPos = transform.position + delta;

    //    // Clamp 限制范围
    //    //targetPos.x = Mathf.Clamp(
    //    //    targetPos.x,
    //    //    moveLimitX.x,
    //    //    moveLimitX.y
    //    //);
    //    //targetPos.z = Mathf.Clamp(
    //    //    targetPos.z,
    //    //    moveLimitY.x,
    //    //    moveLimitY.y
    //    //);

    //    //transform.position = targetPos;
    //    rb.MovePosition(targetPos);
    //}

    private void HandlePLMove()
    {
        if (isInteracting) // 交互阶段强制静止
        {
            animator.SetBool("isMoving", false);
            if (footStepAudioSource != null)
            {
                footStepSFX.StopLoop(footStepAudioSource);
                footStepAudioSource = null;
            }

            // 交互时强制停下（避免惯性滑动）
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }
        if (moveInput != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            if (footStepAudioSource == null)
            {
                footStepAudioSource = footStepSFX.PlayLoop2D();
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            if (footStepAudioSource != null)
            {
                footStepSFX.StopLoop(footStepAudioSource);
                footStepAudioSource = null;
            }
        }

        // 输入转成世界速度（注意：velocity 不需要乘 Time.fixedDeltaTime）
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);

        // 防止斜向移动更快
        if (moveDir.sqrMagnitude > 1f)
        {
            moveDir.Normalize();
        }

        Vector3 desiredVelocity = moveDir * playerSpeed;

        // 预测本帧会不会撞墙（避免“挤过去”）
        if (moveDir != Vector3.zero)
        {
            RaycastHit hitInfo;

            // 本帧最多会走的距离（用于 SweepTest 的检测长度）
            float moveDistance = playerSpeed * Time.fixedDeltaTime;

            // 留一点安全边（别设 0，容易抖/卡）
            float skinWidth = 0.03f;

            // SweepTest：测试刚体沿方向移动时是否会撞到东西
            // QueryTriggerInteraction.Ignore：忽略 Trigger（空气墙要挡人就别做 Trigger）
            if (rb.SweepTest(moveDir, out hitInfo, moveDistance + skinWidth, QueryTriggerInteraction.Ignore))
            {
                // 把“朝墙里”的速度分量去掉，只留下沿墙滑动的分量
                desiredVelocity = Vector3.ProjectOnPlane(desiredVelocity, hitInfo.normal);
            }
        }

        // 只控制水平速度，保留 y 轴速度（例如重力/跳跃）
        rb.linearVelocity= new Vector3(desiredVelocity.x, rb.linearVelocity.y, desiredVelocity.z);

        // Clamp 限制范围
        //targetPos.x = Mathf.Clamp(
        //    targetPos.x,
        //    moveLimitX.x,
        //    moveLimitX.y
        //);
        //targetPos.z = Mathf.Clamp(
        //    targetPos.z,
        //    moveLimitY.x,
        //    moveLimitY.y
        //);

        //transform.position = targetPos;
        //rb.MovePosition(targetPos);
    }


    public void OnMovement(InputAction.CallbackContext context)
    {

        // Value/Vector2 的 Action：
        // performed：有输入变化（非零/变化）
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
            return;
        }

        // canceled：输入归零（松开键或摇杆回中）
        if (context.canceled)
        {
            moveInput = Vector2.zero;
            return;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(isOnBoat) // 船上交互行为
        {
            // 1) started：按下那一刻（不做决定，只重置状态）
            if (context.started)
            {
                longPressTriggered = false;
                if (!isInteracting && currentCube.GetCurrentFixingPlayer() == null)
                // 如果在非互动阶段且玩家没有在修理该浮块
                {
                    currentCube.OnRepairBegin(this); // 标记为开始累计修理时间
                }
                return;
            }

            // 2) performed：达到performed阶段后不再被视为短按
            if (context.performed)
            {
                longPressTriggered = true;
                return;
            }

            // 3) canceled：松开 / 被取消
            if (context.canceled)
            {
                if (!isInteracting) // 如果在非互动阶段
                {
                    currentCube.OnRepairEnd(); // 标记为停止累计修理时间
                }
                if (longPressTriggered) // 如果触发了长按
                {
                    return;
                }
                else
                {
                    HandleShortPress();
                }
            }
        }
        else
        {
            if(interactableScanner.currentInteractableObject != null)
            {
                interactableScanner.currentInteractableObject.Interact();
            }
        }

        
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if(isOnBoat)
        {
            if (!isInteracting)
            {
                return;
            }
            if (currentCube.currentPlayer != this)
            {
                return;
            }
            if (currentCube.CheckCubeHealth() == 0) return; // 完全损坏就无法使用
            if (context.performed)
            {
                currentCube.OnCubeUse();
            }
            else if (context.canceled)
            {
                currentCube.EndCubeUse();
            }
        }

       
    }

    public void OnCancel(InputAction.CallbackContext contex)
    {
        if (isOnBoat)
        {
            if (cubeToEquip == null)
            {
                return;
            }
            if (contex.performed) // 当取消装备时
            {
                HintMessage.Instance.StopLongTimeMessage();
                StorageManager.Instance.GetItem(cubeToEquip, 1);
                ResetCubeToEquip();
            }
        }
     
    }

    private void HandleShortPress()
    {
        if (currentCube == null) return; // 如果脚下没有浮块
        if (currentCube.currentPlayer != null && currentCube.currentPlayer != this)
        {
            return;
        } //  如果其他玩家正在操作该浮块
        if (currentCube.allowInteract == false) // 如果该浮块不支持交互
        {
            return;
        }
        if (currentCube.CheckCubeHealth() == 0) return; // 完全损坏就无法交互
        if (currentCube.shouldMoveCurrentPlayerToCentre)
        {
            if (!isInteracting)
            {
                currentCube.SetCurrentPlayer(this); // 设置浮块所有权
                currentCube.MoveCurrentPlayerToCentre(); // 将玩家移至浮块中心
                currentCube.OnInteractEnter();
                isInteracting = true;
                interactSFX.Play();
            }
            else
            {
                currentCube.SetCurrentPlayer(null); // 还原浮块所有权
                currentCube.OnInteractExit();
                isInteracting = false;
            }
        }
        else
        {
            currentCube.OnEasyInteract(this);
        }

    }

    

    private void OnTriggerEnter(Collider other)
    {
        CubeBase cube = other.GetComponent<CubeBase>();
        if (cube == null) return;

        if (!nearbyCubes.Contains(cube))
        {
            nearbyCubes.Add(cube);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        CubeBase cube = other.GetComponent<CubeBase>();
        if (cube == null) return;

        if (nearbyCubes.Contains(cube))
        {
            nearbyCubes.Remove(cube);
        }

        // 如果正在交互的 Cube 被移除，强制退出
        if (cube == currentCube && isInteracting)
        {
            cube.OnInteractExit();
            isInteracting = false;
        }
    }


    private void RefreshCurrentCube()
    {
        CubeBase nearest = null;
        float minDist = float.MaxValue;

        foreach (var cube in nearbyCubes)
        {
            if (cube == null) continue;

            float dist = Vector2.Distance(
                 new Vector2(transform.position.x, transform.position.z),
                new Vector2(cube.transform.position.x, cube.transform.position.z)
            );

            if (dist < minDist)
            {
                minDist = dist;
                nearest = cube;
            }
        }

        // 如果最近的 Cube 发生变化
        if (nearest != currentCube)
        {

            // 旧的取消高亮
            if (currentCube != null)
            {
                currentCube.OnRepairEnd(); // 结束旧的修理
                currentCube.EndLand();
            }

            currentCube = nearest;

            // 新的高亮
            if (currentCube != null)
            {
                currentCube.OnLand();
            }
        }
    }

    private void FindNearestCube()
    {
        if (refreshCurrentCubeITimer < refreshCurrentCubeInterval)
        {
            refreshCurrentCubeITimer += Time.deltaTime;
            return;
        }
        else
        {
            RefreshCurrentCube();
            if(currentCube != null) 
            currentCubeDetailsUI.UpdateCurrentCubeDetails(currentCube.cubeData, currentCube.CheckCubeHealth()); // 刷新浮块UI
            refreshCurrentCubeITimer = 0;
        }
    }

    public void SetCubeDetailsUI(CubeDetailsUI cubeDetails)
    {
        currentCubeDetailsUI = cubeDetails;
    }

    public void SetCubeToEquip(CubeItemDataSO cubeData)
    {
        cubeToEquip = cubeData;
        cubeToEquipSprite.sprite=cubeData.spriteToShow;
    }

    public void ResetCubeToEquip()
    {
        cubeToEquip = null;
        cubeToEquipSprite.sprite = emptySprite;
    }

    public void SetPlayerInteractingState(bool isInteracting)
    {
        this.isInteracting = isInteracting;
    }

    public void SetIsOnBoat(bool isOnBoat)
    {
        this.isOnBoat = isOnBoat;
    }
}
