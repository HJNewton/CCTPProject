using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishGroupManager : MonoBehaviour
{
    [Header("Fish Spawning Setup")]
    public GameObject fishPrefab; // Fish prefab
    [Range(50.0f, 200.0f)]
    public int fishCount; // Number of fish to spawn
    public List<GameObject> allFish = new List<GameObject>(); // A list of all fish spawned into the scene
    public Vector3 bounds; // The limtis of the area fish can swim in (AUTO ADJUST TO TERRAIN SIZE LATER)
    public Vector3 spawnPosition;
    public GameObject groundPlane;
    public bool small;
    public bool medium;
    public bool large;

    [Header("Fish Behaviour Setup")] // MAKE ALL OF THIS STUFF APPEAR IN UI
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(0.0f, 10.0f)]
    public float distanceToNeighbours; // Gets the maximum distance in which each fish can find a new neighbour
    [Range(0.0f, 10.0f)]
    public float rotationSpeed;
    [Range(0.0f, 10.0f)]
    public float awarenessRange;
    public GameObject fishDestinationTarget;

    [Header("Fish UI")]
    public TextMeshProUGUI currentFishCountText;

    private void Awake()
    {
        if (!small && !large)
        {
            medium = true;
        }

        ChangeBoundsAndSizes();
    }

    private void Start()
    {
        fishDestinationTarget = GameObject.FindGameObjectWithTag("Target");

        for (int i = 0; i < fishCount; i++)
        {
            spawnPosition = new Vector3(Random.Range(-bounds.x, bounds.x), // Generate a random position in the set bounds
                                        Random.Range(-bounds.y, bounds.y),
                                        Random.Range(-bounds.z, bounds.z));

            allFish.Add(Instantiate(fishPrefab, spawnPosition, Quaternion.identity)); // Instantiate the fish at that position and add it to the array
        }
    }

    private void Update()
    {
        currentFishCountText.text = "Current Fish Count: " + allFish.Count.ToString();
    }

    public Vector3 GenerateNewSpawnPosition()
    {
        spawnPosition = new Vector3(Random.Range(-bounds.x, bounds.x), // Generate a random position in the set bounds
                                    Random.Range(-bounds.y, bounds.y),
                                    Random.Range(-bounds.z, bounds.z));

        return spawnPosition;
    }
    
    private void ChangeBoundsAndSizes()
    {
        if (small)
        {
            bounds = new Vector3(14, 2, 14);
            groundPlane.transform.localScale = new Vector3(3, 1, 3);
        }

        else if (medium)
        {
            bounds = new Vector3(28, 2, 28);
            groundPlane.transform.localScale = new Vector3(6, 1, 6);
        }

        else if (large)
        {
            bounds = new Vector3(42, 2, 42);
            groundPlane.transform.localScale = new Vector3(9, 1, 9);
        }
    }

    public void DropdownValueHandle(TMP_Dropdown val)
    {
        if (val.value == 0)
        {
            small = true;
            medium = false;
            large = false;
        }

        if (val.value == 1)
        {
            small = false;
            medium = true;
            large = false;
        }

        if (val.value == 2)
        {
            small = false;
            medium = false;
            large = true;
        }
    }
}
