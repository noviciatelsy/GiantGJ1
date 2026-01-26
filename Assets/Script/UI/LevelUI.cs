using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance;
    public StoragePanel storagePanel { get; private set; }

    private bool timeIsPaused = false;
    private bool storagePanelEnabled;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        storagePanel = GetComponentInChildren<StoragePanel>(true);
        storagePanelEnabled = storagePanel.gameObject.activeSelf;
    }

    private void OnDisable()
    {
        if (storagePanelEnabled)
        {
            ExitStoragePanel();
        }
    }

    public void ToggleStoragePanel(InventoryStorage currentStorage)
    {
        storagePanelEnabled = !storagePanelEnabled;
        if (storagePanelEnabled) // 如果要开启面板
        {
            PauseGame(true);
            storagePanel.SetCurrentStorage(currentStorage); // 传入当前storage
            storagePanel.gameObject.SetActive(true);

        }
        else
        {
            PauseGame(false);
            storagePanel.ResetCurrentStorage();
            storagePanel.gameObject.SetActive(false);
        }
    }

    private void PauseGame(bool pause)
    {
        if(pause)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    private void Pause()
    {
        if (timeIsPaused) return;
        timeIsPaused = true;
        Time.timeScale = 0f;
        GameManager.Instance.inputManager.DisableAllPlayersInput(); // 禁用玩家操作
        Debug.Log("pause");
    }

    private void Resume()
    {
        if (!timeIsPaused) return;
        timeIsPaused = false;
        Time.timeScale = 1f;
        GameManager.Instance.inputManager.EnableAllPlayersInput(); // 启用玩家操作
    }

    public void ExitStoragePanel()
    {
        if (storagePanelEnabled)
        {
            ToggleStoragePanel(null);
        }

    }
}
