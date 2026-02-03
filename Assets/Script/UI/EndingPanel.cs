using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndingPanel : MonoBehaviour
{
    private bool timeIsPaused = false;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private AudioEventSO levelWinSFX;

    private void OnEnable()
    {
        if (GameManager.Instance.playerManager.playerKeyboard != null)
        {
            GameManager.Instance.playerManager.playerKeyboard.GetComponent<PlayerInput>().actions.FindActionMap("UI").Disable();
        }

        if (GameManager.Instance.playerManager.playerGamepad != null)
        {

            GameManager.Instance.playerManager.playerGamepad.GetComponent<PlayerInput>().actions.FindActionMap("UI").Disable();
        }

        PauseGame(true );
        levelWinSFX.Play();
    }

    private void OnDisable()
    {
        PauseGame(false );
    }

    public void ReturnToMainMenuButton()
    {
        FadeScreen.Instance.PlayFade(() =>
        {
            GameManager.Instance.playerManager.DestroyPlayer();
            StorageManager.Instance.inventoryStorage.ClearStorage();
            SceneManager.LoadScene(mainMenuSceneName);
        });
    }

    private void PauseGame(bool pause)
    {
        if (pause)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    private void Pause()
    {
        if (timeIsPaused) return;
        timeIsPaused = true;
        Time.timeScale = 0f;
        GameManager.Instance.inputManager.DisableAllPlayersInput(); // 禁用玩家操作
        Debug.Log("pause");
    }

    private void Resume()
    {
        if (!timeIsPaused) return;
        timeIsPaused = false;
        Time.timeScale = 1f;
        GameManager.Instance.inputManager.EnableAllPlayersInput(); // 启用玩家操作
    }
}
