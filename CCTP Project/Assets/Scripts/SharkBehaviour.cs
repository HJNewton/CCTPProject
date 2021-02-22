using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBehaviour : MonoBehaviour
{
    // TODO: 
    // Rewrite comments on this script to be relevant
    public enum SharkState
    {
        Roaming,
        Avoiding,
        Feeding,
        ReadyToReproduce,
        Reproducing,
    }

    [Header("Shark Setup")]
    [SerializeField] private SharkGroupManager sharkManager;
    [SerializeField] private FishGroupManager fishManager;
    [SerializeField] private float speed;
    [SerializeField] bool turning;
    [SerializeField] GameObject target;
    public GameObject centre;
    public GameObject sharkMovingTargetPrefab;
    [SerializeField] private GameObject sharkDestinationTarget;
    public float eatRadius;
    public float obstacleAvoidanceRange;
    [SerializeField] float outlierDistance;
    
    [Header("Shark States Setup")]
    [SerializeField] private SharkState currentSharkState;
    [SerializeField] private SharkHealth sharkHealth;

    [Header("Shark Reproduction Setup")]
    public bool canReproduce;
    public float reproductionCost;
    [SerializeField] float closestDistance;
    [SerializeField] float gestationPeriod;

    Vector3 randomNewPosition;

    private void Awake()
    {
        currentSharkState = SharkState.Roaming;
        sharkHealth = this.GetComponent<SharkHealth>();
    }

    private void Start()
    {
        sharkManager = GameObject.FindGameObjectWithTag("SharkManager").GetComponent<SharkGroupManager>(); // Fetches the Shark Group Manager script from the Shark Manager object
        fishManager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>(); // Fetches the Fish Group Manager script from the Fish Manager object
        speed = Random.Range(sharkManager.minSpeed, sharkManager.maxSpeed);
        sharkDestinationTarget = Instantiate(sharkMovingTargetPrefab, transform.position, transform.rotation);
        sharkDestinationTarget.transform.position = PickRandomPoint();

        //Invoke("UpdateTarget", 0); // Gets a target on start
        //InvokeRepeating("UpdateTarget", 0, 5f);
    }

    private void Update()
    {
        Movement();
        UpdateState();
        
        if (currentSharkState == SharkState.Feeding)
        {
            Feeding();
        }

        else if (currentSharkState == SharkState.Roaming)
        {
            target = sharkDestinationTarget;
        }
    }

    void Feeding()
    {
        Collider[] overlappedObjects = Physics.OverlapSphere(centre.transform.position, eatRadius);
        foreach (Collider overlappedObject in overlappedObjects)
        {
            if (overlappedObject.gameObject == target.gameObject) // Check if the overlapped object is the current target fish
            {
                sharkHealth.currentFoodAmount += (overlappedObject.GetComponent<FishHealth>().currentFoodAmount / 100 * 10); // Gives the shark 10% of the "biomass" of the fish it eats

                Destroy(overlappedObject.gameObject);

                UpdateTargetToFeedOn();
            }
        }
    }

    void UpdateState()
    {
        // SHARK FEEDING STATE SWITCH
        if (sharkHealth.currentFoodAmount <= (sharkHealth.initialFood / 100 * 66)) // Hungry when it loses 1/3 of it's food
        {
            currentSharkState = SharkState.Feeding;
            UpdateTargetToFeedOn();
        }

        else if (sharkHealth.currentFoodAmount > (sharkHealth.initialFood / 100 * 80)) // Shark will stop eating when it is at 80% of its original food, higher than 66% to provide a buffer
        {
            target = sharkDestinationTarget;
            currentSharkState = SharkState.Roaming;
        }

        // SHARK REPRODUCTIVE STATE SWITCH
        if (sharkHealth.currentFoodAmount >= (sharkHealth.initialFood / 100 * 50) &&
            currentSharkState != SharkState.Feeding &&
            canReproduce) // Add check to ensure it isn't only in a scene with female sharks
        {
            currentSharkState = SharkState.ReadyToReproduce;
        }
    }

    void Movement()
    {
        if (target == null)
        {
            UpdateTargetToFeedOn();
        }

        // Generates a bounds that the shark should stay inside of
        Bounds bounds = new Bounds(sharkManager.transform.position, fishManager.bounds * 2);

        RaycastHit hit;

        if (target != null || target != sharkDestinationTarget)
        {
            Vector3 direction = target.transform.position - transform.position;


            if (!bounds.Contains(transform.position)) // Checks if shark is out of bounds
            {
                turning = true;
                direction = target.transform.position - transform.position; // Set direction of out of bounds shark to be towards target
            }

            else if (Physics.Raycast(transform.position, this.transform.forward, out hit, obstacleAvoidanceRange) /*|| 
                 Physics.Raycast(transform.position, -this.transform.up, out hit, obstacleAvoidanceRange, sharkLayer)*/) // Checks if shark is going to hit an object
            {
                turning = true;
                direction = Vector3.Reflect(this.transform.forward, hit.normal); //Deflects the shark away from the object it is going to hit by reflecting the angle in a "<" like fashion (incoming angle at the top, reflected towards the bottom for example)

                Debug.DrawRay(this.transform.position, this.transform.forward * obstacleAvoidanceRange, Color.red);
            }


            else
            {
                turning = false;
            }

            if (turning)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), sharkManager.rotationSpeed * Time.deltaTime); // A variation of the regular rotation behaviour that just turns towards the centre of the box
            }

            if (!turning)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), sharkManager.rotationSpeed * Time.deltaTime);
            }
        }

        transform.Translate(0, 0, speed * Time.deltaTime); // Moves the shark
    }

    Vector3 PickRandomPoint()
    {
        return randomNewPosition = new Vector3(Random.Range(-fishManager.bounds.x, fishManager.bounds.x),
                                               Random.Range(-fishManager.bounds.y, fishManager.bounds.y),
                                               Random.Range(-fishManager.bounds.z, fishManager.bounds.z));
    
    
    }

    void UpdateTargetToFeedOn()
    {
        outlierDistance = 0;
        
        List<GameObject> fishList;
        fishList = fishManager.allFish;

        foreach (GameObject fish in fishList)
        {
            if (fish)
            {
                if (outlierDistance <= fish.GetComponent<FishBehaviour>().neighbourDistance)
                {
                    outlierDistance = fish.GetComponent<FishBehaviour>().neighbourDistance;
                    target = fish.gameObject;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(centre.transform.position, eatRadius);        
    }
}
