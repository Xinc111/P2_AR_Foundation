using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public ARObjectPlacerWithSelection placerScript;
    public MotionDetector motionDetector;
    public TextMeshProUGUI countdownText;
    public GameObject gameOverCanvas;

    public ARPlaneManager planeManager;           // ✅ 新增：关闭平面检测
    public ARRaycastManager raycastManager;       // ✅ 新增：关闭点击检测

    public float redLightDuration = 3f;
    public float greenLightDuration = 3f;

    private GameObject placedDoll = null;
    private Animator dollAnimator = null;
    private DollController dollController = null;
    private bool gameStarted = false;
    private bool gameEnded = false;

    private TextMeshProUGUI gameOverText;

    void Start()
    {
        if (countdownText != null)
            countdownText.text = "";

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
            gameOverText = gameOverCanvas.GetComponentInChildren<TextMeshProUGUI>();
        }
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

        DisablePlaneDetection(); // ✅ 倒计时后关闭平面检测

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
        Debug.Log("🚨 Red Light!");
        countdownText.text = "Red Light";

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
            yield break;
        }

        countdownText.text = "";
    }

    IEnumerator GreenLight()
    {
        Debug.Log("✅ Green Light!");
        countdownText.text = "Green Light";

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
        StopAllCoroutines();
        gameEnded = true;
    }

    void ShowGameOver(string message)
    {
        countdownText.text = message;

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            if (gameOverText != null)
                gameOverText.text = message;
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
        {
            raycastManager.enabled = false;
        }

        Debug.Log("🛑 Plane detection disabled after GO!");
    }
}
