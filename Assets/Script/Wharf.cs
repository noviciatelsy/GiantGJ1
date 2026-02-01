using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wharf : MonoBehaviour
{
    [SerializeField] private string storeSceneName = "Store";
    [SerializeField] private AudioEventSO storeBGM;

    private void OnTriggerEnter(Collider other)
    {
        CubeBase cube = other.GetComponent<CubeBase>();
        if (cube != null)
        {
            FadeScreen.Instance.PlayFade(() =>
            {
                GameManager.Instance.playerManager.DestroyPlayer();
                GameManager.Instance.playerManager.SpawnPlayerInStore();
                SceneManager.LoadScene(storeSceneName);
                storeBGM.Play();
            });
        }
    }
}
