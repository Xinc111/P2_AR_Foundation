using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public ARObjectPlacerWithSelection placerScript;
    public MotionDetector motionDetector;
    public TextMeshProUGUI countdownText;
    public GameObject victoryPanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI victoryMessageText;
    public TextMeshProUGUI gameOverMessageText;

    public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager;
    public UITimer timerUI;
    public AudioSource gameOverSFX;
    public AudioSource audioSource;



    public float redLightDuration = 3f;
    public float greenLightDuration = 3f;

    private GameObject placedDoll = null;
    private Animator dollAnimator = null;
    
    private DollController dollController = null;
    private bool gameStarted = false;
    private bool gameEnded = false;

    void Start()
    {
        if (countdownText != null)
            countdownText.text = "";

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (!gameStarted && placerScript.HasPlacedDoll())
        {
            placedDoll = placerScript.GetPlacedDoll();
            dollAnimator = placedDoll.GetComponent<Animator>();
            dollController = placedDoll.GetComponent<DollController>();
            StartCoroutine(GameLoop());
            gameStarted = true;
        }
    }

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartCountdown());

        DisablePlaneDetection();

        if (audioSource != null)
            audioSource.Play(); // ✅ 开始游戏音乐

        if (timerUI != null)
            timerUI.StartTimer();

        while (!gameEnded)
        {
            yield return StartCoroutine(RedLight());
            if (gameEnded) yield break;

            yield return StartCoroutine(GreenLight());
        }
    }


    IEnumerator StartCountdown()
    {
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";
    }

    IEnumerator RedLight()
    {
        Debug.Log("Red Light!");
        countdownText.text = "Red Light";
        countdownText.color = Color.red;

        if (dollAnimator != null)
            dollAnimator.SetBool("IsLookingAtPlayer", true);

        if (dollController != null)
            dollController.LookAtPlayer();

        motionDetector.StartDetection();
        yield return new WaitForSeconds(redLightDuration);
        motionDetector.StopDetection();

        if (motionDetector.HasMoved())
        {
            ShowGameOver("You Moved! Game Over");
            gameEnded = true;
            StopAllCoroutines();
            if (timerUI != null) timerUI.StopTimer();
            yield break;
        }

        countdownText.text = "";
    }

    IEnumerator GreenLight()
    {
        countdownText.text = "Green Light";
        countdownText.color = Color.green;

        if (dollAnimator != null)
            dollAnimator.SetBool("IsLookingAtPlayer", false);

        if (dollController != null)
            dollController.LookAwayFromPlayer();

        yield return new WaitForSeconds(greenLightDuration);
        countdownText.text = "";
    }

    public void TriggerVictory()
    {
        if (gameEnded) return;

        ShowGameOver("You Win! You reached the doll!");
        gameEnded = true;
        StopAllCoroutines();

        if (timerUI != null)
            timerUI.StopTimer();
        if (audioSource != null)
            audioSource.Stop(); // ✅ 游戏结束时关闭音乐

    }

    public void TriggerTimeoutFailure()
    {
        if (gameEnded) return;

        ShowGameOver("Time's up! You failed.");
        gameEnded = true;
        StopAllCoroutines();

        if (audioSource != null)
            audioSource.Stop(); // ✅ 游戏结束时关闭音乐

    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }

    void ShowGameOver(string message)
    {
        countdownText.text = "";

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (message.Contains("Win"))
        {
            if (victoryPanel != null) victoryPanel.SetActive(true);
            if (victoryMessageText != null) victoryMessageText.text = message;
        }
        else
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                if (audioSource != null)
                    audioSource.Stop(); // ✅ 一旦出现失败面板，就停止背景音乐
            }

            if (gameOverMessageText != null)
                gameOverMessageText.text = message;

            if (gameOverSFX != null)
                gameOverSFX.Play(); // ✅ 播放 GameOver 音效
        }

        Debug.Log("🎯 Game Over: " + message);
    }

    void DisablePlaneDetection()
    {
        if (planeManager != null)
        {
            planeManager.enabled = false;
            foreach (var plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
        }

        if (raycastManager != null)
            raycastManager.enabled = false;

    }
}
