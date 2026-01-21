using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameRoot : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //SceneManager.LoadScene(mainMenuSceneName);
    }

    public void LoadLevel(string Levelid)
    {
        SceneManager.LoadScene(Levelid);
    }
}
