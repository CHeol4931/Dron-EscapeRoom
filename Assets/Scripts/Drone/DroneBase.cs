using UnityEngine;

/// <summary>
/// 모든 드론의 공통 부모.
/// 조종 중일 때만 입력을 읽고, 아닐 때는 정지 상태를 유지한다.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class DroneBase : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] protected Camera droneCamera;

    protected Rigidbody rb;
    public bool IsControlled { get; private set; }
    public Camera DroneCamera => droneCamera;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (droneCamera != null) droneCamera.enabled = false;
    }

    /// <summary>매니저가 조종 시작/종료를 알려줄 때 호출</summary>
    public virtual void SetControlled(bool controlled)
    {
        IsControlled = controlled;
        if (droneCamera != null) droneCamera.enabled = controlled;
        if (!controlled) OnControlReleased();
    }

    /// <summary>조종이 끝났을 때 (정지/호버링 등) 처리</summary>
    protected abstract void OnControlReleased();
}
