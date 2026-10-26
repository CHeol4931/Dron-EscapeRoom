using UnityEngine;
using System;
using TMPro;

public class EscapeManager : MonoBehaviour
{
    public static EscapeManager Instance {  get; private set; }
    [SerializeField] private int requiredCount = 7;
    [SerializeField] private TMP_Text messageText;

    public int DoneCount {  get; private set; }
    public bool AllDone => DoneCount >= requiredCount;
    public bool Escaped { get; private set; }
    public event Action<int, int> OnProgress;
    public event Action OnEscaped;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        int found = FindObjectsByType<Interactable>(FindObjectsSortMode.None).Length;
        if (found != requiredCount)
            Debug.LogWarning($"EscapeManager:Interactable이 {found}개 있지만 필요한 개수는 {requiredCount}개 입니다");
        ShowMessage($"Interactions: {DoneCount}/{requiredCount}");
    }
    public void NotifyInteracted(Interactable it)
    {
        DoneCount++;
        OnProgress?.Invoke(DoneCount, requiredCount);

        ShowMessage(AllDone
            ? "All done! Reach the exit"
            : $"Interactions:{DoneCount}/{requiredCount}");
    }
    public void TryEscape()
    {
        if (Escaped) return;
        if (!AllDone)
        {
            ShowMessage($"Locked. ({DoneCount}/{requiredCount})");
            return;
        }
        Escaped = true;
        ShowMessage("ESCAPED!");
        OnEscaped?.Invoke();
    }
    private void ShowMessage(string msg)
    {
        if (messageText != null) messageText.text = msg;
    }
    void Update()
    {
        
    }
}
