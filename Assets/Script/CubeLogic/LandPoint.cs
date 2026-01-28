using UnityEngine;

public class LandPoint : MonoBehaviour
{
    public float rotationSpeed = 30f;  // 旋转速度（度/秒）
    public float radius = 5f;          // 检测范围半径

    private Collider landPointCollider;  // 当前物体的 Collider

    void Start()
    {
        landPointCollider = GetComponent<Collider>();
    }

    void Update()
    {
        UpdateRotate();  // 持续旋转
        GetObjectInCollider();
    }

    // 更新物体的旋转
    private void UpdateRotate()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime); // 每秒旋转一定角度
    }

    // 获取当前物体的 Collider 内的所有物体
    public void GetObjectInCollider()
    {
        if (landPointCollider == null)
        {
            landPointCollider = GetComponent<Collider>();
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var hitCollider in hitColliders)
        {
            Debug.Log("Collided with: " + hitCollider.gameObject.name);
            // 你可以在这里对重叠的物体进行其他操作
        }
    }
}
