using UnityEngine;
using System.Collections;

public class Wave : EnemyCommonLogic
{
    [Header("Audio")]
    [SerializeField] AudioEventSO boatBangSFX;
    [SerializeField] AudioEventSO waveBreakSFX;

    private bool isDestroy = false;
    private float moveSpeed = 20.0f;

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
            //耐久度>0才会碰撞
            if (cube.CheckCubeHealth() > 0 && !isDestroy)
            {
                isDestroy = true;
                cube.CubeCrush();
                ApplyImpulseToBoat();

                Debug.Log("Crush");
                boatBangSFX.Play();
                waveBreakSFX.Play();
                ToDestroySelf();
            }
        }
    }

    public override void ToDestroySelf()
    {
        base.ToDestroySelf();
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
