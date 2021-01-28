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
    [SerializeField] float closestDistance;
    [SerializeField] float gestationPeriod;


    Vector3 direction;


    private void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>(); // Fetches the Fish Group Manager script from the Fish Manager object
        fishDestinationTarget = GameObject.FindGameObjectWithTag("Target");
        fishHealth = this.GetComponent<FishHealth>();

        currentFishState = FishState.Roaming;

        Debug.Log("Spawned");
    }

    private void Start()
    {
        speed = Random.Range(manager.minSpeed, manager.maxSpeed);
        movingTarget = fishDestinationTarget;

        GenerateNewGestationPeriod();
        
        //InvokeRepeating("ApplyBoidsRules", 0, 0.05f);
    }

    private void Update()
    {
        if (fishDestinationTarget == null)
        {
            fishDestinationTarget = movingTarget;
        }

        Movement();
        UpdateState();
        CheckSurroundings();
        Reproduction();

        if (currentFishState == FishState.Feeding)
        {
            Feeding();
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            currentFishState = FishState.ReadyToReproduce;
        }

        if (!turning /*&& (currentFishState == FishState.Roaming || currentFishState == FishState.ReadyToReproduce || currentFishState == FishState.Feeding)*/) // Only apply the boids rules if the fish is roaming
        {
            if (Random.Range(0, 100) < 10)
            {
                speed = Random.Range(manager.minSpeed, manager.maxSpeed);
            }

            if (Random.Range(0, 100) < 20) // Chance to apply boids rules
            {
                ApplyBoidsRules();
            }
        }
    }

    void UpdateState()
    {
        // FISH FEEDING STATE SWITCH
        if (fishHealth.currentFoodAmount <= (fishHealth.initialFood / 100 * 66) 
            /*&& time == FEEDING TIME*/
            /*&& not reproducing*/)
        {
            currentFishState = FishState.Feeding;
        }

        else if (fishHealth.currentFoodAmount > (fishHealth.initialFood / 100 * 66))
        {
            fishDestinationTarget = movingTarget;
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
            List<GameObject>fishArray;
            fishArray = manager.allFish;

            Vector3 averageCentre = Vector3.zero; // Average centre of the group calculated from each member of the group
            Vector3 averageAvoid = Vector3.zero; // Average avoidance vector of each member of the group
            float globalSpeed = 0.01f; // Average group speed
            int localGroupSize = 0; // Size of the local group of this particular fish

            foreach (GameObject fish in fishArray)
            {
                if (fish != this.gameObject)
                {
                    neighbourDistance = Vector3.Distance(fish.transform.position, this.transform.position);

                    if (neighbourDistance <= manager.distanceToNeighbours)
                    {
                        averageCentre += fish.transform.position;
                        localGroupSize++;

                        if (neighbourDistance < 1.0f) // How close each fish should be before they should begin avoiding
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

    void CheckSurroundings()
    {
        Collider[] overlappedObjects = Physics.OverlapSphere(transform.position, manager.awarenessRange);

        foreach (Collider overlappedObject in overlappedObjects)
        {
            if (overlappedObject.CompareTag("Shark"))
            {
                SharkAvoidance(); // Do shark avoidance behaviour
            }
        }
    }

    void SharkAvoidance()
    {

    }

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

        if (closestDistance <= manager.awarenessRange && currentFishState == FishState.ReadyToReproduce) // Check if the fish has another fish nearby and this fish is ready to reproduce ADD CHECKS FOR FOOD AMOUNTS
        {
            currentFishState = FishState.Reproducing; // Change state to reproducing
        }

        if (currentFishState == FishState.Reproducing)
        {
            gestationPeriod -= Time.deltaTime; // Reduces the time to spawn another fish

            if (gestationPeriod <= 0)
            {
                manager.allFish.Add(Instantiate(manager.fishPrefab, manager.GenerateNewSpawnPosition(), Quaternion.identity)); // Instantiate the fish at that position and add it to the listy 
                Debug.Log("Reproduced");

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
