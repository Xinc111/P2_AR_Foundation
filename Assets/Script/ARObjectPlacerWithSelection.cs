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
    public GameObject defaultDollPrefab; // ✅ 默认娃娃
    public GameObject introductionPanel; // ✅ 介绍界面

    [HideInInspector] public GameObject selectedDoll;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject placedDoll = null;
    private float placeDelay = 0f;

    void Start()
    {
        selectedDoll = defaultDollPrefab;
    }

    void Update()
    {
        if (Touchscreen.current == null || selectedDoll == null) return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
        {
            // 防误触延迟
            if (Time.time < placeDelay)
            {
                Debug.Log("⏳ 延迟中，忽略点击");
                return;
            }

            // 忽略 UI 点击
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

                // 让娃娃朝向摄像头
                Vector3 lookDirection = arCamera.transform.position - placedDoll.transform.position;
                lookDirection.y = 0f;
                placedDoll.transform.rotation = Quaternion.LookRotation(lookDirection);

                Debug.Log("✅ 放置娃娃：" + selectedDoll.name);
            }
        }
    }

    // 🧸 玩家点击选择娃娃按钮时调用
    public void SetDollPrefab(GameObject prefab)
    {
        selectedDoll = prefab;
        placeDelay = Time.time + 0.2f;

        // ✅ 隐藏介绍面板
        if (introductionPanel != null)
            introductionPanel.SetActive(false);

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
