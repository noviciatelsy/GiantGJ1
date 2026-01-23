using System.Collections;
using UnityEngine;

public class ChosenImage : MonoBehaviour
{
    private SpriteRenderer chosenImage;
    private CubeBase cube;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    [SerializeField] private Vector3 chosenPositon=new Vector3(0,0.55f,0);
    [SerializeField] private Vector3 chosenScale=new Vector3(0.65f,0.65f,0.65f);
    [SerializeField] private float timeToShow = 0.25f;

    private bool isChosen;

    private Coroutine showRoutine;
    private void Awake()
    {
        chosenImage = GetComponent<SpriteRenderer>();
        cube=GetComponentInParent<CubeBase>();
        chosenImage.enabled = false;
        isChosen=false;
        originalPosition=transform.localPosition;
        originalScale=transform.localScale;
    }

    public void ChooseCube()
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

    public void EndChooseCube()
    {
        if(!isChosen)
        {
            return;
        }
        else
        {
            if(cube.landNumber!=0) // 如果该浮块上有其他玩家
            {
                return ;
            }
            isChosen = false;
            chosenImage.enabled=false;
            ResetChosenImage();
        }
    }

    private IEnumerator ShowChosenImageCoroutine()
    {
        float timer = 0f;
        while(timer < timeToShow)
        {
            timer += Time.deltaTime;
            float t = timer / timeToShow;
            chosenImage.transform.localScale = Vector3.Lerp(originalScale, chosenScale, t);
            chosenImage.transform.localPosition =Vector3.Lerp(originalPosition,chosenPositon, t);
            yield return null;
        }
        chosenImage.transform.localScale = chosenScale;
        chosenImage.transform.localPosition = chosenPositon;
    }

    private void ResetChosenImage()
    {
        chosenImage.transform.localScale = originalScale;
        chosenImage.transform.localPosition = originalPosition;
    }
}
