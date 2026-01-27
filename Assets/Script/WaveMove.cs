using UnityEngine;

public class WaveMove : MonoBehaviour
{
    public float Xlimit = 5f;
    public float Ylimit = 3f;
    private float xOffset;
    private float yOffset;
    private float time;

    void Start()
    {
        xOffset = Random.Range(0f, Mathf.PI * 2f);
        yOffset = Random.Range(0f, Mathf.PI * 2f);
        time = 0f;
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime * 0.6f;

        float x = Xlimit * Mathf.Sin(time + xOffset);
        float y = Ylimit * Mathf.Sin(time + yOffset);

        transform.localPosition = new Vector3(x, y, transform.localPosition.z);
    }
}
