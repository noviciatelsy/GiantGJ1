using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wharf : MonoBehaviour
{
    [SerializeField] private string storeSceneName = "Store";

    private void OnTriggerEnter(Collider other)
    {
        CubeBase cube = other.GetComponent<CubeBase>();
        if (cube != null)
        {
            FadeScreen.Instance.PlayFade(() =>
            {
                GameManager.Instance.playerManager.DestroyPlayer();
                AudioManager.Instance.StopAllLoopSFX();
                GameManager.Instance.playerManager.SpawnPlayerInStore();
                SceneManager.LoadScene(storeSceneName);
            
            });
        }
    }
}
