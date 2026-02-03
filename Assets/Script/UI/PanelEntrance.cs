using UnityEngine;

public class PanelEntrance : MonoBehaviour
{
    [Header("References")]
    private RectTransform panelRect;
    private CanvasGroup canvasGroup;

    [Header("Positions (Anchored)")]
    [SerializeField] private Vector2 shownAnchoredPosition = Vector2.zero;

    [Header("Adaptive Hidden Position")]
    [Tooltip("用哪个 Rect 作为“屏幕可视区域”的参考（Canvas 或面板的父物体RectTransform）")]
    [SerializeField] private RectTransform referenceRect;

    public enum HideDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    [Tooltip("面板退场方向（决定用宽度还是高度）。例如向右退场会用可视区域宽度来确保完全隐藏。")]
    [SerializeField] private HideDirection hideDirection = HideDirection.Up;

    [Tooltip("额外隐藏余量（防止刚好卡边露一条缝）")]
    [Min(0f)]
    [SerializeField] private float extraHidePadding = 8f;

    // 运行时动态计算得到的隐藏位置（Anchored）
    private Vector2 hiddenAnchoredPosition;

    [Header("Move In")]
    [Min(0.01f)]
    [SerializeField] private float moveInDuration = 0.18f;

    [SerializeField] private AnimationCurve moveInCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Move Out")]
    [Min(0.01f)]
    [SerializeField] private float moveOutDuration = 0.14f;

    [Tooltip("退场曲线（默认快走快消失）")]
    [SerializeField] private AnimationCurve moveOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Settle Shake (Inertia)")]
    [Min(0f)]
    [SerializeField] private float settleDuration = 0.22f;

    [SerializeField] private float settleAmplitude = 18f;
    [SerializeField] private float settleFrequency = 26f;
    [SerializeField] private float settleDamping = 12f;

    [Header("Interaction")]
    [SerializeField] private bool disableInteractionUntilSettled = true;

    private Coroutine playCo;

    // 是否已经播放过退场动画
    private bool isHidingByScript;


    private void Awake()
    {
        panelRect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (referenceRect == null)
        {
            // 默认用父物体作为“可视区域参考”
            referenceRect = panelRect.parent as RectTransform;
        }

        RefreshHiddenAnchoredPosition();
    }

    private void OnEnable()
    {
        isHidingByScript = false;

        // 启用时刷新一次（防止分辨率/布局变化）
        RefreshHiddenAnchoredPosition();

        // 启用时先放到隐藏位置（避免上一帧残留）
        panelRect.anchoredPosition = hiddenAnchoredPosition;

        StartEntrance();
    }

    private void OnDisable()
    {
        if (!isHidingByScript)
        {
            if (panelRect != null)
            {
                // 禁用时也保证在隐藏位置
                panelRect.anchoredPosition = hiddenAnchoredPosition;
            }
        }

        SetInteractable(true);

        if (playCo != null)
        {
            StopCoroutine(playCo);
            playCo = null;
        }
    }

    // 当屏幕尺寸/父级Rect变化时可能会触发（比如窗口缩放、分辨率变化）
    private void OnRectTransformDimensionsChange()
    {
        if (isActiveAndEnabled)
        {
            RefreshHiddenAnchoredPosition();
        }
    }

    public void StartEntrance()
    {
        if (playCo != null)
        {
            StopCoroutine(playCo);
        }

        playCo = StartCoroutine(EntranceCo());
    }


    // 外界想关闭面板时，调用这个，而不是直接 SetActive(false)
    public void HideWithMoveOut()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (playCo != null)
        {
            StopCoroutine(playCo);
        }

        playCo = StartCoroutine(MoveOutAndDisableCo());
    }

    private System.Collections.IEnumerator EntranceCo()
    {
        if (disableInteractionUntilSettled)
        {
            SetInteractable(false);
        }

        // 1Move In：隐藏 -> 出现
        float t = 0f;
        while (t < moveInDuration)
        {
            t += Time.unscaledDeltaTime;

            float p = Mathf.Clamp01(t / moveInDuration);
            float eased = moveInCurve.Evaluate(p);

            panelRect.anchoredPosition = Vector2.LerpUnclamped(hiddenAnchoredPosition, shownAnchoredPosition, eased);
            yield return null;
        }

        panelRect.anchoredPosition = shownAnchoredPosition;

        // 2Settle：上下抖几下（阻尼正弦）
        if (settleDuration > 0.0001f && settleAmplitude > 0.0001f && settleFrequency > 0.0001f)
        {
            float st = 0f;
            while (st < settleDuration)
            {
                st += Time.unscaledDeltaTime;

                float damper = Mathf.Exp(-settleDamping * st);
                float offsetY = Mathf.Sin(st * settleFrequency) * settleAmplitude * damper;

                panelRect.anchoredPosition = shownAnchoredPosition + new Vector2(0f, offsetY);
                yield return null;
            }
        }

        panelRect.anchoredPosition = shownAnchoredPosition;

        if (disableInteractionUntilSettled)
        {
            SetInteractable(true);
        }

        playCo = null;
    }

    private System.Collections.IEnumerator MoveOutAndDisableCo()
    {
        // 退场过程禁用交互
        SetInteractable(false);

        // 退场前也刷新一次，确保是最新的隐藏位置
        RefreshHiddenAnchoredPosition();

        Vector2 from = panelRect.anchoredPosition;
        Vector2 to = hiddenAnchoredPosition;

        float t = 0f;
        while (t < moveOutDuration)
        {
            t += Time.unscaledDeltaTime;

            float p = Mathf.Clamp01(t / moveOutDuration);
            float eased = moveOutCurve.Evaluate(p);

            panelRect.anchoredPosition = Vector2.LerpUnclamped(from, to, eased);
            yield return null;
        }

        panelRect.anchoredPosition = to;

        isHidingByScript = true;

        // 彻底关闭
        gameObject.SetActive(false);

        playCo = null;
    }

    private void RefreshHiddenAnchoredPosition()
    {
        if (panelRect == null || referenceRect == null)
        {
            // 兜底：如果缺引用，就至少保证能移出去（这里用屏幕高度做个保守值）
            hiddenAnchoredPosition = shownAnchoredPosition + new Vector2(0f, Screen.height);
            return;
        }

        // 让布局先结算一次（如果你面板上有 ContentSizeFitter / Layout）
        Canvas.ForceUpdateCanvases();

        // 为了算“刚好移出可视区域”，我们临时把面板放到显示位置计算边界
        Vector2 cached = panelRect.anchoredPosition;
        panelRect.anchoredPosition = shownAnchoredPosition;

        // 计算面板在 referenceRect 局部空间里的包围盒
        Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(referenceRect, panelRect);
        Rect r = referenceRect.rect;

        // 还原位置（避免影响当前动画）
        panelRect.anchoredPosition = cached;

        Vector2 delta = Vector2.zero;

        switch (hideDirection)
        {
            case HideDirection.Up:
                {
                    // 让面板的底边 > 可视区域顶边
                    delta.y = (r.yMax - bounds.min.y) + extraHidePadding;
                    break;
                }
            case HideDirection.Down:
                {
                    // 让面板的顶边 < 可视区域底边
                    delta.y = (r.yMin - bounds.max.y) - extraHidePadding;
                    break;
                }
            case HideDirection.Left:
                {
                    // 让面板的右边 < 可视区域左边
                    delta.x = (r.xMin - bounds.max.x) - extraHidePadding;
                    break;
                }
            case HideDirection.Right:
                {
                    // 让面板的左边 > 可视区域右边
                    delta.x = (r.xMax - bounds.min.x) + extraHidePadding;
                    break;
                }
        }

        hiddenAnchoredPosition = shownAnchoredPosition + delta;
    }

    private void SetInteractable(bool value)
    {
        if (canvasGroup == null)
        {
            return;
        }

        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
    }
}
