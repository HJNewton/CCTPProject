﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Environment Elements Setup")]
    public GameObject coralPrefab;
    public int coralCount;
    //public GameObject[] allCoral;
    public List<GameObject> allCoral = new List<GameObject>(); // An array of all fish spawned into the scene
    public GameObject kelpPrefab;
    public int kelpCount;
    public List<GameObject> allKelp = new List<GameObject>(); // A list of all kelp in the scene

    [Header("Kelp Respawning")]
    public bool slow;
    public bool medium;
    public bool fast;
    [SerializeField] private float timeBetweenSpawns;
    public TextMeshProUGUI currentKelpCount;

    Vector3 spawnPosition;

    FishGroupManager fishManager;

    private void Awake()
    {
        GenerateNewTimeToSpawn(); // Adds an initial value for the kelp respawn timer to use
    }

    private void Start()
    {
        if (!medium && !fast)
        {
            slow = true;
        }
        
        fishManager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>();
        
        // Randomly spawn a set amount of coral with a set size, this is essentially going to be the obstacle fish are forced to avoid
        for (int i = 0; i < coralCount; i++)
        {
            Vector3 newSpawnPos = new Vector3(Random.Range(-fishManager.bounds.x, fishManager.bounds.x),
                                                -1.5f,
                                                Random.Range(-fishManager.bounds.z, fishManager.bounds.z));
            //SpawnPosition();

            Vector3 coralScale = new Vector3(Random.Range(0.75f, 1.5f), 
                                             Random.Range(0.75f, 1.5f), 
                                             Random.Range(0.75f, 1.5f));

            allCoral.Add(Instantiate(coralPrefab, newSpawnPos, Quaternion.identity));
            allCoral[i].transform.localScale = coralScale;
        }

        // Randomly spawn a set amount of kelp with a set size, this is essentially going to be the food for smaller fish to consume
        for (int i = 0; i < kelpCount; i++)
        {
            SpawnPosition();

            allKelp.Add(Instantiate(kelpPrefab, spawnPosition, Quaternion.identity));
        }
    }

    private void Update()
    {
        currentKelpCount.text = "Current Kelp Count: " + allKelp.Count.ToString();
        
        KelpRespawnTimer();
    }

    // Reduce time until next kelp is spawned
    void KelpRespawnTimer() 
    {
        timeBetweenSpawns -= Time.deltaTime;

        if (timeBetweenSpawns <= 0)
        {
            InstantiateKelp();
            
            GenerateNewTimeToSpawn();
        }
    }

    void InstantiateKelp()
    {
        SpawnPosition();

        allKelp.Add(Instantiate(kelpPrefab, spawnPosition, Quaternion.identity));
    }
    
    // Generates a new spawn position on each call, currently only applicable to kelp due to the Y value being different
    // TODO: Make it so the bottom of each object automatically is placed on the nearest piece of floor below it
    //          - This would also allow for more complex terrain via the use of perlin noise.
    Vector3 SpawnPosition()
    {
        spawnPosition = new Vector3(Random.Range(-fishManager.bounds.x, fishManager.bounds.x),
                                                -2.1f,
                                                Random.Range(-fishManager.bounds.z, fishManager.bounds.z));

        return spawnPosition;
    }

    public void DropdownValueHandle(int val)
    {
        if (val == 0)
        {
            slow = true;
            medium = false;
            fast = false;
        }

        if (val == 1)
        {
            slow = false;
            medium = true;
            fast = false;
        }

        if (val == 2)
        {
            slow = false;
            medium = false;
            fast = true;
        }
    }

    float GenerateNewTimeToSpawn()
    {
        if (slow)
        {
            timeBetweenSpawns = Random.Range(10, 15);
        }

        if (medium)
        {
            timeBetweenSpawns = Random.Range(5, 10);
        }

        if (fast)
        {
            timeBetweenSpawns = Random.Range(3, 5);
        }

        return timeBetweenSpawns;
    }
}
