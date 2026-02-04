using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour
{
    private tentacle owner;
    [Header("Attack")]
    public float attackCooldown = 2f;
    public float attackDamage = 10f;
    private bool isAttacking = false;
    private bool isCooldown = false;

    [Header("Attack Anim")]
    public Transform spriteRoot;   // 放 SpriteRenderer 的那个物体
    public float windUpTime = 0.2f;
    public float attackTime = 0.1f;
    public float recoverTime = 0.1f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = spriteRoot.localScale;
    }


    private void OnTriggerEnter(Collider other)
    {
        CubeBase cube = other.GetComponent<CubeBase>();
        if (cube == null) return;

        Debug.Log("进入攻击范围");
        TryAttack(cube);
    }

    public void TryAttack(CubeBase cube)
    {
        if (isAttacking || isCooldown) return;

        StartCoroutine(AttackCoroutine(cube));
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isCooldown = false;
    }

    private IEnumerator AttackCoroutine(CubeBase target)
    {
        isAttacking = true;

        Vector3 windUpScale = new Vector3(
            originalScale.x * 0.5f,
            originalScale.y * 2.5f,
            originalScale.z
        );

        Vector3 attackScale = new Vector3(
            originalScale.x * 2.5f,
            originalScale.y * 0.5f,
            originalScale.z
        );

        // 前摇
        yield return ScaleTo(spriteRoot, windUpScale, windUpTime);

        // 攻击
        yield return ScaleTo(spriteRoot, attackScale, attackTime);

        // 真正造成伤害（这里时机最准）
        if (target != null && target.CheckCubeHealth() > 0)
        {
            target.CubeCrush();
        }

        // 恢复
        yield return ScaleTo(spriteRoot, originalScale, recoverTime);

        isAttacking = false;
    }


    private IEnumerator ScaleTo(Transform target, Vector3 to, float duration)
    {
        Vector3 from = target.localScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            target.localScale = Vector3.Lerp(from, to, t);
            yield return null;
        }

        target.localScale = to;
    }

}
