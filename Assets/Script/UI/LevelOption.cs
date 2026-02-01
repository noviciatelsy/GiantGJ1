using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelOption : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI levelDisplayName;
    [SerializeField] private TextMeshProUGUI levelDescription;
    private LevelDataSO levelData;



    public void UpdateLevelOptionDetails(LevelDataSO levelData)
    {
        this.levelData = levelData;
        levelDisplayName.text = levelData.levelDisplayName;
        levelDescription.text = levelData.levelDescription;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(levelData!=null)
        {
            FadeScreen.Instance.PlayFade(() =>
            {
                SceneManager.LoadScene(levelData.levelSceneName);
            });
        }
    }
}
