using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private GameManager gameManager;


    private void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();
    }

    public void DisableAllPlayersInput()
    {
        if(gameManager.playerManager.playerKeyboard!=null)
        {
            gameManager.playerManager.playerKeyboard.GetComponent<PlayerInput>().currentActionMap.Disable();
        }
        if(gameManager.playerManager.playerGamepad!=null)
        {
            gameManager.playerManager.playerGamepad.GetComponent<PlayerInput>().currentActionMap.Disable();
        }

    }

    public void EnableAllPlayersInput()
    {
        if (gameManager.playerManager.playerKeyboard != null)
        {
            gameManager.playerManager.playerKeyboard.GetComponent<PlayerInput>().currentActionMap.Enable();
        }
        if (gameManager.playerManager.playerGamepad != null)
        {
            gameManager.playerManager.playerGamepad.GetComponent<PlayerInput>().currentActionMap.Enable();
        }
    }
}
