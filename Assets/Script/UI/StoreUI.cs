using UnityEngine;
using UnityEngine.InputSystem;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private AudioEventSO storeBGM;
    [SerializeField] private AudioEventSO waveEnvironmentSFX;

    public PausePanel pausePanel { get; private set; }

    private bool timeIsPaused = false;
    private bool pausePanelEnabled;

    private void Awake()
    {
        pausePanel = GetComponentInChildren<PausePanel>(true);
        pausePanel.enabled = pausePanel.gameObject.activeSelf;
        storeBGM.Play();
        waveEnvironmentSFX.PlayLoop2D();
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
        if (GameManager.Instance.playerManager.playerKeyboard != null)
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
        if (pause)
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


    private void BindUIToPlayer()
    {
        if (GameManager.Instance.playerManager.playerKeyboard != null)
        {
            GameManager.Instance.playerManager.playerKeyboard.GetComponent<PlayerInput>().actions.FindActionMap("UI").Enable();
        }
        if (GameManager.Instance.playerManager.playerGamepad != null)
        {

            GameManager.Instance.playerManager.playerGamepad.GetComponent<PlayerInput>().actions.FindActionMap("UI").Enable();
        }
    }
}
