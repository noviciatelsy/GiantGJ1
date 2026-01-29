using UnityEngine;
using UnityEngine.InputSystem;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance;
    public StoragePanel storagePanel { get; private set; }
    public PausePanel pausePanel { get; private set; }
    public CubeDetailsUI[] cubeDetails {  get; private set; }
    private bool timeIsPaused = false;
    private bool storagePanelEnabled;
    private bool pausePanelEnabled;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        storagePanel = GetComponentInChildren<StoragePanel>(true);
        pausePanel= GetComponentInChildren<PausePanel>(true);
        cubeDetails = GetComponentsInChildren<CubeDetailsUI>(true);
        storagePanelEnabled = storagePanel.gameObject.activeSelf;
        pausePanelEnabled = pausePanel.gameObject.activeSelf;
    }

    private void OnEnable()
    {
        GameManager.Instance.playerManager.onPlayerSpawned += BindUIToPlayer;
        BindUIToPlayer();
        
    }

    private void OnDisable()
    {
        GameManager.Instance.playerManager.onPlayerSpawned -= BindUIToPlayer;
        PauseGame(false);
    }

    private void Update()
    {
        if(GameManager.Instance.playerManager.playerKeyboard!=null)
        {
            if (GameManager.Instance.playerManager.playerKeyboard.GetComponent<PlayerInput>().actions.FindActionMap("UI").FindAction("Pause").WasPerformedThisFrame())
            {
                TogglePausePanel();
            }
        }
        if (GameManager.Instance.playerManager.playerGamepad != null)
        {
            if (GameManager.Instance.playerManager.playerGamepad.GetComponent<PlayerInput>().actions.FindActionMap("UI").FindAction("Pause").WasPerformedThisFrame())
            {
                TogglePausePanel();
            }
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

    public void TogglePausePanel()
    {
        pausePanelEnabled = !pausePanelEnabled;
        if (pausePanelEnabled)
        {
            PauseGame(true);
            pausePanel.gameObject.SetActive(true);
        }
        else
        {
            PauseGame(false);
            pausePanel.HideWithMoveOut();
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

    private void BindUIToPlayer()
    {
        if(GameManager.Instance.playerManager.playerKeyboard!=null)
        {
            cubeDetails[0].gameObject.SetActive(true);
            GameManager.Instance.playerManager.playerKeyboard.GetComponent<PLControl>().SetCubeDetailsUI(cubeDetails[0]);
            GameManager.Instance.playerManager.playerKeyboard.GetComponent<PlayerInput>().actions.FindActionMap("UI").Enable();
        }
        else
        {
            cubeDetails[0].gameObject.SetActive(false);
        }
        if (GameManager.Instance.playerManager.playerGamepad != null)
        {
            cubeDetails[1].gameObject.SetActive(true);
            GameManager.Instance.playerManager.playerGamepad.GetComponent<PLControl>().SetCubeDetailsUI(cubeDetails[1]);
            GameManager.Instance.playerManager.playerGamepad.GetComponent<PlayerInput>().actions.FindActionMap("UI").Enable();
        }
        else
        {
            cubeDetails[1].gameObject.SetActive(false);
        }
    }


}
