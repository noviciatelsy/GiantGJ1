using System.Collections.Generic;
using UnityEngine;

public class LandPoint : MonoBehaviour
{
    public float rotationSpeed = 30f;  // 旋转速度（度/秒）
    public float radius = 5f;          // 检测范围半径
    public Transform collidertrans;
    private Collider landPointCollider;  // 当前物体的 Collider

    private List<MaterialBase> currentMats = new List<MaterialBase>();

    void Start()
    {
        landPointCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (collidertrans != null)
        {
            // 强制子物体世界旋转
            collidertrans.rotation = Quaternion.Euler(30f, 0f, 0f);
        }
    }

    // 更新物体的旋转
    private void UpdateRotate()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime); // 每秒旋转一定角度
    }

    private void OnTriggerEnter(Collider other)
    {
        MaterialBase mat = other.GetComponent<MaterialBase>();
        if (mat != null)
        {
            if (!currentMats.Contains(mat))
            {
                currentMats.Add(mat);
                Debug.Log("进入落点区域: " + other.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MaterialBase mat = other.GetComponent<MaterialBase>();
        if (mat != null)
        {
            if (currentMats.Remove(mat))
            {
                Debug.Log("离开落点区域: " + other.name);
            }
        }
    }

    public List<MaterialBase> GetMatInLandPoint()
    {
        return currentMats;
    }
}
