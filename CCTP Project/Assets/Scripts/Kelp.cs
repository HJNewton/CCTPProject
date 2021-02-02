using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kelp : MonoBehaviour
{
    [Header("Kelp Setup")]
    public float triggerRange;
    public float foodToGive;
    public int timesCanBeEaten;
    [SerializeField] private int timesEaten;
    public bool canBeEaten;

    private void Awake()
    {
        this.GetComponent<SphereCollider>().radius = triggerRange;
    }

    private void Update()
    {
        if (!canBeEaten)
        {
            gameObject.tag = "Untagged";
        }

        if (canBeEaten)
        {
            gameObject.tag = "Kelp";
        }

        if (timesEaten >= timesCanBeEaten)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fish") && canBeEaten && other.GetComponent<FishBehaviour>().currentFishState == FishBehaviour.FishState.Feeding) // If object is a fish, kelp is ready to consume and fish is hungry
        {
            timesEaten++;

            Debug.Log("Fish In Range");

            other.GetComponent<FishHealth>().ModifyFood(foodToGive);
            other.GetComponent<FishBehaviour>().fishDestinationTarget = other.GetComponent<FishBehaviour>().movingTarget;
            other.GetComponent<FishBehaviour>().currentFishState = FishBehaviour.FishState.Roaming;
        }
    }
}
