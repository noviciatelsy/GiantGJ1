using UnityEngine;

public class InteractToolTipMove : MonoBehaviour
{

    [Header("Floaty ToolTip")]
    [SerializeField] private float floatSpeed = 8; // 上限浮动速度
    [SerializeField] private float floatRange = 0.1f; // 上下浮动范围
    private Vector3 startPosition; // 初始位置

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset);
    }
}
