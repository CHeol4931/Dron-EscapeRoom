using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// 1인칭 플레이어: WASD 이동 + 마우스 시점.
/// enabled를 false로 끄면 이동과 시점이 함께 멈춘다 (DroneControlManager가 그렇게 사용).
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot;   // 플레이어 카메라(또는 그 부모). 상하 회전 대상

    [Header("Move")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float gravity = -20f;

    [Header("Look")]
    [SerializeField] private float lookSensitivity = 0.1f;
    [SerializeField] private float pitchLimit = 80f;

    private CharacterController cc;
    private float pitch;
    private float verticalVelocity;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // 게임 시작 시 한 번만 잠금. 이후 커서 상태는 DroneControlManager가 관리
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
        Move();
    }

    private void Look()
    {
        Vector2 delta = ReadLookDelta() * lookSensitivity;

        transform.Rotate(0f, delta.x, 0f);

        pitch = Mathf.Clamp(pitch - delta.y, -pitchLimit, pitchLimit);
        if (cameraPivot != null)
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void Move()
    {
        Vector2 input = DroneInput.Move;
        Vector3 horizontal = (transform.right * input.x + transform.forward * input.y);
        if (horizontal.sqrMagnitude > 1f) horizontal.Normalize();

        // 바닥에 붙어 있게 살짝 눌러주고, 공중이면 중력 적용
        if (cc.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = horizontal * moveSpeed + Vector3.up * verticalVelocity;
        cc.Move(velocity * Time.deltaTime);
    }

    private static Vector2 ReadLookDelta()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
#else
        // 구 Input Manager는 Mouse X/Y가 이미 프레임당 이동량이라 감도를 크게 곱해줘야 함
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * 10f;
#endif
    }
}
