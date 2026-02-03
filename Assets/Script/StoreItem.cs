using System;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class StoreItem : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject itemTooltip;
    [SerializeField] SpriteRenderer itemSprite;
    [SerializeField] private TextMeshPro itemName;
    [SerializeField] private TextMeshPro itemDescription;
    [SerializeField] private TextMeshPro woodCost;
    [SerializeField] private TextMeshPro ironCost;
    [SerializeField] private CubeItemDataSO[] cubeDatas;
    private CubeItemDataSO currentCubeData;

    [Space]
    [Header("Audio")]
    [SerializeField] private AudioEventSO setInteractableSFX;
    [SerializeField] private AudioEventSO buyItemSFX;
    private void Awake()
    {
        itemTooltip.SetActive(false);
    }

    private void OnEnable()
    {
        SelectCurrentCubeData();
    }

    private void SelectCurrentCubeData()
    {
        int cubeDataIndex = Random.Range(0, cubeDatas.Length);
        currentCubeData = cubeDatas[cubeDataIndex];
        itemSprite.sprite=currentCubeData.spriteToShow;
        itemName.text=currentCubeData.itemName;
        itemDescription.text=currentCubeData.itemDescription;
        woodCost.text="x"+currentCubeData.piece.woodCost;
        ironCost.text="x"+currentCubeData.piece.IronCost;
    }

    public void Interact()
    {
        if(currentCubeData==null)
        {
            return;
        }
        if(!StorageManager.Instance.inventoryStorage.HasEnoughWood(currentCubeData.piece.woodCost))
        {
            return;
        }
        if(!StorageManager.Instance.inventoryStorage.HasEnoughIron(currentCubeData.piece.IronCost))
        {
            return; 
        }
        StorageManager.Instance.inventoryStorage.ConsumeWood(currentCubeData.piece.woodCost);
        StorageManager.Instance.inventoryStorage.ConsumeIron(currentCubeData.piece.IronCost);
        StorageManager.Instance.GetItem(currentCubeData, 1);
        buyItemSFX.Play();
        currentCubeData = null;
        gameObject.SetActive(false);
    }

    public void SetInterable(bool isInteractable)
    {
        if(currentCubeData== null)
        {
            return;
        }
        if(isInteractable)
        {
            setInteractableSFX.Play();
        }
        itemTooltip.SetActive(isInteractable);
    }
}
