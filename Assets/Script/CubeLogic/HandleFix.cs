using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HandleFix : MonoBehaviour
{
    public GameObject HealthBar;
    public List<GameObject> Bars;

    private int Fixedindex = 0;
    private CanvasGroup canvasGroup;

    private Coroutine hideDelayCoroutine;
    private bool isShowing = false;

    private void Awake()
    {
        canvasGroup = GetCanvasGroup();

        HealthBar.SetActive(false);
        Fixedindex = Bars.Count;

        // 初始：全部显示（满耐久）
        for (int i = 0; i < Bars.Count; i++)
        {
            Bars[i].SetActive(true);
        }
    }

    void Start()
    {
        HealthBar.SetActive(false);
        //StartCoroutine(EzTest() );
    }


    public void HealthBarFix()
    {
        StartCoroutine(HealthBarFix1());
    }

    public void HealthBarCrush()
    {
        StartCoroutine(HealthBarCrush1());
    }

    public  IEnumerator EzTest()
    {
        yield return new WaitForSeconds(1f);
        HealthBarCrush();
        yield return new WaitForSeconds(1f);
        HealthBarFix();
    }


    private IEnumerator HealthBarFix1()
    {
        if (Fixedindex >= Bars.Count)
            yield break;
        yield return StartCoroutine(ShowHealthBar());

        Bars[Fixedindex].SetActive(true);
        Fixedindex++;

        //yield return StartCoroutine(HideHealthBar());
        RequestHide();
    }

    private IEnumerator HealthBarCrush1()
    {
        if (Fixedindex <= 0)
            yield break;
        yield return StartCoroutine(ShowHealthBar());

        Fixedindex--;
        Bars[Fixedindex].SetActive(false);

        //yield return StartCoroutine(HideHealthBar());
        RequestHide();
    }
    // ===================== 动画 =====================

    private IEnumerator ShowHealthBar()
    {
        if (isShowing)
            yield break;

        isShowing = true;

        HealthBar.SetActive(true);
        canvasGroup.alpha = 0f;

        Vector3 startPos = HealthBar.transform.localPosition;
        HealthBar.transform.localPosition =
            new Vector3(startPos.x, 0.5f, startPos.z);

        Vector3 targetPos = new Vector3(0f, 1.5f, 0f);

        yield return StartCoroutine(
            MoveAndFade(targetPos, 0.25f, 1f)
        );
    }

    private void RequestHide()
    {
        if (hideDelayCoroutine != null)
            StopCoroutine(hideDelayCoroutine);

        hideDelayCoroutine = StartCoroutine(HideAfterDelay(2f));
    }
    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 targetPos = new Vector3(0f, 0.5f, 0f);
        yield return StartCoroutine(
            MoveAndFade(targetPos, 0.15f, 0f)
        );

        HealthBar.SetActive(false);
        isShowing = false;
        hideDelayCoroutine = null;
    }


    private IEnumerator MoveAndFade(
        Vector3 targetPos,
        float duration,
        float targetAlpha
    )
    {
        Vector3 startPos = HealthBar.transform.localPosition;
        float startAlpha = canvasGroup.alpha;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            HealthBar.transform.localPosition =
                Vector3.Lerp(startPos, targetPos, t);

            canvasGroup.alpha =
                Mathf.Lerp(startAlpha, targetAlpha, t);

            yield return null;
        }

        HealthBar.transform.localPosition = targetPos;
        canvasGroup.alpha = targetAlpha;
    }

    // ===================== 工具 =====================

    private CanvasGroup GetCanvasGroup()
    {
        CanvasGroup cg = HealthBar.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = HealthBar.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
        return cg;
    }
}
