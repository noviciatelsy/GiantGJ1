using UnityEngine;

public class GlobalUI : MonoBehaviour
{
    public static GlobalUI Instance;

    public LevelSelectUI levelSelectUI {  get; private set; }

    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        levelSelectUI = GetComponentInChildren<LevelSelectUI>(true);
    }
}
