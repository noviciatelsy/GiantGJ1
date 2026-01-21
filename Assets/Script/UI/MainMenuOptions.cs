using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuOptions : MonoBehaviour
{
    public void SinglePlayerButton()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void MutiPlayerButton()
    {

    }

    public void SettingsButton()
    {

    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
