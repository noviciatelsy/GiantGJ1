using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuOptions : MonoBehaviour
{
    [SerializeField] private string InGameSceneName = "LevelScene";
    [SerializeField] private AudioEventSO mainMenuBGM;

    private void Awake()
    {
        mainMenuBGM.Play();
        AudioManager.Instance.StopAllLoopSFX();
    }
    public void SinglePlayerButton()
    {
        FadeScreen.Instance.PlayFade(() =>
        {
            GameManager.Instance.playerManager.SetSinglePlayer(); // 设置为单人模式
            GameManager.Instance.playerManager.SpawnPlayerOnBoat();
            SceneManager.LoadScene(InGameSceneName);

        });

    }

    public void MutiPlayerButton()
    {
        FadeScreen.Instance.PlayFade(() =>
        {
            GameManager.Instance.playerManager.SetMultiPlayer(); // 设置为多人模式
            GameManager.Instance.playerManager.SpawnPlayerOnBoat();
            SceneManager.LoadScene(InGameSceneName);
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
