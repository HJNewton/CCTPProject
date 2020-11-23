using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    [Header("Fish Speed")]
    [SerializeField] private FishGroupManager manager;
    [SerializeField] private float speed;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>(); // Fetches the Fish Group Manager script from the Fish Manager object

        speed = Random.Range(manager.minSpeed, manager.maxSpeed);

        InvokeRepeating("ApplyBoidsRules", 0, 0.05f);
    }

    private void Update()
    {
        //ApplyBoidsRules();

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    // Apply the rules for boids to each fish to enact the relevant behaviour
    void ApplyBoidsRules()
    {
        GameObject[] fishArray;
        fishArray = manager.allFish;

        Vector3 averageCentre = Vector3.zero; // Average centre of the group calculated from each member of the group
        Vector3 averageAvoid = Vector3.zero; // Average avoidance vector of each member of the group
        float groupSpeed = 0.01f; // Average group speed
        float neighbourDistancce; // Distance from nearest neighbour
        int localGroupSize = 0; // Size of the local group of this particular fish

        foreach(GameObject fish in fishArray)
        {
            if(fish != this.gameObject)
            {
                neighbourDistancce = Vector3.Distance(fish.transform.position, this.transform.position);

                if(neighbourDistancce <= manager.distanceToNeighbours)
                {
                    averageCentre += fish.transform.position;
                    localGroupSize ++;

                    if(neighbourDistancce < 1.0f) // How close each fish should be before they should begin avoiding
                    {
                        averageAvoid = averageAvoid + (this.transform.position - fish.transform.position);
                    }

                    FishBehaviour fishManager = fish.GetComponent<FishBehaviour>();
                    groupSpeed = groupSpeed + fishManager.speed;
                }
            }
        }

        if(localGroupSize > 0)
        {
            averageCentre = averageCentre / localGroupSize;
            speed = groupSpeed / localGroupSize;

            Vector3 direction = (averageCentre + averageAvoid) - transform.position;

            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), manager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
