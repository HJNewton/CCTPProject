using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    // Functionality to add:
    // Enum states for roaming, eating and reproducing
    // Only apply rules when roaming; eating and reproducing should be seperate behaviour that potentially makes them vulnerable
    public enum FishState
    {
        Roaming,
        Avoiding,
        Feeding,
        ReadyToReproduce,
        Reproducing,
    }

    [Header("Fish Setup")]
    [SerializeField] private FishGroupManager manager;
    [SerializeField] private float speed;
    [SerializeField] bool turning;
    public float neighbourDistance; // Distance from nearest neighbour
    public float obstacleAvoidanceRange;
    public bool isDebugging = false;
    public GameObject fishDestinationTarget;
    public GameObject movingTarget;

    [Header("Fish States Setup")]
    public FishState currentFishState;
    [SerializeField] FishHealth fishHealth;

    [Header("Fish Reproduction Setup")]
    public bool canReproduce;
    public float reproductionCost;
    [SerializeField] float closestDistance;
    [SerializeField] float gestationPeriod;

    [Header("Fish Avoidance Setup")]
    public bool sharkInRange;

    SphereCollider sphereCollider;
    Vector3 direction;
    private int currentFrame;

    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>(); // Fetches the Fish Group Manager script from the Fish Manager object
        fishDestinationTarget = GameObject.FindGameObjectWithTag("Target");
        fishHealth = this.GetComponent<FishHealth>();

        sphereCollider = this.GetComponent<SphereCollider>();
        sphereCollider.radius = 125;

        currentFishState = FishState.Roaming;
    }

    private void Start()
    {
        speed = Random.Range(manager.minSpeed, manager.maxSpeed);
        movingTarget = fishDestinationTarget;

        GenerateNewGestationPeriod();
    }

    private void Update()
    {
        currentFrame++;

        if (fishDestinationTarget == null)
        {
            fishDestinationTarget = movingTarget;
        }

        Movement();
        UpdateState();
        //CheckSurroundings();
        Reproduction();

        if (currentFishState == FishState.Feeding)
        {
            Debug.Log("Hungry");
            Feeding();
        }
                
        if (currentFrame >= 15) // Applies boids rules every 15 frames as long as the fish isnt turning
        {
            if(!turning)
            {
                ApplyBoidsRules();
                currentFrame = 0;
            }
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Shark"))
    //    {
    //        sharkInRange = true;
    //        SharkAvoidance(other.gameObject);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Shark"))
    //    {
    //        sharkInRange = false;
    //    }
    //}

    void UpdateState()
    {
        // FISH FEEDING STATE SWITCH
        if (fishHealth.currentFoodAmount <= (fishHealth.initialFood / 100 * 66) // Hungry when it loses 1/3 of it's food
            /*&& time == FEEDING TIME*/
            /*&& not reproducing*/)
        {
            currentFishState = FishState.Feeding;
        }

        else if (fishHealth.currentFoodAmount > (fishHealth.initialFood / 100 * 66))
        {
            fishDestinationTarget = movingTarget;
        }

        // FISH REPRODUCTIVE STATE SWITCH
        if (fishHealth.currentFoodAmount >= (fishHealth.initialFood /100 * 50) &&
            currentFishState != FishState.Feeding &&
            canReproduce) // Add check to ensure it isn't only in a scene with female fish
        {
            currentFishState = FishState.ReadyToReproduce;
        }
    }
  
    void Movement()
    {
        // Generates a bounds that the fish should stay inside of
        Bounds bounds = new Bounds(manager.transform.position, manager.bounds * 2);

        RaycastHit hit;

        direction = fishDestinationTarget.transform.position - transform.position;

        if (!bounds.Contains(transform.position)) // Checks if fish is out of bounds
        {
            turning = true;
            direction = fishDestinationTarget.transform.position - transform.position; // Set direction of out of bounds fish to be towards target
        }

        else if (Physics.Raycast(transform.position, this.transform.forward, out hit, obstacleAvoidanceRange)) // Checks if fish is going to hit an object
        {
            if (hit.transform.gameObject.layer != LayerMask.GetMask("Ignore Raycast"))
            {
                turning = true;
                direction = Vector3.Reflect(this.transform.forward, hit.normal); // Deflects the fish away from the object it is going to hit by reflecting the angle in a "<" like fashion (incoming angle at the top, reflected towards the bottom for example)

                Debug.DrawRay(this.transform.position, this.transform.forward * obstacleAvoidanceRange, Color.red);
            }
        }

        //if (sharkInRange)
        //{
        //    turning = true;
        //}

        else
        {
            turning = false;
        }

        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), manager.rotationSpeed * Time.deltaTime); // A variation of the regular rotation behaviour that just turns towards the centre of the box
        }

        transform.Translate(0, 0, speed * Time.deltaTime); // Moves the fish
    }

    // Apply the rules for boids to each fish to enact the relevant behaviour
    void ApplyBoidsRules()
    {
        if (!turning) // Only applies the rules so long as the fish is not turning away from the outer wall
        {
            List<GameObject>fishList;
            fishList = manager.allFish;

            Vector3 averageCentre = Vector3.zero; // Average centre of the group calculated from each member of the group
            Vector3 averageAvoid = Vector3.zero; // Average avoidance vector of each member of the group
            float globalSpeed = 0.01f; // Average group speed
            int localGroupSize = 0; // Size of the local group of this particular fish

            foreach (GameObject fish in fishList)
            {
                if (fish != this.gameObject)
                {
                    neighbourDistance = Vector3.Distance(fish.transform.position + (transform.localScale / 2), this.transform.position);

                    if (neighbourDistance <= manager.distanceToNeighbours)
                    {
                        averageCentre += fish.transform.position;
                        localGroupSize++;

                        if (neighbourDistance < 1.5f) // How close each fish should be before they should begin avoiding
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
                averageCentre = averageCentre / localGroupSize + (/*manager.*/fishDestinationTarget.transform.position - this.transform.position); // Gets the centre of the local group and moves it towards the target
                speed = globalSpeed / localGroupSize; // Matches fish speed with global speed
                // speed = Random.Range(manager.minSpeed, manager.maxSpeed);

                Vector3 direction = (averageCentre + averageAvoid) - transform.position;

                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), manager.rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    //void CheckSurroundings()
    //{
    //    Collider[] overlappedObjects = Physics.OverlapSphere(transform.position, manager.awarenessRange);

    //    foreach (Collider overlappedObject in overlappedObjects)
    //    {
    //        if (overlappedObject.CompareTag("Shark"))
    //        {
    //            sharkInRange = true;
    //        }
    //    }
    //}

    //void SharkAvoidance(GameObject sharkToAvoid)
    //{
    //    if (sharkInRange)
    //    {
    //        turning = true;
    //        direction = Vector3.Reflect(this.transform.forward, sharkToAvoid.transform.position.normalized); // Deflects the fish away from the object it is going to hit by reflecting the angle in a "<" like fashion (incoming angle at the top, reflected towards the bottom for example)
    //    }
    //}

    void Feeding()
    {
        GameObject[] kelpInScene;
        kelpInScene = GameObject.FindGameObjectsWithTag("Kelp"); // Populate the list with all kelp in the scene
        
        float distance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject kelp in kelpInScene)
        {
            Vector3 difference = kelp.transform.position - currentPos;

            float curDistance = difference.sqrMagnitude;

            if(curDistance < distance) // If the kelp is closer than the last kelp then that is the new target kelp
            {
                fishDestinationTarget = kelp;
            }
        }
    }

    void Reproduction()
    {
        float tempDistance;

        Collider[] overlappedObjects = Physics.OverlapSphere(transform.position, manager.awarenessRange);
        
        foreach (Collider overlappedObject in overlappedObjects)
        {
            if (overlappedObject.CompareTag("Fish"))
            {
                tempDistance = Vector3.Distance(transform.position, overlappedObject.transform.position); // Check distance for each fish between themself and the current fish

                if (closestDistance == 0) // Ensures that closest distance has an initial value otherwise it can never be compared
                {
                    closestDistance = tempDistance;
                }

                if (tempDistance <= closestDistance) // Check if the new fish that is checked is closer than the most recent closest fish
                {
                    closestDistance = tempDistance;
                }
            }
        }

        if (closestDistance <= manager.awarenessRange && currentFishState == FishState.ReadyToReproduce && canReproduce) // Check if the fish has another fish nearby and this fish is ready to reproduce ADD CHECKS FOR FOOD AMOUNTS
        {
            currentFishState = FishState.Reproducing; // Change state to reproducing
        }

        if (currentFishState == FishState.Reproducing)
        {
            gestationPeriod -= Time.deltaTime; // Reduces the time to spawn another fish

            if (gestationPeriod <= 0)
            {
                manager.allFish.Add(Instantiate(manager.fishPrefab, transform.position, Quaternion.identity)); // Instantiate the fish at that position and add it to the listy 

                fishHealth.ModifyFood(reproductionCost); // How much food is removed upon reproduciton
                fishHealth.timesReproduced++;

                canReproduce = false;

                GenerateNewGestationPeriod();
                currentFishState = FishState.Roaming; // Change state back to roaming
            }
        }
    }

    float GenerateNewGestationPeriod()
    {
        gestationPeriod = Random.Range(5, 15); // Get reproductive timer on this fish

        return gestationPeriod;
    }

    private void OnDestroy()
    {
        manager.allFish.Remove(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, manager.awarenessRange);
    }
}
