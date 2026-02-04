using UnityEngine;
using UnityEngine.SceneManagement;

public class Wharf : MonoBehaviour
{
    [SerializeField] private string storeSceneName = "Store";
    [SerializeField] private bool isEndingLine;
    private void OnTriggerEnter(Collider other)
    {
        CubeBase cube = other.GetComponent<CubeBase>();
        if (cube != null)
        {
            GameManager.Instance.levelPassed += 1;
            if (GameManager.Instance.levelPassed == 4)
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
