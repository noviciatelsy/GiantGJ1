using UnityEngine;

public class RiverLogic : MonoBehaviour
{
    private GeRiver geRiver;

    void Start()
    {
        geRiver = FindAnyObjectByType<GeRiver>();
    }

    void FixedUpdate()
    {
        if(transform.position.z < -500f)
        {
            geRiver.GeNewRiver();
            geRiver.DestoryRiver(this.gameObject);
        }
    }
}
