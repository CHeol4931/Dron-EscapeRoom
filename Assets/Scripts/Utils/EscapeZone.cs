using UnityEngine;

/// <summary>
/// 출구 Collider(Is Trigger 체크)에 붙인다.
/// 플레이어가 닿았을 때 7개가 모두 완료됐으면 탈출.
/// </summary>
[RequireComponent(typeof(Collider))]
public class EscapeZone : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() == null) return;
        if (EscapeManager.Instance != null) EscapeManager.Instance.TryEscape();
    }
}