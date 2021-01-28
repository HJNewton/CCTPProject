using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kelp : MonoBehaviour
{
    [Header("Kelp Setup")]
    public float triggerRange;
    public float foodToGive;
    public bool canBeEaten;

    private void Awake()
    {
        this.GetComponent<SphereCollider>().radius = triggerRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fish") && canBeEaten && other.GetComponent<FishBehaviour>().currentFishState == FishBehaviour.FishState.Feeding) // If object is a fish, kelp is ready to consume and fish is hungry
        {
            Debug.Log("Fish In Range");

            other.GetComponent<FishHealth>().AddFood(foodToGive);
            other.GetComponent<FishBehaviour>().fishDestinationTarget = other.GetComponent<FishBehaviour>().movingTarget;
            other.GetComponent<FishBehaviour>().currentFishState = FishBehaviour.FishState.Roaming;

            Destroy(gameObject, 3);
        }
    }
}
