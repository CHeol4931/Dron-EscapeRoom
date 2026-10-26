using UnityEngine;

public class Playerinteractor : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private DroneControlManager droneManager;
    [SerializeField] private float interactRange = 3f;
    void Update()
    {
        if (droneManager != null && droneManager.Mode != DroneMode.None) return;
        if (!DroneInput.EDown || playerCamera == null) return;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, interactRange, -0, QueryTriggerInteraction.Ignore))
        {
            var target = hit.collider.GetComponentInParent<Interactable>();
            if (target != null) target.Interact();
        }
    }
}
