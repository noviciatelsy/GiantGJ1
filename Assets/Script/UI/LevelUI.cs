using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance;
    public StoragePanel storagePanel { get; private set; }
    public CubeDetails[] cubeDetails {  get; private set; }
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
        cubeDetails = GetComponentsInChildren<CubeDetails>(true);
        storagePanelEnabled = storagePanel.gameObject.activeSelf;
    }

    private void OnEnable()
    {
        GameManager.Instance.playerManager.onPlayerSpawned += BindCubeDetailsUIToPlayer;
        BindCubeDetailsUIToPlayer();

    }

    private void OnDisable()
    {
        GameManager.Instance.playerManager.onPlayerSpawned -= BindCubeDetailsUIToPlayer;
        if (storagePanelEnabled)
        {
            ExitStoragePanel();
        }
    }

    public void ToggleStoragePanel(InventoryStorage currentStorage,PLControl interactPlayer)
    {
        storagePanelEnabled = !storagePanelEnabled;
        if (storagePanelEnabled) // 如果要开启面板
        {
            PauseGame(true);
            storagePanel.SetupStoragePanel(currentStorage,interactPlayer); // 传入当前storage和interactPlayer
            storagePanel.gameObject.SetActive(true);

        }
        else
        {
            PauseGame(false);
            storagePanel.gameObject.SetActive(false);
            storagePanel.ResetStoragePanel();
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
            ToggleStoragePanel(null,null);
        }

    }

    private void BindCubeDetailsUIToPlayer()
    {
        if(GameManager.Instance.playerManager.playerKeyboard!=null)
        {
            cubeDetails[0].gameObject.SetActive(true);
            GameManager.Instance.playerManager.playerKeyboard.GetComponent<PLControl>().SetCubeDetailsUI(cubeDetails[0]);
        }
        else
        {
            cubeDetails[0].gameObject.SetActive(false);
        }
        if (GameManager.Instance.playerManager.playerGamepad != null)
        {
            cubeDetails[1].gameObject.SetActive(true);
            GameManager.Instance.playerManager.playerGamepad.GetComponent<PLControl>().SetCubeDetailsUI(cubeDetails[1]);
        }
        else
        {
            cubeDetails[1].gameObject.SetActive(false);
        }
    }
}
