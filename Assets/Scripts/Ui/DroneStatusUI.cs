using TMPro;
using UnityEngine;

/// <summary>
/// 테스트용 상태 표시. DroneControlManager.OnModeChanged를 구독해서
/// 컨트롤러 ON/OFF와 현재 조종 중인 드론을 텍스트로 보여준다.
/// </summary>
public class DroneStatusUI : MonoBehaviour
{
    [SerializeField] private DroneControlManager manager;
    [SerializeField] private TMP_Text statusText;

    private void OnEnable()
    {
        if (manager != null) manager.OnModeChanged += Refresh;
    }

    private void OnDisable()
    {
        if (manager != null) manager.OnModeChanged -= Refresh;
    }

    private void Start()
    {
        Refresh(manager != null ? manager.Mode : DroneMode.None);
    }

    private void Refresh(DroneMode mode)
    {
        if (statusText == null) return;

        switch (mode)
        {
            case DroneMode.None:
                statusText.text = "Controller: OFF  (Q)";
                break;
            case DroneMode.Menu:
                statusText.text = "Controller: ON  |  R: Ground  T: Flying  Q: Close";
                break;
            case DroneMode.Ground:
                statusText.text = "Controller: ON  |  GROUND drone  (R: back, T: Flying)";
                break;
            case DroneMode.Flying:
                statusText.text = "Controller: ON  |  FLYING drone  (T: back, R: Ground)";
                break;
        }
    }
}
