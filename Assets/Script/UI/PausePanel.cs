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
        GameManager.Instance.playerManager.DestroyPlayer();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
