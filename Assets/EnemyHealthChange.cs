using UnityEngine;
using System.Collections;

public class EnemyHealthChange : MonoBehaviour
{
    public int EnemyMaxHealth;
    public int EnemyCurHealth;

    [SerializeField] private SpriteRenderer spriteBack;
    [SerializeField] private SpriteRenderer spriteLeft;
    [SerializeField] private SpriteRenderer spriteMid;
    [SerializeField] private SpriteRenderer spriteRight;

    // 血条的最大宽度
    private float maxWidth;

    // 左边和右边的固定宽度
    private float leftWidth;
    private float rightWidth;
    private float decreaseInterval = 0.2f; // 每 0.2 秒减少 1 点血量
    private bool isDecreasingHealth = false;

    void Start()
    {
        // 初始化血量
        EnemyCurHealth = EnemyMaxHealth;

        // 获取背景的宽度，减去左右两部分的宽度
        maxWidth = spriteBack.bounds.size.x - spriteLeft.bounds.size.x - spriteRight.bounds.size.x;

        // 获取左边和右边部分的宽度
        leftWidth = spriteLeft.bounds.size.x;
        rightWidth = spriteRight.bounds.size.x;

        // 设置初始血条中间部分宽度
        spriteMid.transform.localScale = new Vector3(maxWidth, spriteMid.transform.localScale.y, spriteMid.transform.localScale.z);

        // 设置右边的初始位置
        spriteRight.transform.localPosition = new Vector3(leftWidth + maxWidth, spriteRight.transform.localPosition.y, spriteRight.transform.localPosition.z);
    }

    void Update()
    {

    }

    private IEnumerator DecreaseHealth()
    {
        while (EnemyCurHealth > 0)
        {
            EnemyCurHealth -= 1; // 每 0.2 秒减少 1 点血量
            ChangeHealth(); // 更新血条
            yield return new WaitForSeconds(decreaseInterval); // 每隔 0.2 秒执行一次
        }

        // 血量为 0 时停止减少
        isDecreasingHealth = false;
    }

    public void ChangeHealth()
    {
        // 防止血量溢出或为负值
        EnemyCurHealth = Mathf.Clamp(EnemyCurHealth, 0, EnemyMaxHealth);

        // 根据当前血量比例计算中间部分的宽度
        float healthPercentage = (float)EnemyCurHealth / (float)EnemyMaxHealth;
        float currentWidth = maxWidth * healthPercentage;

        // 更新中间血条的宽度
        spriteMid.transform.localScale = new Vector3(currentWidth, spriteMid.transform.localScale.y, spriteMid.transform.localScale.z);

        // 更新右边血条的位置，使其始终与中间部分对齐
        spriteRight.transform.localPosition = new Vector3(leftWidth + currentWidth, spriteRight.transform.localPosition.y, spriteRight.transform.localPosition.z);
    }
}
