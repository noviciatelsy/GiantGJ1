using UnityEngine;

using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;


public class PLControl : MonoBehaviour
{

    private float playerSpeed = 15f;

    public Vector2Int boatSize = new Vector2Int(3, 3); // x = 宽，y = 长
    public Vector2 moveLimitX = new Vector2(3f, 3f);
    public Vector2 moveLimitY = new Vector2(3f, 3f);


    public float interactRange = 2.5f;
    private CubeBase currentCube;
    private bool isInteracting = false;

    private float eHoldTime = 0f;
    private float longPressTime = 0.4f;//识别长按时间
    private readonly List<CubeBase> nearbyCubes = new List<CubeBase>(); // 所有进入交互范围的 Cube


    void Start()
    {
        TransMoveLimit();
    }

    void Update()
    {
        HandleInput();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        HandlePLMove();
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


    private void HandleInput()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            eHoldTime = 0f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            eHoldTime += Time.deltaTime;

            // 长按 → 修理
            if (eHoldTime >= longPressTime && currentCube != null)
            {
                currentCube.OnRepairStart();
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            // 松开时，如果是短按
            if (eHoldTime < longPressTime)
            {
                HandleShortPress();
            }
            else
            {
                currentCube?.OnRepairEnd();
            }

            eHoldTime = 0f;
        }
    }

    private void HandlePLMove()
    {
        if (isInteracting) return; //在于浮块交互时不处理其他输入

        //处理wasd移动
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;
        int verticalInput = 0;
        if (Input.GetKey(KeyCode.W)) verticalInput = 1;
        else if (Input.GetKey(KeyCode.S)) verticalInput = -1;

        Vector3 delta = new Vector3(
            horizontalInput * playerSpeed * Time.fixedDeltaTime,
            0f,
            verticalInput * playerSpeed * Time.fixedDeltaTime
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

    private void HandleShortPress()
    {
        if (currentCube == null) return;

        if (!isInteracting)
        {
            currentCube.CurrentPlayer = this; //单人逻辑

            currentCube.IsChoose();
            currentCube.OnInteractEnterBase();
            isInteracting = true;
        }
        else
        {
            currentCube.CurrentPlayer = null; //单人逻辑

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

        RefreshCurrentCube();
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

        RefreshCurrentCube();
    }


    private void RefreshCurrentCube()
    {
        CubeBase nearest = null;
        float minDist = float.MaxValue;

        foreach (var cube in nearbyCubes)
        {
            if (cube == null) continue;

            float dist = Vector3.Distance(
                transform.position,
                cube.transform.position
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

}
