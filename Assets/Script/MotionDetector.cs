using UnityEngine;

public class MotionDetector : MonoBehaviour
{
    private Vector3 lastPosition;
    private bool detecting = false;
    private bool moved = false;
    public float movementThreshold = 0.02f;

    void Update()
    {
        if (!detecting) return;

        Vector3 currentPos = Camera.main.transform.position;
        float distance = Vector3.Distance(currentPos, lastPosition);

        if (distance > movementThreshold)
        {
            moved = true;
            Debug.Log("🚶 摄像头移动了：" + distance);
        }

        lastPosition = currentPos;
    }

    public void StartDetection()
    {
        moved = false;
        detecting = true;
        lastPosition = Camera.main.transform.position;
    }

    public void StopDetection()
    {
        detecting = false;
    }

    public bool HasMoved()
    {
        return moved;
    }
}
