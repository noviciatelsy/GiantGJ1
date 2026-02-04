using UnityEngine;

public class GeBoat : MonoBehaviour
{
    public GameObject Boat;

    public void Awake()
    {
        BoatManager.Instance.GenerateBoat();
    }
}
