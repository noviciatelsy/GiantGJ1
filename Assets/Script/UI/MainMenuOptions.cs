using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuOptions : MonoBehaviour
{
    [SerializeField] private string InGameSceneName = "LevelScene";
    [SerializeField] private AudioEventSO mainMenuBGM;
    private SettingsPanel settingsPanel;

    private void Awake()
    {
        mainMenuBGM.Play();
        AudioManager.Instance.StopAllLoopSFX();
        settingsPanel=GetComponentInChildren<SettingsPanel>(true);
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
        settingsPanel.gameObject.SetActive(true);
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
