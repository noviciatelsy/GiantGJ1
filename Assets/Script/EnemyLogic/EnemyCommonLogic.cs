using UnityEngine;

public class EnemyCommonLogic : MonoBehaviour
{
    public int EnemyHealth = 20;

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

    public virtual void ToDestroySelf()
    {

    }
}
