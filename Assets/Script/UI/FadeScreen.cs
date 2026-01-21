using System.Collections;
using UnityEngine;


public class FadeScreen : MonoBehaviour
{
    [Header("References")]
    private CanvasGroup canvasGroup;

    [Header("Fade Settings")]
    [SerializeField] private float fadeHoldTime = 0.5f;     // 黑屏停留时间
    [SerializeField] private float fadeOutDuration = 0.25f; // 变黑时间
    [SerializeField] private float fadeInDuration = 0.25f;  // 变亮时间

    public static FadeScreen Instance;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        canvasGroup = GetComponent<CanvasGroup>();

        // 默认不挡视野
        SetAlpha(0f);
    }


    public void PlayFade(System.Action onBlackReached)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeCo(onBlackReached));
    }



    private IEnumerator FadeCo(System.Action onBlackReached)
    {
        yield return FadeRoutine(1,fadeOutDuration); // 屏幕变黑
        onBlackReached?.Invoke();
        yield return WaitUnscaledSeconds(fadeHoldTime);
        yield return FadeRoutine(0, fadeInDuration); // 屏幕变亮
        fadeCoroutine = null;
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;
        float safeDuration = Mathf.Max(0.0001f, duration);

        if (Mathf.Approximately(startAlpha, targetAlpha))
        {
            SetAlpha(targetAlpha);
            fadeCoroutine = null;
            yield break;
        }

        while (time < safeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / safeDuration);

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }

        SetAlpha(targetAlpha);
    }



    private IEnumerator WaitUnscaledSeconds(float seconds)
    {
        float time = 0f;
        float safeSeconds = Mathf.Max(0f, seconds);

        while (time < safeSeconds)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private void SetAlpha(float a)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = a;
        }
    }
}
