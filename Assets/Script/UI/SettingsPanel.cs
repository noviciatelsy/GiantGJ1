using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Slider BGMVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;
    private GameData gameData;
    private PanelEntrance panelEntranceUI;

    private void Awake()
    {
        panelEntranceUI = GetComponent<PanelEntrance>();
    }

    private void OnEnable()
    {
        // 更新Slider数值
        gameData = SaveManager.Instance.GetRunTimeGameData();
        BGMVolumeSlider.value = gameData.bgmVolume;
        SFXVolumeSlider.value = gameData.sfxVolume;

    }


    public void OnBGMVolumeChanged(float volume)
    {
        gameData.bgmVolume = volume;
    }

    public void OnSFXVolumeChanged(float volume)
    {
        gameData.sfxVolume = volume;
    }

    public void SaveAndReturnButton()
    {
        AudioManager.Instance.LoadVolume();
        SaveManager.Instance.SaveGame();
        panelEntranceUI.HideWithMoveOut();
    }

}
