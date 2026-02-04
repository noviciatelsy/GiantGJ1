using UnityEngine;

public static class GlobalGameData
{
    public static Vector3 BoatVelocity = Vector3.zero;

    public static float BoatTravelDistance = 0f;

    public static void ResetTravelData()
    {
        BoatTravelDistance = 0f;
    }
}
