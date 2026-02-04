using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private LevelDataSO[] normalLevelOptions;
    [SerializeField] private LevelDataSO[] specialLevelOptions;

    private LevelOption[] levelOptions;
    private bool timeIsPaused = false;
    private void Awake()
    {

        levelOptions = GetComponentsInChildren<LevelOption>();
    }

    private void OnEnable()
    {

        LevelDataSO randomNormalLevelOption = GetRandomNormalLevelOption();
        if (randomNormalLevelOption != null)
        {
            levelOptions[0].UpdateLevelOptionDetails(randomNormalLevelOption);
        }
        else
        {
            Debug.LogError("[LevelSelectUI] 没有可用的 Normal 关卡数据（normalLevelOptions 为空或全是空引用）。");
            return;
        }

        LevelDataSO[] randomSpecialLevelOptions = GetRandomSpecialLevelOptions(2);
        if (randomSpecialLevelOptions != null && randomSpecialLevelOptions.Length >= 2)
        {
            if (randomSpecialLevelOptions[0] != null)
            {
                levelOptions[1].UpdateLevelOptionDetails(randomSpecialLevelOptions[0]);
            }

            if (randomSpecialLevelOptions[1] != null)
            {
                levelOptions[2].UpdateLevelOptionDetails(randomSpecialLevelOptions[1]);
            }
        }
        else
        {
            Debug.LogError("[LevelSelectUI] 没有足够的 Special 关卡数据（specialLevelOptions 为空或数量不足）。");
        }

        PauseGame(true);
    }

    private void OnDisable()
    {
        PauseGame(false);
    }

    private LevelDataSO[] GetRandomSpecialLevelOptions(int optionAmount)
    {
        if (optionAmount <= 0)
        {
            return new LevelDataSO[0];
        }

        List<LevelDataSO> pool = BuildValidUniquePool(specialLevelOptions, shouldBeNormalRoute: false);

        if (pool.Count == 0)
        {
            return null;
        }

        LevelDataSO[] results = new LevelDataSO[optionAmount];

        // 1) 先尽量“无重复”抽取
        int takeCount = Mathf.Min(optionAmount, pool.Count);
        Shuffle(pool);

        for (int i = 0; i < takeCount; i++)
        {
            results[i] = pool[i];
        }

        // 2) 如果 special 不够 2 个，就允许重复补齐（至少别让 UI 直接炸）
        for (int i = takeCount; i < optionAmount; i++)
        {
            results[i] = pool[Random.Range(0, pool.Count)];
        }

        return results;

    }

    private LevelDataSO GetRandomNormalLevelOption()
    {
        List<LevelDataSO> pool = BuildValidUniquePool(normalLevelOptions, shouldBeNormalRoute: true);

        if (pool.Count == 0)
        {
            return null;
        }

        return pool[Random.Range(0, pool.Count)];
    }

    private List<LevelDataSO> BuildValidUniquePool(LevelDataSO[] source, bool shouldBeNormalRoute)
    {
        List<LevelDataSO> pool = new List<LevelDataSO>();
        HashSet<LevelDataSO> unique = new HashSet<LevelDataSO>();

        if (source == null || source.Length == 0)
        {
            return pool;
        }

        for (int i = 0; i < source.Length; i++)
        {
            LevelDataSO data = source[i];
            if (data == null)
            {
                continue;
            }

            // 这里用 isNormalRoute 做一个“校验”，避免你不小心把 SO 拖错数组还不知道
            if (data.isNormalRoute != shouldBeNormalRoute)
            {
                Debug.LogWarning($"[LevelSelectUI] LevelDataSO '{data.name}' 的 isNormalRoute = {data.isNormalRoute}，" +
                                 $"但它被放进了 {(shouldBeNormalRoute ? "normalLevelOptions" : "specialLevelOptions")}（可能拖错了）。");
            }

            if (unique.Add(data))
            {
                pool.Add(data);
            }

            if(GlobalGameData.isEnd == true)
            {
                //失败ui
                //返回主界面
            }
        }

        return pool;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
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

}
