using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectPlacerWithSelection : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public Camera arCamera;
    public GameObject defaultDollPrefab; // ✅ 默认可放的娃娃

    [HideInInspector] public GameObject selectedDoll;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject placedDoll = null;
    private float placeDelay = 0f;

    void Start()
    {
        // 🟢 默认一打开就设置娃娃 prefab（用户还没点按钮时也能放置）
        selectedDoll = defaultDollPrefab;
    }

    void Update()
    {
        if (Touchscreen.current == null || selectedDoll == null) return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
        {
            // ⏳ 添加延迟判断（防止刚点按钮就误触）
            if (Time.time < placeDelay)
            {
                Debug.Log("⏳ 延迟中，忽略点击");
                return;
            }

            // 👆 如果点击在 UI 上，忽略放置
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(-1))
            {
                Debug.Log("👉 点击在 UI 上，忽略放置");
                return;
            }

            if (placedDoll != null) return;

            Vector2 touchPosition = touch.position.ReadValue();

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                placedDoll = Instantiate(selectedDoll, hitPose.position, Quaternion.identity);

                // 朝向摄像头
                Vector3 lookDirection = arCamera.transform.position - placedDoll.transform.position;
                lookDirection.y = 0f;
                placedDoll.transform.rotation = Quaternion.LookRotation(lookDirection);

                Debug.Log("✅ 放置娃娃：" + selectedDoll.name);
            }
        }
    }

    // 👉 点击按钮选择其他娃娃时调用
    public void SetDollPrefab(GameObject prefab)
    {
        selectedDoll = prefab;
        placeDelay = Time.time + 0.2f; // 延迟 0.2 秒，避免点击按钮也触发放置
        Debug.Log("🎯 切换娃娃：" + prefab.name);
    }

    public bool HasPlacedDoll()
    {
        return placedDoll != null;
    }

    public GameObject GetPlacedDoll()
    {
        return placedDoll;
    }
}
