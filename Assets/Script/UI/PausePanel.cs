using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    private PanelEntrance panelEntrance;
    private SettingsPanel settingsPanel;

    private void Awake()
    {
        panelEntrance = GetComponent<PanelEntrance>();
        settingsPanel = GetComponentInChildren<SettingsPanel>(true);
    }

    public void HideWithMoveOut()
    {
        panelEntrance.HideWithMoveOut();
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

    public void SettingsButton()
    {
        settingsPanel.gameObject.SetActive(true);
    }
}
