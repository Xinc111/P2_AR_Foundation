using UnityEngine;

public class DollController : MonoBehaviour
{
    public Camera arCamera;

    /// <summary>
    /// 红灯：转向玩家
    /// </summary>
    public void LookAtPlayer()
    {
        if (arCamera == null)
            arCamera = Camera.main;

        Vector3 lookDirection = arCamera.transform.position - transform.position;
        lookDirection.y = 0f; // 保持头不上下动
        transform.rotation = Quaternion.LookRotation(lookDirection);

        Debug.Log("🧸 Doll is looking at player (Red Light)");
    }

    /// <summary>
    /// 绿灯：背对玩家
    /// </summary>
    public void LookAwayFromPlayer()
    {
        if (arCamera == null)
            arCamera = Camera.main;

        Vector3 awayDirection = transform.position - arCamera.transform.position;
        awayDirection.y = 0f;
        transform.rotation = Quaternion.LookRotation(awayDirection);

        Debug.Log("🧸 Doll turned back (Green Light)");
    }
}
