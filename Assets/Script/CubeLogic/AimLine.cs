using UnityEngine;

public class AimLine : MonoBehaviour
{
    [SerializeField] private GameObject aimLineDotPrefab;
    [SerializeField] private int dotsAmount = 20;
    [SerializeField] private float dotDistance = 0.5f;
    [SerializeField] private Transform aimLineDotsTranform;

    private void Awake()
    {
        GenerateDots();
    }

    private void GenerateDots()
    {
        for (int i = 0; i < dotsAmount; i++)
        {
            GameObject dot = Instantiate(aimLineDotPrefab, aimLineDotsTranform); 
            dot.transform.localPosition = new Vector3(0f, dotDistance * i, -0.1f);   // 设置相对位置（局部坐标）
        }
    }

}
