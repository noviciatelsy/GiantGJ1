using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimLine : MonoBehaviour
{
    [Header("Aimline参数")]
    [SerializeField] private GameObject aimLineDotPrefab;
    [SerializeField] public int dotsAmount = 12;
    //[SerializeField] private float dotDistance = 0.35f;
    [Header("Dynamic Length")]
    public float currentLength = 1.5f;   // 当前瞄准线长度
    public bool isLand = false; //是否落地

    [Header("抛物线参数")]
    private float maxHeight = 0.3f;
    [SerializeField] private float cutT = 0.75f;   // 只走到抛物线的 75%
    private float scrollSpeed = 0.3f; // 点前进速度
    private float scrollT = 0f;

    [SerializeField] private Transform aimLineDotsTranform;

    private List<Transform> dots = new List<Transform>();
    private int lastDotsAmount; //动态修改dots用到

    [Header("Land Point")]
    [SerializeField] private GameObject landPointPrefab;
    private GameObject landPointInstance;

    private void Awake()
    {
        lastDotsAmount = dotsAmount;
        maxHeight = currentLength * 0.1f;
        if (isLand) maxHeight *= 3;

        GenerateDots();
        UpdateDotsTransform();

        if (isLand)
        {
            CreateLandPoint();
            cutT = 1.0f;
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log(currentLength);
        dotsAmount = (int)(currentLength * 4);

        // === 检测 dotsAmount 是否被动态修改 ===
        if (dotsAmount != lastDotsAmount)
        {
            RebuildDots();
            lastDotsAmount = dotsAmount;
        }

        float tSpeed = scrollSpeed / currentLength;
        scrollT += tSpeed * Time.deltaTime;
        scrollT %= cutT;  // 只在未落地时循环

        UpdateDotsTransform();
    }



    private void GenerateDots()
    {
        for (int i = 0; i < dotsAmount; i++)
        {
            GameObject dot = Instantiate(aimLineDotPrefab, aimLineDotsTranform);
            //Vector3 LocalLocation = transform.position;
            //dot.transform.localPosition = new Vector3(0f, dotDistance * i, -0.0f);   // 设置相对位置（局部坐标）

            dots.Add(dot.transform);
        }
    }

    private void CreateLandPoint()
    {
        if (landPointPrefab == null) return;

        landPointInstance = Instantiate(landPointPrefab, aimLineDotsTranform);

        UpdateLandPoint();
    }


    private void UpdateDotsTransform()
    {
       
        float totalForwardLength = currentLength;

        for (int i = 0; i < dotsAmount; i++)
        {
            float baseT = (i / (float)(dotsAmount - 1)) * cutT;
            float t = baseT + scrollT;
            if (t > cutT) t -= cutT;

            float forward = totalForwardLength * (t / cutT);
            float height = maxHeight * (1f - Mathf.Pow(Mathf.Abs(2f * t - 1f), 2.5f));
            dots[i].localPosition = new Vector3(0f, height, forward);

            // 根据 t 动态计算透明度，t在0~cutT之间，alpha 0~1
            float alpha = 1.0f;
            if (isLand)
            {
                alpha = 1.4f - (t / cutT);
            }
            else alpha = 1 - (t / cutT);

            SpriteRenderer sr = dots[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }
        }


        // 计算旋转
        for (int i = 0; i < dotsAmount - 1; i++)
        {
            Vector3 p0 = dots[i].localPosition;
            Vector3 p1 = dots[i + 1].localPosition;
            Vector3 dir = p1 - p0;

            if (dir.sqrMagnitude < 0.0001f) continue;

            float pitchDeg = Mathf.Atan2(dir.z, dir.y) * Mathf.Rad2Deg;
            dots[i].localRotation = Quaternion.Euler(pitchDeg, 0f, 90f);
        }

        dots[dotsAmount - 1].localRotation = dots[dotsAmount - 2].localRotation;

        if (isLand)
        {
            UpdateLandPoint();
        }


    }

    private void UpdateLandPoint()
    {
        if (landPointInstance == null) return;

        // 计算 LandPoint 的 y 轴位置
        // 获取当前抛物线末尾点的高度
        float t = cutT;  // 使用cutT，表示轨迹的最终时间点
        float height = maxHeight * (1f - Mathf.Pow(Mathf.Abs(2f * t - 1f), 2.5f));  // 计算高度
        Vector3 landPointPosition = new Vector3(0f, height, currentLength);  // 计算landpoint的最终位置

        // 计算 LandPoint 的大小比例
        float minScale = 0.3f;
        float maxScale = 0.5f;
        float lengthMin = 1.5f;  // 你之前 mindist
        float lengthMax = 10.0f; // 你之前 maxdist

        // 将 currentLength 映射到 0~1 之间
        float tScale = Mathf.InverseLerp(lengthMin, lengthMax, currentLength);
        float landPointScale = Mathf.Lerp(minScale, maxScale, tScale);

        // 设置位置和缩放
        landPointInstance.transform.localPosition = landPointPosition;
        landPointInstance.transform.localScale = new Vector3(landPointScale, landPointScale, landPointScale);
    }


    private void RebuildDots()
    {
        // 删除旧点
        for (int i = 0; i < dots.Count; i++)
        {
            if (dots[i] != null)
                Destroy(dots[i].gameObject);
        }

        dots.Clear();

        // 重新计算高度
        maxHeight = maxHeight = currentLength * 0.1f;
        if (isLand) maxHeight *= 3;

        // 重新生成
        GenerateDots();
    }

}