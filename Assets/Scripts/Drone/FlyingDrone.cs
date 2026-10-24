using UnityEngine;

/// <summary>
/// 비행 드론: WASD 이동, Space/LeftCtrl 상승·하강, 마우스 좌우로 회전.
/// 조종하지 않을 때는 그 자리에서 호버링.
/// </summary>
public class FlyingDrone : DroneBase
{
    [Header("Flight Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private float yawSpeed = 3f;
    [SerializeField] private float acceleration = 8f; // 값이 클수록 반응이 빠름

    private Vector2 move;
    private float vertical;
    private float yaw;

    protected override void Awake()
    {
        base.Awake();
        rb.useGravity = false;       // 공중에 떠 있어야 하므로
        rb.angularDamping = 5f;      
    }

    private void Update()
    {
        if (!IsControlled) { move = Vector2.zero; vertical = 0f; yaw = 0f; return; }
        move = DroneInput.Move;
        vertical = DroneInput.Vertical;
        yaw = DroneInput.Yaw;
    }

    private void FixedUpdate()
    {
        Vector3 target =
            transform.right * (move.x * moveSpeed) +
            transform.forward * (move.y * moveSpeed) +
            Vector3.up * (vertical * climbSpeed);

        // 부드러운 가감속 (입력이 없으면 target=0 → 자연스럽게 멈춰 호버링)
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, target, acceleration * Time.fixedDeltaTime);

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, yaw * yawSpeed, 0f));
    }

    protected override void OnControlReleased()
    {
        move = Vector2.zero; vertical = 0f; yaw = 0f;
    }
}
