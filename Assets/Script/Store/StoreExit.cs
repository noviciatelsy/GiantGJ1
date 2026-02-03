using UnityEngine;

public class StoreExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PLControl player = other.GetComponent<PLControl>();
        if (player!= null)
        {
            if(player.footStepAudioSource != null)
            {
                player.footStepAudioSource.Stop();
            }
            GlobalUI.Instance.levelSelectUI.gameObject.SetActive(true);
        }
    }
}
