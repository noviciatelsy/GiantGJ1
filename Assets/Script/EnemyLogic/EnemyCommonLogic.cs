using UnityEngine;

public class EnemyCommonLogic : MonoBehaviour
{
    public int EnemyHealth = 20;
    public float impulseStrength = 12f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ondamage(int Damage)
    {
        EnemyHealth -= Damage;
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
}
