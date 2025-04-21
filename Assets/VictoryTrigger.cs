using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ✅ 检测是否摄像头进入
        if (other.gameObject.CompareTag("MainCamera") && gameManager != null)
        {
            Debug.Log("🏁 Player entered Victory Zone!");
            gameManager.TriggerVictory();
        }
    }
}
