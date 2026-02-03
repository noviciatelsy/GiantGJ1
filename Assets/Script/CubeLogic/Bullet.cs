using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private AudioEventSO cannonHitSFX;

    public Vector3 direction = Vector3.zero;
    [SerializeField] private float speed = 50f;
    public int damage = 10;


    private float maxDistance = 500f;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // 持续沿方向移动
        transform.position += direction.normalized * speed * Time.deltaTime;

        // 超出范围销毁
        if (Vector3.Distance(startPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //等待完成
        EnemyCommonLogic enemy = other.GetComponent<EnemyCommonLogic>();
        if (enemy != null && enemy.canDamaged)
        {
            Debug.Log("子弹打到敌人");
            enemy.Ondamage(damage); // 对敌人造成伤害
            cannonHitSFX.Play();
            Destroy(gameObject); // 碰到敌人销毁子弹

        }
    }

}
