using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// 입력 처리를 한 곳에 모아둔 래퍼.
/// Input System(New) / Input Manager(Old) 어느 쪽이든 동작한다.
/// </summary>
public static class DroneInput
{
#if ENABLE_INPUT_SYSTEM
    public static bool QDown => Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame;
    public static bool RDown => Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame;
    public static bool TDown => Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame;
    public static bool EDown => Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
    public static bool FireDown => Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

    // WASD  (x: A/D, y: W/S)
    public static Vector2 Move
    {
        get
        {
            var k = Keyboard.current;
            if (k == null) return Vector2.zero;
            float x = (k.dKey.isPressed ? 1 : 0) - (k.aKey.isPressed ? 1 : 0);
            float y = (k.wKey.isPressed ? 1 : 0) - (k.sKey.isPressed ? 1 : 0);
            return new Vector2(x, y);
        }
    }

    // Space: 상승 / LeftCtrl: 하강
    public static float Vertical
    {
        get
        {
            var k = Keyboard.current;
            if (k == null) return 0f;
            return (k.spaceKey.isPressed ? 1 : 0) - (k.leftCtrlKey.isPressed ? 1 : 0);
        }
    }

    // 마우스 좌우 이동량 (회전용)
    public static float Yaw => Mouse.current != null ? Mouse.current.delta.ReadValue().x * 0.1f : 0f;
#else
    public static bool QDown => Input.GetKeyDown(KeyCode.Q);
    public static bool RDown => Input.GetKeyDown(KeyCode.R);
    public static bool TDown => Input.GetKeyDown(KeyCode.T);
    public static bool EDown => Input.GetKeyDown(KeyCode.E);
    public static bool FireDown => Input.GetMouseButtonDown(0);

    public static Vector2 Move => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    public static float Vertical =>
        (Input.GetKey(KeyCode.Space) ? 1 : 0) - (Input.GetKey(KeyCode.LeftControl) ? 1 : 0);

    public static float Yaw => Input.GetAxis("Mouse X");
#endif
}