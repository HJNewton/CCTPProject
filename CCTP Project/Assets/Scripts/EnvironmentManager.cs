using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Environment Elements Setup")]
    public GameObject coralPrefab;
    public int coralCount;
    public GameObject[] allCoral;
    public GameObject kelpPrefab;
    public int kelpCount;
    public GameObject[] allKelp;

    [Header("Kelp Respawning")]
    public float timeBetweenSpawnsLowerRange;
    public float timeBetweenSpawnsUpperRange;
    [SerializeField] private float timeBetweenSpawns;

    FishGroupManager fishManager;

    private void Awake()
    {
        RespawnKelpValueGenerator(); // Adds an initial value for the kelp respawn timer to use
    }

    private void Start()
    {
        fishManager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>();

        allCoral = new GameObject[coralCount];

        // Randomly spawn a set amount of coral with a set size, this is essentially going to be the obstacle fish are forced to avoid
        for (int i = 0; i < coralCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-fishManager.bounds.x, fishManager.bounds.x),
                                                -1.5f,
                                                Random.Range(-fishManager.bounds.z, fishManager.bounds.z));

            Vector3 coralScale = new Vector3(Random.Range(0.75f, 1.5f), 
                                             Random.Range(0.75f, 1.5f), 
                                             Random.Range(0.75f, 1.5f));

            allCoral[i] = Instantiate(coralPrefab, spawnPosition, Quaternion.identity);
            allCoral[i].transform.localScale = coralScale;

        }

        allKelp = new GameObject[kelpCount]; 

        // Randomly spawn a set amount of kelp with a set size, this is essentially going to be the food for smaller fish to consume
        for (int i = 0; i < kelpCount; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-fishManager.bounds.x, fishManager.bounds.x),
                                                -2.1f,
                                                Random.Range(-fishManager.bounds.z, fishManager.bounds.z));
            

            allKelp[i] = Instantiate(kelpPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void Update()
    {
        KelpRespawnTimer();
    }

    void KelpRespawnTimer() // Reduce time until next kelp is spawned
    {
        timeBetweenSpawns -= Time.deltaTime;

        if (timeBetweenSpawns <= 0)
        {
            // ADD INSTANTIATION
            
            RespawnKelpValueGenerator();
        }
    }

    void InstantiateKelp()
    {

    }

    float RespawnKelpValueGenerator()
    {
        timeBetweenSpawns = Random.Range(timeBetweenSpawnsLowerRange, timeBetweenSpawnsUpperRange);

        return timeBetweenSpawns;
    }

    // Functionality to add:
    // Kelp respawning as it is destroyed, determined by a food replenishment rate
    // Coral has physical properties that the fish must avoid
}
