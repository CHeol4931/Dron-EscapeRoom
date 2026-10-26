using UnityEngine;
using UnityEngine.Events;


public class Interactable : MonoBehaviour
{
    [SerializeField] private string displayName = "Switch";
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color doneColor = Color.green;
    [SerializeField] private UnityEvent onInteracted;

    public string DisplayName => displayName;
    public bool Done { get; private set;  }
    public void Interact()
    {
        if (Done) return;
        Done = true;
        if (targetRenderer != null) targetRenderer.material.color = doneColor;
        onInteracted.Invoke();
        if(EscapeManager.Instance != null) EscapeManager.Instance.NotifyInteracted(this);
    }
}
