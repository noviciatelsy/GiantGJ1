using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public InputManager inputManager {  get; private set; }
    public PlayerManager playerManager { get; private set; }

    public int levelPassed;

    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        inputManager = GetComponentInChildren<InputManager>();
        playerManager = GetComponentInChildren<PlayerManager>();
        levelPassed = 0;
    }
}
