using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    [SerializeField] private GameObject aimLineDotPrefab;
    [SerializeField] private int dotsAmount = 15;
    [SerializeField] private float dotDistance = 0.5f;

    [SerializeField] private float maxHeight = 0.3f;
    [SerializeField] private float cutT = 0.75f;   // 只走到抛物线的 75%
    [SerializeField] private float forwardStretch = 1.1f;
    private float scrollSpeed = 0.05f; // 点前进速度
    private float scrollT = 0f;

    [SerializeField] private Transform aimLineDotsTranform;

    private List<Transform> dots = new List<Transform>();

    private void Awake()
    {
        GenerateDots();

        UpdateDotsTransform();
    }

    private void FixedUpdate()
    {
        scrollT += scrollSpeed * Time.deltaTime;
        scrollT %= cutT;  // 保持在抛物线t区间内循环
        UpdateDotsTransform();
    }


    private void GenerateDots()
    {
        for (int i = 0; i < dotsAmount; i++)
        {
            GameObject dot = Instantiate(aimLineDotPrefab, aimLineDotsTranform);
            Vector3 LocalLocation = transform.position;
            //dot.transform.localPosition = new Vector3(0f, dotDistance * i, -0.0f);   // 设置相对位置（局部坐标）

            dots.Add(dot.transform);
        }
    }

    private void UpdateDotsTransform()
    {
        float totalForwardLength = dotDistance * (dotsAmount - 1) * forwardStretch;

        // 预先存储点的t值，方便排序
        float[] tValues = new float[dotsAmount];

        for (int i = 0; i < dotsAmount; i++)
        {
            float baseT = (i / (float)(dotsAmount - 1)) * cutT;
            float t = baseT + scrollT;
            if (t > cutT) t -= cutT;

            float forward = totalForwardLength * (t / cutT);
            float height = maxHeight * (1f - Mathf.Pow(Mathf.Abs(2f * t - 1f), 2.5f));
            dots[i].localPosition = new Vector3(0f, height, forward);

            // 根据 t 动态计算透明度，t在0~cutT之间，alpha 0~1
            float alpha = 1 - (t / cutT);

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
    }


}
