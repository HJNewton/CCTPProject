﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SharkGroupManager : MonoBehaviour
{
    public static SharkGroupManager instance = null; // Create a singleton for this


    [Header("Shark Spawning Setup")]
    public GameObject sharkPrefab; // Fish prefab
    [Range(0.0f, 10.0f)]
    public int sharkCount; // Number of fish to spawn
    public float spawnDelay; // Delay before shark spawn
    public List<GameObject> allSharks = new List<GameObject>(); // An array of all sharks spawned into the scene

    [Header("Shark Behaviour Setup")] // MAKE ALL OF THIS STUFF APPEAR IN UI
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(0.0f, 10.0f)]
    public float eatRadius; // Gets the maximum distance in which each shark can 
    [Range(0.0f, 10.0f)]
    public float rotationSpeed;

    [Header("Shark UI")]
    public TextMeshProUGUI currentSharkCountText;
    public TextMeshProUGUI totalLifetimeAmount;
    public int lifetimeSharkCount;

    FishGroupManager fishManager;

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
    }

    private void Start()
    {
        fishManager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>();

        StartCoroutine("SpawnSharks");
    }

    IEnumerator SpawnSharks()
    {
        yield return new WaitForSeconds(spawnDelay);

        for (int i = 0; i < sharkCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-fishManager.bounds.x, fishManager.bounds.x), // Generate a random position in the set bounds
                                                Random.Range(-fishManager.bounds.y, fishManager.bounds.y),
                                                Random.Range(-fishManager.bounds.z, fishManager.bounds.z));

            lifetimeSharkCount++;
            allSharks.Add(Instantiate(sharkPrefab, spawnPosition, Quaternion.identity)); // Instantiate the fish at that position and add it to the array
        }
    }

    private void Update()
    {
        currentSharkCountText.text = "Current Top Predator Count: " + allSharks.Count.ToString();
        totalLifetimeAmount.text = "Total population since sim start:  " + lifetimeSharkCount.ToString();
    }
}
