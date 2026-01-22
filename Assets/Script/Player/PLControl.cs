using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PLControl : MonoBehaviour
{
    // 移动
    private float playerSpeed = 15f;
    public Vector2Int boatSize = new Vector2Int(3, 3); // x = 宽，y = 长
    public Vector2 moveLimitX = new Vector2(3f, 3f);
    public Vector2 moveLimitY = new Vector2(3f, 3f);

    // 交互
    private bool isInteracting = false;
    private bool longPressTriggered;
    private Vector2 moveInput; // 缓存的移动输入（来自 PlayerInput 的回调）

    // currentCube判定
    private CubeBase currentCube;
    private readonly List<CubeBase> nearbyCubes = new List<CubeBase>(); // 所有进入交互范围的 Cube
    private float refreshCurrentCubeInterval = 0.1f;
    private float refreshCurrentCubeITimer=0f;


    void Start()
    {
        TransMoveLimit();
    }

    void FixedUpdate()
    {
        HandlePLMove();
    }

    void Update()
    {
        FindNearestCube();
    }

    public void TransMoveLimit()
    {
        int isodd = boatSize.y % 2;
        moveLimitY.x = -5;
        moveLimitY.y = -5 + boatSize.x * 10;
        moveLimitX.x = -boatSize.y / 2 * 10 - isodd * 5;
        moveLimitX.y = boatSize.y / 2 * 10 + isodd * 5;
        Debug.Log("PL Move Limit X: " + moveLimitX);
        Debug.Log("PL Move Limit Y: " + moveLimitY);
    }


    private void HandlePLMove()
    {
        Vector3 delta = new Vector3(
            moveInput.x * playerSpeed * Time.fixedDeltaTime,
            0f,
            moveInput.y * playerSpeed * Time.fixedDeltaTime
        );

        Vector3 targetPos = transform.position + delta;

        // Clamp 限制范围
        targetPos.x = Mathf.Clamp(
            targetPos.x,
            moveLimitX.x,
            moveLimitX.y
        );
        targetPos.z = Mathf.Clamp(
            targetPos.z,
            moveLimitY.x,
            moveLimitY.y
        );

        transform.position = targetPos;
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        // 在于浮块交互时不处理其他输入
        if (isInteracting)
        {
            moveInput = Vector2.zero;
            return;
        }

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
        // 1) started：按下那一刻（不做决定，只重置状态）
        if (context.started)
        {
            longPressTriggered = false;
            return;
        }

        // 2) performed：Hold 达成那一刻（长按成功）
        if (context.performed)
        {
            longPressTriggered = true;
            currentCube.OnRepair(); // 长按触发修理
            return;
        }

        // 3) canceled：松开 / 被取消
        if (context.canceled)
        {
            if (longPressTriggered) // 如果触发了长按修理
            {
                return;
            }
            else
            {
                HandleShortPress();
            }
        }
    }

    private void HandleShortPress()
    {
        if (currentCube == null) return; // 如果脚下没有浮块
        if(currentCube.currentPlayer!=null&&currentCube.currentPlayer!=this)
        {
            return;
        } //  如果其他玩家正在操作该浮块
        Debug.Log("SHort Press");
        if (!isInteracting)
        {
            currentCube.SetCurrentPlayer( this); // 设置浮块所有权
            currentCube.IsChoose();
            currentCube.OnInteractEnterBase();

            isInteracting = true;
        }
        else
        {
            currentCube.SetCurrentPlayer(null); // 还原浮块所有权

            currentCube.OnInteractExit();
            isInteracting = false;
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
                 new Vector2( transform.position.x,transform.position.z),
                new Vector2( cube.transform.position.x,cube.transform.position.z)
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
        if(refreshCurrentCubeITimer<refreshCurrentCubeInterval)
        {
            refreshCurrentCubeITimer += Time.deltaTime;
            return;
        }
        else
        {
            RefreshCurrentCube();
            refreshCurrentCubeITimer = 0;
        }
    }

}
