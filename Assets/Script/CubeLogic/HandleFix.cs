using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandleFix : MonoBehaviour
{
    public GameObject HealthBar;
    public List<GameObject> Bars;

    private int currentBarCount = 0;
    private CanvasGroup canvasGroup;

    private Coroutine hideDelayCoroutine;
    private bool isShowing = false;
    private float hideDelay = 2;
    private float quickHideDelay = 0.1f;

    // 修理
    private float showFixBarDelay = 0.5f;
    private float timeToFix = 2f;
    private float fixingTimer;
    private bool isfixing = false;
    public PLControl currentFixingPlayer {  get; private set; } // 正在修理该浮块的玩家 

    private void Awake()
    {
        canvasGroup = HealthBar.GetComponent<CanvasGroup>();
        fixingTimer = 0;
        HealthBar.SetActive(false);
        currentBarCount = Bars.Count;

        // 初始：全部显示（满耐久）
        for (int i = 0; i < Bars.Count; i++)
        {
            Bars[i].transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Update()
    {
        if (currentBarCount >= Bars.Count)
        {
            return; // 如果此刻耐久为满
        }
        else
        {
            if (isfixing) // 如果该浮块正在被玩家按住修理
            {
                fixingTimer += Time.deltaTime; // 累计修理时间
                if (!isShowing) // 如果此时未显示进度条
                {
                    if (fixingTimer >= showFixBarDelay) // 如果达到足够显示进度条的时间
                    {

                        StartCoroutine(ShowHealthBar());
                    }
                }
                UpdateCurrentBarFixedPercent(); // 更新当前修理条的进度
                if (fixingTimer >= timeToFix)
                {
                    currentBarCount += 1; // 耐久度+1
                    fixingTimer = 0;
                    if (currentBarCount >= Bars.Count) // 如果此时刚好修满
                    {
                        EndFix(); // 强制结束修理过程
                    }
                }
            }
        }

    }

    public void HealthBarCrush()
    {
        if(currentBarCount==0)
        {
            return ;
        }
        StartCoroutine(HealthBarCrush1());
    }

    //public void HealthBarFix()
    //{
    //    StartCoroutine(HealthBarFix1());
    //}



    //public  IEnumerator EzTest()
    //{
    //    yield return new WaitForSeconds(1f);
    //    HealthBarCrush();
    //    yield return new WaitForSeconds(1f);
    //    HealthBarFix();
    //}


    //private IEnumerator HealthBarFix1()
    //{
    //    if (Fixedindex >= Bars.Count)
    //        yield break;
    //    yield return StartCoroutine(ShowHealthBar());

    //    Bars[Fixedindex].SetActive(true);
    //    Fixedindex++;

    //    //yield return StartCoroutine(HideHealthBar());
    //    RequestHide();
    //}

    private IEnumerator HealthBarCrush1()
    {
        if (currentBarCount <= 0)
            yield break;
        yield return StartCoroutine(ShowHealthBar());

        DoDamageToBar(); // 修理条-1
        currentBarCount--;

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

        hideDelayCoroutine = StartCoroutine(HideAfterDelay(hideDelay));
    }

    private void RequestQuickHide()
    {
        if (hideDelayCoroutine != null)
            StopCoroutine(hideDelayCoroutine);

        hideDelayCoroutine = StartCoroutine(HideAfterDelay(quickHideDelay));
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

    private void UpdateCurrentBarFixedPercent()
    {
        float fixedPercent = fixingTimer / timeToFix; // 计算进度
        fixedPercent = Mathf.Clamp01(fixedPercent);
        Bars[currentBarCount].transform.localScale = new Vector3(fixedPercent, 1, 1);
    }

    private void ResetCurrentBar()
    {
        Bars[currentBarCount].transform.localScale = new Vector3(0, 1, 1); 
    }

    private void DoDamageToBar()
    {
        Bars[currentBarCount-1].transform.localScale = new Vector3(0, 1, 1);
    }

    public void StartFix(PLControl currentFixingPlayer)
    {
        this.currentFixingPlayer=currentFixingPlayer;
        isfixing = true;
        fixingTimer = 0f;
    }

    public void EndFix()
    {
        if (currentBarCount < Bars.Count&&fixingTimer<timeToFix) // 如果结束修理时有修到一半的
        {
            ResetCurrentBar(); // 重置正在修理的修理进度
        }
        RequestQuickHide();
        currentFixingPlayer = null;
        isfixing = false;
        fixingTimer = 0f;
    }

}
