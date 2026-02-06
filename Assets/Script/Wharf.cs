using UnityEngine;
using UnityEngine.SceneManagement;

public class Wharf : MonoBehaviour
{
    [SerializeField] private string storeSceneName = "Store";
    [SerializeField] private bool isEndingLine;

    public int levelIncrease = 1; // 关卡进度条难度
    private bool hasEntered;
    private void OnTriggerEnter(Collider other)
    {
        CubeBase cube = other.GetComponent<CubeBase>();

        if (cube != null)
        {
            if (hasEntered)
            {
                return;
            }
            GameManager.Instance.levelPassed += levelIncrease;
            hasEntered = true;
            if (GameManager.Instance.levelPassed == 12)
            {
                LevelUI.Instance.endingPanel.gameObject.SetActive(true);
                return;
            }
            if (!isEndingLine)
            {
                FadeScreen.Instance.PlayFade(() =>
                {
                    GameManager.Instance.playerManager.DestroyPlayer();
                    AudioManager.Instance.StopAllLoopSFX();
                    GameManager.Instance.playerManager.SpawnPlayerInStore();
                    SceneManager.LoadScene(storeSceneName);

                });
            }
            else
            {
                LevelUI.Instance.endingPanel.gameObject.SetActive(true);
            }
        }
    }
}
