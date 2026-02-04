using UnityEngine;

public class RiverLogic : MonoBehaviour
{
    public GeRiver geRiver;

    void Start()
    {
        //geRiver = FindAnyObjectByType<GeRiver>();
    }

    void FixedUpdate()
    {
        if(transform.position.z < -300f)
        {
            geRiver.GeNewRiver(transform.position.x);
            geRiver.DestoryRiver(this.gameObject);
        }
    }
}
