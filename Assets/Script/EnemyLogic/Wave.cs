using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wave : EnemyCommonLogic
{
    [Header("Audio")]
    [SerializeField] AudioEventSO boatBangSFX;
    [SerializeField] AudioEventSO waveBreakSFX;

    private bool isDestroy = false;
    private float moveSpeed = 40.0f;

    private HashSet<GameObject> hitCubes = new HashSet<GameObject>();

    public float impulseDuration = 0.5f;
    private bool isImpulseActive = false;
    private float impulseInterval = 0.08f;

    void FixedUpdate()
    {
        // 在 RelativeMove 脚本的基础上继续向 -z 方向匀速移动
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        CubeBase cube = other.GetComponent<CubeBase>();
        if (cube != null)
        {
            // 如果该浮块已经被处理过，跳过
            if (!hitCubes.Contains(other.gameObject))
            {
                // 耐久度 > 0 才会碰撞
                if (cube.CheckCubeHealth() > 0 && !isDestroy)
                {
                    // 造成伤害并加入列表
                    hitCubes.Add(other.gameObject);

                    // 造成伤害
                    cube.CubeCrush();
                    //ApplyImpulseToBoat();

                    // 播放音效
                    boatBangSFX.Play();
                    waveBreakSFX.Play();

                    if (!isImpulseActive)
                    {
                        isImpulseActive = true;
                        StartCoroutine(ApplyImpulseToBoatForDuration());
                    }

                    Debug.Log("Crush");
                }
            }
        }
    }

    private IEnumerator ApplyImpulseToBoatForDuration()
    {
        float elapsedTime = 0f;

        // 每隔一定时间施加冲量
        while (elapsedTime < impulseDuration)
        {
            ApplyImpulseToBoat();  // 施加冲量
            elapsedTime += impulseInterval;  // 增加已过去时间
            yield return new WaitForSeconds(impulseInterval);  // 等待下一个冲量时刻
        }

        // 冲量时间结束
        isImpulseActive = false;
    }

    public override void ToDestroySelf()
    {
        base.ToDestroySelf();
        waveBreakSFX.Play();
        StartCoroutine(DestroySelf());
    }

    private IEnumerator DestroySelf()
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y - 2.0f, transform.position.z);
        yield return MoveToCoroutine(transform, targetPos, 0.3f);
        Destroy(gameObject);
        yield return null;
    }

    private IEnumerator MoveToCoroutine(Transform target, Vector3 targetPos, float duration)
    {
        Vector3 startPos = target.transform.position;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            target.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        target.transform.position = targetPos;
    }
}
