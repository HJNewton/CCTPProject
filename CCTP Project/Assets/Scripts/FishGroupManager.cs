using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGroupManager : MonoBehaviour
{
    [Header("Fish Spawning Setup")]
    public GameObject fishPrefab; // Fish prefab
    [Range(50.0f, 200.0f)]
    public int fishCount; // Number of fish to spawn
    public List<GameObject> allFish = new List<GameObject>(); // A list of all fish spawned into the scene
    public Vector3 bounds; // The limtis of the area fish can swim in (AUTO ADJUST TO TERRAIN SIZE LATER)
    public Vector3 spawnPosition;

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

    private void Start()
    {
        //allFish = new GameObject[fishCount]; // Sets the array equal to desired number of fish
        fishDestinationTarget = GameObject.FindGameObjectWithTag("Target");

        for (int i = 0; i < fishCount; i++)
        {
            spawnPosition = new Vector3(Random.Range(-bounds.x, bounds.x), // Generate a random position in the set bounds
                                        Random.Range(-bounds.y, bounds.y),
                                        Random.Range(-bounds.z, bounds.z));

            allFish.Add(Instantiate(fishPrefab, spawnPosition, Quaternion.identity)); // Instantiate the fish at that position and add it to the array
        }
    }

    public Vector3 GenerateNewSpawnPosition()
    {
        spawnPosition = new Vector3(Random.Range(-bounds.x, bounds.x), // Generate a random position in the set bounds
                                    Random.Range(-bounds.y, bounds.y),
                                    Random.Range(-bounds.z, bounds.z));

        return spawnPosition;
    }
}
