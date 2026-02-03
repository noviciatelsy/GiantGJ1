using UnityEngine;
using System.Collections;

public class EnemyHealthChange : MonoBehaviour
{
    public int EnemyMaxHealth;
    public int EnemyCurHealth;

    [SerializeField] private SpriteRenderer spriteHealth;

    //Health Bar Show / Hide
    private float showDuration = 0.1f;
    private float hideDelay = 1.0f;
     private float hideDuration = 0.08f;
    private float offsetY = 1.0f;
    private SpriteRenderer[] allSprites;

    private Coroutine hideCoroutine;
    private bool isShowing = false;

    private Vector3 originLocalPos;

    // 血条的最大宽度
    private float maxWidth;

    private void Start()
    {
        EnemyCurHealth = EnemyMaxHealth;
        spriteHealth.transform.localScale = new Vector3( 4f,0.83f,0.83f);
        originLocalPos = transform.localPosition;
        transform.localPosition = originLocalPos + Vector3.down * offsetY;

        allSprites = GetComponentsInChildren<SpriteRenderer>();
        SetAllSpritesAlpha(0f);
    }

    public void ChangeHealth()
    {
        ShowHealthBar();
        float targetscale = (float)EnemyCurHealth / (float)EnemyMaxHealth;
        StartCoroutine(AnimateHealthBar(targetscale));
    }


    private float decreaseInterval = 0.4f; // 每 0.2 秒减少 1 点血量
    private bool isDecreasingHealth = false;
    public void StartHealthDecrease()
    {
        if (!isDecreasingHealth)
        {
            StartCoroutine(DecreaseHealth());
            isDecreasingHealth = true;
        }
    }

    // 每 0.2 秒减少 1 点血量
    private IEnumerator DecreaseHealth()
    {
        while (EnemyCurHealth > 0)
        {
            EnemyCurHealth -= 5; // 每 0.2 秒减少 1 点血量
            ChangeHealth(); // 更新血条
            yield return new WaitForSeconds(decreaseInterval); // 每隔 0.2 秒执行一次
        }

        // 血量为 0 时停止减少
        isDecreasingHealth = false;
    }

    private IEnumerator AnimateHealthBar(float targetscale)
    {
        float duration = 0.15f; // 动画持续时间
        float elapsed = 0f;
        float initialScaleX = spriteHealth.transform.localScale.x;
        float targetScaleX = ( (float)EnemyCurHealth / (float)EnemyMaxHealth ) * 4;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newScaleX = Mathf.Lerp(initialScaleX, targetScaleX, elapsed / duration);
            spriteHealth.transform.localScale = new Vector3(newScaleX, spriteHealth.transform.localScale.y, spriteHealth.transform.localScale.z);
            yield return null;
        }
        // 确保最终值正确
        spriteHealth.transform.localScale = new Vector3(targetScaleX, spriteHealth.transform.localScale.y, spriteHealth.transform.localScale.z);
    }

    public void ShowHealthBar()
    {
        if (!isShowing)
        {
            StartCoroutine(ShowRoutine());
        }

        RequestHide();
    }

    private IEnumerator ShowRoutine()
    {
        isShowing = true;

        Vector3 startPos = originLocalPos + Vector3.down * offsetY;
        Vector3 targetPos = originLocalPos;

        StartCoroutine(FadeAllSprites(1f, showDuration));
        float time = 0f;

        while (time < showDuration)
        {
            time += Time.deltaTime;
            float t = time / showDuration;

            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.localPosition = targetPos;
    }

    private void RequestHide()
    {
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }
    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);

        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = originLocalPos + Vector3.down * offsetY;

        StartCoroutine(FadeAllSprites(0f, hideDuration));
        float time = 0f;

        while (time < hideDuration)
        {
            time += Time.deltaTime;
            float t = time / hideDuration;

            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.localPosition = targetPos;

        isShowing = false;
        hideCoroutine = null;
    }

    private void SetAllSpritesAlpha(float alpha)
    {
        for (int i = 0; i < allSprites.Length; i++)
        {
            if (allSprites[i] == null) continue;

            Color c = allSprites[i].color;
            c.a = alpha;
            allSprites[i].color = c;
        }
    }

    private IEnumerator FadeAllSprites(float targetAlpha, float duration)
    {
        float startAlpha = GetCurrentAlpha();
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            SetAllSpritesAlpha(alpha);

            yield return null;
        }

        SetAllSpritesAlpha(targetAlpha);
    }

    private float GetCurrentAlpha()
    {
        if (allSprites == null || allSprites.Length == 0)
            return 1f;

        return allSprites[0].color.a;
    }

}
