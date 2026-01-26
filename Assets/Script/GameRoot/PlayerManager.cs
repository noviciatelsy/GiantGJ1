using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerKeyboardPrefab;
    [SerializeField] private GameObject playerGamepadPrefab;
    [SerializeField] private Vector3 singlePlayerSpwanPosition;
    [SerializeField] private Vector3 multiPlayerOneSpawnPosition;
    [SerializeField] private Vector3 multiPlayerTwoSpawnPosition;
    private bool isMultiPlayer=false;

    public GameObject playerKeyboard {  get; private set; }
    public GameObject playerGamepad {  get; private set; }
    public void SpawnPlayer()
    {
        if (isMultiPlayer)
        {
            SpawnMultiPlayerOne();
            SpawnMultiPlayerTwo();
        }
        else
        {
            SpawnSinglePlayer();
        }
    }

    private void SpawnSinglePlayer()
    {
        playerKeyboard=Instantiate(playerKeyboardPrefab,singlePlayerSpwanPosition,Quaternion.identity,transform);
    }

    private void SpawnMultiPlayerOne()
    {
        playerKeyboard = Instantiate(playerKeyboardPrefab, multiPlayerOneSpawnPosition, Quaternion.identity,transform);
    }
    private void SpawnMultiPlayerTwo()
    {
        playerGamepad = Instantiate(playerGamepadPrefab, multiPlayerTwoSpawnPosition, Quaternion.identity,transform);
    }

    public void SetSinglePlayer()
    {
        isMultiPlayer = false;
    }

    public void SetMultiPlayer()
    {
        isMultiPlayer = true;
    }

}
