using UnityEngine;
using System.Collections;

public class EnemyCommonLogic : MonoBehaviour
{
    public int EnemyHealth = 20;
    public float impulseStrength = 12f;
    private EnemyHealthChange enemyHealthChange;

    public bool canDamaged = true;

    void Start()
    {
        enemyHealthChange = GetComponentInChildren<EnemyHealthChange>();
        if(enemyHealthChange == null) Debug.Log("没有找到EnemyHealthChange组件");
        else
        {
            enemyHealthChange.EnemyMaxHealth = EnemyHealth;
            enemyHealthChange.EnemyCurHealth = EnemyHealth;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ondamage(int Damage)
    {
        if(!canDamaged) return;
        EnemyHealth -= Damage;
        if (enemyHealthChange != null)
        {
            enemyHealthChange.EnemyCurHealth = EnemyHealth;
            enemyHealthChange.ChangeHealth();
        }

        Debug.Log("目前敌人血量"+EnemyHealth);
        if(EnemyHealth <= 0)
        { 
            ToDestroySelf();
        }
    }

    public void ApplyImpulseToBoat()
    {
        if (BoatMove.Instance == null) return;

        Vector3 dir = (BoatMove.Instance.transform.position - transform.position).normalized;
        dir.y = 0f;

        //Vector3 impulse = dir * impulseStrength;
        Vector3 impulse = new Vector3(dir.x * 0.9f, dir.y * 0.6f, dir.z * 1.4f) * impulseStrength;
        BoatMove.Instance.AddImpulse(impulse);
    }


    public virtual void ToDestroySelf()
    {

    }

    public IEnumerator DestroySelf()
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y - 2.0f, transform.position.z - 1.0f);
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
