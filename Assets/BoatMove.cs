using UnityEngine;

public class BoatMove : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float acceleration = 20f;
    public float deceleration = 15f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 inputDirection = Vector3.zero;

    void FixedUpdate()
    {
        HandleInput();
        UpdateVelocity();

        // 把结果写入全局数据
        GameData.BoatVelocity = velocity;
    }

    void HandleInput()
    {
        inputDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) inputDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) inputDirection += Vector3.back;
        if (Input.GetKey(KeyCode.A)) inputDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D)) inputDirection += Vector3.right;

        inputDirection.Normalize();
    }

    void UpdateVelocity()
    {
        if (inputDirection.sqrMagnitude > 0f)
        {
            velocity = Vector3.MoveTowards(
                velocity,
                inputDirection * maxSpeed,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            velocity = Vector3.MoveTowards(
                velocity,
                Vector3.zero,
                deceleration * Time.deltaTime
            );
        }
    }
}
