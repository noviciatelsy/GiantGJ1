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
        GameObject player=Instantiate(playerKeyboardPrefab,singlePlayerSpwanPosition,Quaternion.identity,transform);
    }

    private void SpawnMultiPlayerOne()
    {
        GameObject player = Instantiate(playerKeyboardPrefab, multiPlayerOneSpawnPosition, Quaternion.identity,transform);
    }
    private void SpawnMultiPlayerTwo()
    {
        GameObject player = Instantiate(playerGamepadPrefab, multiPlayerTwoSpawnPosition, Quaternion.identity,transform);
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
