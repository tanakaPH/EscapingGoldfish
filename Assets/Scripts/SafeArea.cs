using UnityEngine;
using System.Collections;

/// <summary>
/// SafeAreaの反映
/// </summary>
[ExecuteInEditMode]
public class SafeArea : MonoBehaviour
{
    private RectTransform rectTransform;

    /// <summary>
    /// iPhoneXのサイズ
    /// </summary>
    private const float safeAreaEnableAspect = (float)1125 / 2436;

    /// <summary>
    /// The top area rate.
    /// </summary>
    private const float topAreaRate = 0.04f;

    /// <summary>
    /// The bottom area rate.
    /// </summary>
    private const float bottomAreaRate = 0.06f;

    /// <summary>
    /// エディタ上でリアルタイム反映させたい場合
    /// </summary>
    [SerializeField]
    private bool isEnableEditor = true;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        Debug.Assert(rectTransform != null, "Not Attach");
    }

    // Update is called once per frame
    void Update()
    {
        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        var area = GetSafeArea();

        var anchorMin = area.position;
        var anchorMax = area.position + area.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }

    private Rect GetSafeArea()
    {
        var rect = Screen.safeArea;
#if UNITY_EDITOR
        if (isEnableEditor)
        {
            var screenAspect = (float)Screen.width / Screen.height;
            // 表示サイズによって若干の誤差が生まれるので少し修正
            screenAspect *= 0.95f;
            if (screenAspect <= safeAreaEnableAspect)
            {
                // デバッグ用に設定
                var top = rect.height * topAreaRate;
                var bottom = rect.height * bottomAreaRate;
                rect.y = bottom;
                rect.height = rect.height - top - bottom;
            }
        }
#endif
        return rect;
    }
}