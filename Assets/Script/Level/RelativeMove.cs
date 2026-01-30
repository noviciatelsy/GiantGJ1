using UnityEngine;

public class RelativeMove : MonoBehaviour
{
    void FixedUpdate()
    {
        // 世界相对船反向移动
        Vector3 worldMove = -GlobalGameData.BoatVelocity;

        transform.Translate(worldMove * Time.deltaTime, Space.World);
    }
}
