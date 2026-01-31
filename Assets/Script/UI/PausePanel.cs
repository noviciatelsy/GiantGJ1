using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    private PanelEntrance panelEntrance;

    private void Awake()
    {
        panelEntrance = GetComponent<PanelEntrance>();
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
}
