using UnityEngine;
using TMPro;

public class UITimer : MonoBehaviour
{
    public float timeLimit = 30f;                     // ⏱️ 倒计时时长
    public TextMeshProUGUI timerText;                 // 🧾 UI 显示文字
    public GameManager gameManager;                   // 💥 通知 GameManager
    private float timeRemaining;
    private bool isRunning = false;

    public void StartTimer()
    {
        timeRemaining = timeLimit;
        isRunning = true;
        if (timerText != null)
            timerText.gameObject.SetActive(true);
    }

    public void StopTimer()
    {
        isRunning = false;
        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isRunning || gameManager == null || gameManager.IsGameEnded()) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isRunning = false;
            gameManager.TriggerTimeoutFailure();
        }

        if (timerText != null)
            timerText.text = $"Time Left: {timeRemaining:F1}s";
    }
}
