using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishGroupManager : MonoBehaviour
{
    public static FishGroupManager instance = null; // Create a singleton for this

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
    public TextMeshProUGUI totalLifetimeAmount;
    public int lifetimeFishCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

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

            lifetimeFishCount++;

            allFish.Add(Instantiate(fishPrefab, spawnPosition, Quaternion.identity)); // Instantiate the fish at that position and add it to the array
        }
    }

    private void Update()
    {
        currentFishCountText.text = "Current First Order Consumer Count: " + allFish.Count.ToString();
        totalLifetimeAmount.text = "Total population since sim start: " + lifetimeFishCount.ToString();
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
            MeshGenerator.instance.xSize = 30;
            MeshGenerator.instance.zSize = 30;
            MeshGenerator.instance.MoveGround(new Vector3(-15, -3, -15));
            MeshGenerator.instance.CreateShape();
            MeshGenerator.instance.UpdateMesh();
        }

        else if (medium)
        {
            bounds = new Vector3(28, 2, 28);
            MeshGenerator.instance.xSize = 60;
            MeshGenerator.instance.zSize = 60;
            MeshGenerator.instance.MoveGround(new Vector3(-30, -3, -30));
            MeshGenerator.instance.CreateShape();
            MeshGenerator.instance.UpdateMesh();
        }

        else if (large)
        {
            bounds = new Vector3(42, 2, 42);
            MeshGenerator.instance.xSize = 90;
            MeshGenerator.instance.zSize = 90;
            MeshGenerator.instance.MoveGround(new Vector3(-45, -3, -45));
            MeshGenerator.instance.CreateShape();
            MeshGenerator.instance.UpdateMesh();
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
