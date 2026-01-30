using UnityEngine;

/// <summary>
/// 音频事件类别。
/// 用来告诉 AudioEventSO 默认应该通过哪种方式播放。
/// </summary>
public enum AudioEventPlayMode
{
    BGM,    // 背景音乐（走 AudioManager.PlayBGM）
    UI,     // UI 音效（走 AudioManager.PlayUI）
    SFX2D,  // 普通 2D 音效（走 AudioManager.PlaySFX2D）
    LoopedSFX2D,
}

[CreateAssetMenu(menuName = "Audio/Audio Event", fileName = "Audio Event - ")]
public class AudioEventSO : ScriptableObject
{
    [Header("基础配置")]
    [SerializeField] private string audioName;
    // 对应 AudioDatabaseSO.clipCollection 里的 key

    [SerializeField] private AudioEventPlayMode playMode = AudioEventPlayMode.SFX2D;

    [Header("BGM 专用设置")]
    [SerializeField] private bool loopBgm = true;
    // 当 playMode 为 BGM 时，是否循环播放
    // 对于 UI / SFX 类型，这个字段不会被使用

    [Header("音调设置（主要用于 UI / SFX）")]
    [SerializeField] private bool useRandomPitch = false;
    // 是否启用随机音调变化（适合重复的音效听起来不那么单调）

    [Range(0.1f, 3f)]
    [SerializeField] private float minPitch = 0.95f;

    [Range(0.1f, 3f)]
    [SerializeField] private float maxPitch = 1.05f;

    [Tooltip("当不使用随机音调时，使用该固定音调，一般为 1。")]
    [Range(0.1f, 3f)]
    [SerializeField] private float fixedPitch = 1f;

    /// <summary>
    /// 对外公开的只读属性，方便在别处调试时查看这个事件用的是哪个 audioName。
    /// </summary>
    public string AudioName
    {
        get
        {
            return audioName;
        }
    }

    /// <summary>
    /// 计算本次播放应使用的 pitch。
    /// 如果开启了随机音调，则在 [minPitch, maxPitch] 区间随机；
    /// 否则使用 fixedPitch。
    /// </summary>
    private float GetPitch()
    {
        if (!useRandomPitch)
        {
            return fixedPitch;
        }

        // 防御性写法：如果不小心把 min 和 max 填反了，就自动交换
        float min = minPitch;
        float max = maxPitch;

        if (max < min)
        {
            float temp = max;
            max = min;
            min = temp;
        }

        return Random.Range(min, max);
    }


    public void Play()
    {
        // 1. AudioManager 单例是否存在
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("[AudioEventSO] AudioManager.Instance 为 null，无法播放音频事件：" + name);
            return;
        }

        float pitch = GetPitch();


        switch (playMode)
        {
            case AudioEventPlayMode.BGM:
                AudioManager.Instance.PlayBGM(audioName, loopBgm);
                break;
            case AudioEventPlayMode.UI:
                AudioManager.Instance.PlayUI(audioName, pitch);
                break;

            case AudioEventPlayMode.SFX2D:
                AudioManager.Instance.PlaySFX2D(audioName, pitch);
                break;
        }
    }


    public AudioSource PlayLoop2D()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("[AudioEventSO] AudioManager.Instance 为 null，无法播放循环音频事件：" + name);
            return null;
        }

        float pitch = GetPitch();

        switch (playMode)
        {
            case AudioEventPlayMode.LoopedSFX2D:
                // 循环 2D SFX
                return AudioManager.Instance.PlayLoopSFX2D(audioName, pitch);
        }

        return null;
    }

    /// <summary>
    /// 停止一个通过 PlayLoop/PlayLoopAtPosition 播放的循环 SFX。
    /// 本质上是帮你转发给 AudioManager.StopLoopSFX。
    /// </summary>
    public void StopLoop(AudioSource loopSource)
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("[AudioEventSO] AudioManager.Instance 为 null，无法停止循环音频事件：" + name);
            return;
        }

        if (loopSource == null)
        {
            return;
        }

        AudioManager.Instance.StopLoopSFX(loopSource);
    }
}
