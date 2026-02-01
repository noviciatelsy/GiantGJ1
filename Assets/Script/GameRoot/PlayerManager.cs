using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerKeyboardPrefab;
    [SerializeField] private GameObject playerGamepadPrefab;
    [SerializeField] private Vector3 singlePlayerSpwanPositionOnBoat;
    [SerializeField] private Vector3 multiPlayerOneSpawnPositionOnBoat;
    [SerializeField] private Vector3 multiPlayerTwoSpawnPositionOnBoat;
    [Space]
    [SerializeField] private Vector3 PlayerSpawnPositionInStore;
    private bool isMultiPlayer=false;
    public event Action onPlayerSpawned;

    public GameObject playerKeyboard {  get; private set; }
    public GameObject playerGamepad {  get; private set; }
    public void SpawnPlayerOnBoat()
    {
        if (isMultiPlayer)
        {
            SpawnMultiPlayerOneOnBoat();
            SpawnMultiPlayerTwoOnBoat();
        }
        else
        {
            SpawnSinglePlayerOnBoat();
        }
        onPlayerSpawned?.Invoke();
    }

    public void SpawnPlayerInStore()
    {
        if (isMultiPlayer)
        {
            SpawnMultiPlayerOneInStore();
            SpawnMultiPlayerTwoInStore();
        }
        else
        {
            SpawnSinglePlayerInStore();
        }
        onPlayerSpawned?.Invoke();
    }

    private void SpawnSinglePlayerOnBoat()
    {
        playerKeyboard=Instantiate(playerKeyboardPrefab,singlePlayerSpwanPositionOnBoat,Quaternion.identity,transform);
        playerKeyboard.GetComponent<PLControl>().SetIsOnBoat(true);
    }

    private void SpawnSinglePlayerInStore()
    {
        playerKeyboard = Instantiate(playerKeyboardPrefab, PlayerSpawnPositionInStore, Quaternion.identity, transform);
        playerKeyboard.GetComponent<PLControl>().SetIsOnBoat(false);
    }

    private void SpawnMultiPlayerOneOnBoat()
    {
        playerKeyboard = Instantiate(playerKeyboardPrefab, multiPlayerOneSpawnPositionOnBoat, Quaternion.identity,transform);
        playerKeyboard.GetComponent<PLControl>().SetIsOnBoat(true);
    }

    private void SpawnMultiPlayerOneInStore()
    {
        playerKeyboard = Instantiate(playerKeyboardPrefab, PlayerSpawnPositionInStore, Quaternion.identity, transform);
        playerKeyboard.GetComponent<PLControl>().SetIsOnBoat(false);
    }
    private void SpawnMultiPlayerTwoOnBoat()
    {
        playerGamepad = Instantiate(playerGamepadPrefab, multiPlayerTwoSpawnPositionOnBoat, Quaternion.identity,transform);
        playerGamepad.GetComponent<PLControl>().SetIsOnBoat(true);
    }

    private void SpawnMultiPlayerTwoInStore()
    {
        playerGamepad = Instantiate(playerGamepadPrefab, PlayerSpawnPositionInStore, Quaternion.identity, transform);
        playerGamepad.GetComponent<PLControl>().SetIsOnBoat(false);
    }

    public void SetSinglePlayer()
    {
        isMultiPlayer = false;
    }

    public void SetMultiPlayer()
    {
        isMultiPlayer = true;
    }

    public void DestroyPlayer()
    {
        if(playerGamepad!=null)
        {
            Destroy(playerGamepad);
        }
        if(playerKeyboard!=null)
        {
            Destroy(playerKeyboard);
        }
          
    }

}
