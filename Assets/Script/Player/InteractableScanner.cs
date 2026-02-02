using System.Collections.Generic;
using UnityEngine;


public class InteractableScanner : MonoBehaviour
{
    private IInteractable currentInteractableObject;
    private List<IInteractable> nearbyInteractableObjects;
    private float refreshInterval = 0.1f;
    private float refreshTimer = 0f;

    private void Awake()
    {
        nearbyInteractableObjects = new List<IInteractable>();
    }

    private void Update()
    {
        RefreshCurrentInteractableObject();
    }

    private void RefreshCurrentInteractableObject()
    {
        if(refreshTimer<refreshInterval)
        {
            refreshTimer += Time.deltaTime;
            return;
        }
        else
        {
            FindNearestInteractableObject();
            refreshTimer = 0f;
        }
    }

    private void FindNearestInteractableObject()
    {
        IInteractable nearest = null;
        float minDist = float.MaxValue;

        foreach (var interactable in nearbyInteractableObjects )
        {
            if (interactable== null) continue;

            var interactableMb = interactable as MonoBehaviour;
            if (interactableMb == null)
            {
                continue;
            }

            Vector3 targetPos = interactableMb.transform.position;

            float dist = Vector2.Distance(
                 new Vector2(transform.position.x, transform.position.z),
                new Vector2(targetPos.x, targetPos.z)
            );

            if (dist < minDist)
            {
                minDist = dist;
                nearest = interactable;
            }
        }

        // 如果最近的发生变化
        if (nearest != currentInteractableObject)
        {

            // 旧的取消可交互
            if (currentInteractableObject != null)
            {
                currentInteractableObject.SetInterable(false);
            }

            currentInteractableObject= nearest;

            // 新的设为可交互
            if (currentInteractableObject != null)
            {
                currentInteractableObject.SetInterable(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactableObject= other.GetComponent<IInteractable>();
        if (interactableObject!= null)
        {
            if(!nearbyInteractableObjects.Contains(interactableObject))
            {
                nearbyInteractableObjects.Add(interactableObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactableObject = other.GetComponent<IInteractable>();
        if (interactableObject != null)
        {
            if (nearbyInteractableObjects.Contains(interactableObject))
            {
                nearbyInteractableObjects.Remove(interactableObject);
            }
        }
    }
}

public interface IInteractable
{
    public void Interact();

    public void SetInterable(bool isInteractable);

}
