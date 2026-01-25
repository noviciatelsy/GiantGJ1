using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class SlotSelectedImage : MonoBehaviour
{
    private Image chosenImage;
    private RectTransform rect;
    private Vector2 originalSize;
    [SerializeField] private Vector2 chosenSize = new Vector2(135,135);
    [SerializeField] private float timeToShow = 0.15f;

    private bool isChosen;

    private Coroutine showRoutine;
    private void Awake()
    {
        chosenImage = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        chosenImage.enabled = false;
        isChosen = false;
        originalSize = rect.sizeDelta;
    }

    public void SelectSlot()
    {
        if (isChosen)
        {
            return;
        }
        else
        {
            isChosen = true;
            chosenImage.enabled = true;
            if (showRoutine != null)
            {
                StopCoroutine(showRoutine);
            }
            showRoutine = StartCoroutine(ShowChosenImageCoroutine());
        }
    }

    public void EndSelectSlot()
    {
        if (!isChosen)
        {
            return;
        }
        else
        {
            isChosen = false;
            chosenImage.enabled = false;
            ResetChosenImage();
        }
    }

    private IEnumerator ShowChosenImageCoroutine()
    {
        float timer = 0f;
        while (timer < timeToShow)
        {
            timer += Time.deltaTime;
            float t = timer / timeToShow;
            rect.sizeDelta=Vector2.Lerp(originalSize, chosenSize, t);
            yield return null;
        }
        rect.sizeDelta = chosenSize;
    }

    private void ResetChosenImage()
    {
        rect.sizeDelta = originalSize;
    }
}
