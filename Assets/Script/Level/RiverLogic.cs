using UnityEngine;

public class RiverLogic : MonoBehaviour
{
    public GeRiver geRiver;

    private float spacing = 350f;
    private float bound;
    private float wrapWidth;

    void Start()
    {
        bound = spacing * 2.0f;     // ±185
        wrapWidth = spacing * 3f;
    }

    void FixedUpdate()
    {
        if(transform.position.z < -300f)
        {
            // 只让中间列负责生成
            if (Mathf.Abs(transform.position.x) < 180f)
            {
                geRiver.GeNewRiver(transform.position.x);
            }

            geRiver.DestoryRiver(this.gameObject);
            return;
        }

        Vector3 pos = transform.position;
        if (pos.x < -bound)
        {
            transform.position = new Vector3(
                pos.x + wrapWidth,
                pos.y,
                pos.z
            );
            Debug.Log(transform.position);
            return;
        }
        else if (pos.x > bound)
        {
            transform.position = new Vector3(
                pos.x - wrapWidth,
                pos.y,
                pos.z
            );
            Debug.Log(transform.position);
            return;
        }
    }


}
