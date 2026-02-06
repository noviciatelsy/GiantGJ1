using UnityEngine;
using UnityEngine.SceneManagement;

public class Progressbar : MonoBehaviour
{
    public RectTransform Alltarget;
    public RectTransform target;
    //rect tranform范围: 从-400 - 400;
    public float minY = -400f;
    public float maxY = 400f;

    private int levelPassed;
    void Start()
    {
        levelPassed = 0;
        UpdateSceneVisible();
    }

    void Update()
    {
        UpdateSceneVisible();

        float dis = GlobalGameData.BoatTravelDistance;
        // 只有在Scene 1里才更新进度条
        if (SceneManager.GetActiveScene().buildIndex < 2 || SceneManager.GetActiveScene().buildIndex > 12)
            return;

        if (levelPassed != GameManager.Instance.levelPassed)
        {
            levelPassed = GameManager.Instance.levelPassed;
            levelPassed = Mathf.Clamp(levelPassed, 0, 12);

            float t = levelPassed / 12f;
            float y = Mathf.Lerp(minY, maxY, t);

            Vector2 pos = target.anchoredPosition;
            pos.y = y;
            target.anchoredPosition = pos;
        }

        Vector2 temppos = target.anchoredPosition;
        temppos.y += 1.0f * Time.deltaTime;
        target.anchoredPosition = temppos;
    }

    void UpdateSceneVisible()
    {
        bool show = (SceneManager.GetActiveScene().buildIndex >= 2 && SceneManager.GetActiveScene().buildIndex <= 12);

        if (Alltarget.gameObject.activeSelf != show)
            Alltarget.gameObject.SetActive(show);
    }


}
