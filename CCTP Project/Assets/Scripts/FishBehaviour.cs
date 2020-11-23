using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    // Functionality to add:
    // Enum states for roaming, eating and reproducing
    // Only apply rules when roaming; eating and reproducing should be seperate behaviour that potentially makes them vulnerable

    [Header("Fish Speed")]
    [SerializeField] private FishGroupManager manager;
    [SerializeField] private float speed;
    [SerializeField] bool turning;

    GameObject fishDestinationTarget;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>(); // Fetches the Fish Group Manager script from the Fish Manager object
        fishDestinationTarget = GameObject.FindGameObjectWithTag("Target");

        speed = Random.Range(manager.minSpeed, manager.maxSpeed);

        InvokeRepeating("ApplyBoidsRules", 0, 0.05f);
    }

    private void Update()
    {
       Movement();
    }

    void Movement()
    {
        // Generates a bounds that the fish should stay inside of
        Bounds bounds = new Bounds(manager.transform.position, manager.bounds * 2);

        RaycastHit hit;
        Vector3 direction = Vector3.zero;

        if (!bounds.Contains(transform.position)) // Checks if fish is out of bounds
        {
            turning = true;
            direction = fishDestinationTarget.transform.position - transform.position; // Set direction of out of bounds fish to be towards target
        }

        else if (Physics.Raycast(transform.position, this.transform.forward * 25, out hit)) // Checks if fish is going to hit an object
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal); //Deflects the fish away from the object it is going to hit by reflecting the angle in a "<" like fashion (incoming angle at the top, reflected towards the bottom for example)

            Debug.DrawRay(this.transform.position, this.transform.forward * 25, Color.red);
        }

        else
        {
            turning = false;
        }

        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), manager.rotationSpeed * Time.deltaTime); // A variation of the regular rotation behaviour that just turns towards the centre of the box
        }

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    // Apply the rules for boids to each fish to enact the relevant behaviour
    void ApplyBoidsRules()
    {
        if (!turning) // Only applies the rules so long as the fish is not turning away from the outer wall
        {
            GameObject[] fishArray;
            fishArray = manager.allFish;

            Vector3 averageCentre = Vector3.zero; // Average centre of the group calculated from each member of the group
            Vector3 averageAvoid = Vector3.zero; // Average avoidance vector of each member of the group
            float globalSpeed = 0.01f; // Average group speed
            float neighbourDistancce; // Distance from nearest neighbour
            int localGroupSize = 0; // Size of the local group of this particular fish

            foreach (GameObject fish in fishArray)
            {
                if (fish != this.gameObject)
                {
                    neighbourDistancce = Vector3.Distance(fish.transform.position, this.transform.position);

                    if (neighbourDistancce <= manager.distanceToNeighbours)
                    {
                        averageCentre += fish.transform.position;
                        localGroupSize++;

                        if (neighbourDistancce < 1.0f) // How close each fish should be before they should begin avoiding
                        {
                            averageAvoid = averageAvoid + (this.transform.position - fish.transform.position);
                        }

                        FishBehaviour fishManager = fish.GetComponent<FishBehaviour>();
                        globalSpeed += fishManager.speed;
                    }
                }
            }

            if (localGroupSize > 0)
            {
                averageCentre = averageCentre / localGroupSize + (manager.fishDestinationTarget.transform.position - this.transform.position); // Gets the centre of the local group and moves it towards the target
                speed = globalSpeed / localGroupSize; // Matches fish speed with global speed
                //speed = Random.Range(manager.minSpeed, manager.maxSpeed);

                Vector3 direction = (averageCentre + averageAvoid) - transform.position;

                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), manager.rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
