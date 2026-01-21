using UnityEngine;

public class GlobalGameRoot : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
