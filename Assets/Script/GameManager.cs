using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public InputManager inputManager;

    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        inputManager = GetComponentInChildren<InputManager>();
    }
}
