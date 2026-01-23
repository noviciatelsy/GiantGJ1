using UnityEngine;
using System.Collections;

public class Rock : EnemyCommonLogic
{
    private bool isDestroy = false;
    private void OnTriggerEnter(Collider other)
    {
        CubeBase cube = other.GetComponent<CubeBase>();
        if (cube != null)
        {
            //ÄÍ¾Ã¶È>0²Å»áÅö×²
            if (cube.CheckCubeHealth() > 0 && !isDestroy)
            {
                isDestroy = true;  
                cube.CubeCrush();
                Debug.Log("Crush");
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
        Vector3 targetPos = new Vector3 (transform.position.x, transform.position.y - 2.0f, transform.position.z);
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
