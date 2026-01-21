using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuOptions : MonoBehaviour
{
    [SerializeField] private string InGameSceneName = "LevelScene";
    public void SinglePlayerButton()
    {
        FadeScreen.Instance.PlayFade(() =>
        {
            GameManager.Instance.playerManager.SetSinglePlayer(); // 设置为单人模式
            SceneManager.LoadScene(InGameSceneName);
            GameManager.Instance.playerManager.SpawnPlayer();
        });

    }

    public void MutiPlayerButton()
    {
        FadeScreen.Instance.PlayFade(() =>
        {
            GameManager.Instance.playerManager.SetMultiPlayer(); // 设置为多人模式
            SceneManager.LoadScene(InGameSceneName);
            GameManager.Instance.playerManager.SpawnPlayer();
        });
    }

    public void SettingsButton()
    {

    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
