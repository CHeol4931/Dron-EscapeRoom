using UnityEngine;

/// <summary>
/// 지상 드론: W/S 전진·후진, A/D 제자리 회전(탱크 방식).
/// </summary>
public class GroundDrone : DroneBase
{
    [Header("Ground Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float turnSpeed = 120f; // deg/sec

    private Vector2 input;

    private void Update()
    {
        input = IsControlled ? DroneInput.Move : Vector2.zero;
    }

    private void FixedUpdate()
    {
        // Unity 6: linearVelocity / Unity 2022 이하: velocity 로 바꿔주세요
        Vector3 v = rb.linearVelocity;
        Vector3 planar = transform.forward * (input.y * moveSpeed);
        rb.linearVelocity = new Vector3(planar.x, v.y, planar.z); // 중력(y)은 유지

        Quaternion turn = Quaternion.Euler(0f, input.x * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turn);
    }

    protected override void OnControlReleased()
    {
        input = Vector2.zero;
    }
}
