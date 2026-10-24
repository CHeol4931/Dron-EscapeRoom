using System;
using UnityEngine;

public enum DroneMode { None, Menu, Ground, Flying }

/// <summary>
/// Q: 컨트롤러 열기/닫기  |  (열린 상태에서) R: 지상 드론, T: 비행 드론
/// 조종 대상이 바뀔 때 플레이어 이동/카메라/커서를 함께 전환한다.
/// </summary>
public class DroneControlManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private MonoBehaviour playerController; // 플레이어 이동 스크립트 (조종 중엔 비활성화)
    [SerializeField] private Camera playerCamera;

    [Header("Drones")]
    [SerializeField] private GroundDrone groundDrone;
    [SerializeField] private FlyingDrone flyingDrone;

    [Header("UI")]
    [SerializeField] private GameObject controllerPanel;      // Q 누르면 나오는 컨트롤러 UI

    public DroneMode Mode { get; private set; } = DroneMode.None;

    /// <summary>UI 쪽에서 구독해서 안내 문구/아이콘 등을 갱신</summary>
    public event Action<DroneMode> OnModeChanged;

    private CursorLockMode prevLock;
    private bool prevCursorVisible;

    private void Start()
    {
        if (controllerPanel != null) controllerPanel.SetActive(false);
        ApplyMode(DroneMode.None);
    }

    private void Update()
    {
        if (DroneInput.QDown)
        {
            SetMode(Mode == DroneMode.None ? DroneMode.Menu : DroneMode.None);
            return;
        }

        if (Mode == DroneMode.None) return; // 컨트롤러가 닫혀 있으면 R/T 무시

        if (DroneInput.RDown) SetMode(Mode == DroneMode.Ground ? DroneMode.Menu : DroneMode.Ground);
        else if (DroneInput.TDown) SetMode(Mode == DroneMode.Flying ? DroneMode.Menu : DroneMode.Flying);
    }

    public void SetMode(DroneMode next)
    {
        if (next == Mode) return;

        // 처음 컨트롤러를 열 때만 커서 상태 저장
        if (Mode == DroneMode.None)
        {
            prevLock = Cursor.lockState;
            prevCursorVisible = Cursor.visible;
        }

        Mode = next;
        ApplyMode(next);

        if (next == DroneMode.None)
        {
            Cursor.lockState = prevLock;
            Cursor.visible = prevCursorVisible;
        }

        OnModeChanged?.Invoke(next);
    }

    private void ApplyMode(DroneMode mode)
    {
        bool droneActive = mode == DroneMode.Ground || mode == DroneMode.Flying;

        if (controllerPanel != null) controllerPanel.SetActive(mode != DroneMode.None);

        // 플레이어: 드론 조종/메뉴 중에는 이동 정지
        if (playerController != null) playerController.enabled = (mode == DroneMode.None);

        // 드론 조종 대상 전환
        if (groundDrone != null) groundDrone.SetControlled(mode == DroneMode.Ground);
        if (flyingDrone != null) flyingDrone.SetControlled(mode == DroneMode.Flying);

        // 카메라 전환 (드론 카메라가 켜지면 플레이어 카메라는 끔)
        SetCamera(playerCamera, !droneActive);

        // 커서: 드론 조종 중엔 잠금, 메뉴에서는 보이게
        if (mode != DroneMode.None)
        {
            Cursor.lockState = droneActive ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !droneActive;
        }
    }

    // 카메라와 AudioListener를 같이 켜고 꺼서 "AudioListener 2개" 경고를 방지
    private static void SetCamera(Camera cam, bool on)
    {
        if (cam == null) return;
        cam.enabled = on;
        var listener = cam.GetComponent<AudioListener>();
        if (listener != null) listener.enabled = on;
    }
}
