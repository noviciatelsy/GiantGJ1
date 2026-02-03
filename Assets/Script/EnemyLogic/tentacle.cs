using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class tentacle : EnemyCommonLogic
{
    [Header("Audio")]
    [SerializeField] AudioEventSO boatBangSFX;
    [SerializeField] AudioEventSO rockBreakSFX;

    private bool isDestroy = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        CubeBase cube = other.GetComponent<CubeBase>();
        if (cube != null)
        {
            //ÄÍ¾Ã¶È>0²Å»áÅö×²
            if (cube.CheckCubeHealth() > 0 && !isDestroy)
            {
                //isDestroy = true;
                cube.CubeCrush();
                ApplyImpulseToBoat();

                Debug.Log("Crush");
                boatBangSFX.Play();

                //ToDestroySelf();
                Ondamage(10);
            }
        }
    }

    public override void ToDestroySelf()
    {
        base.ToDestroySelf();
        rockBreakSFX.Play();
        StartCoroutine(DestroySelf());
    }

}
