using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager instance = null; // Create a singleton for this mesh generator

    [Header("Environment Elements Setup")]
    public GameObject[] obstaclePrefabs;
    public int obstacleCount;
    public List<GameObject> allObstacles = new List<GameObject>(); // An array of all fish spawned into the scene
    public GameObject kelpPrefab;
    public int kelpCount;
    public List<GameObject> allKelp = new List<GameObject>(); // A list of all kelp in the scene
    private float yPos;

    [Header("Kelp Respawning")]
    public bool slow;
    public bool medium;
    public bool fast;
    [SerializeField] private float timeBetweenSpawns;
    public TextMeshProUGUI currentKelpCount;
    public LayerMask layerMask;

    Vector3 spawnPosition;

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

        GenerateNewTimeToSpawn(); // Adds an initial value for the kelp respawn timer to use       
    }

    private void Start()
    {
        if (!slow && !fast)
        {
            medium = true;
        }
        
        fishManager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>();

        Invoke("SpawnObjects", 0f);
    }

    void SpawnObjects()
    {
        // Randomly spawn a set amount of obstacles with a set size
        for (int i = 0; i < obstacleCount; i++)
        {
            SpawnPosition();

            Vector3 obstacleScale = new Vector3(Random.Range(0.75f, 1.5f),
                                                Random.Range(0.75f, 1.5f),
                                                Random.Range(0.75f, 1.5f));

            int prefabToSpawn = Random.Range(0, obstaclePrefabs.Length);

            allObstacles.Add(Instantiate(obstaclePrefabs[prefabToSpawn], spawnPosition, Quaternion.identity));
            allObstacles[i].transform.localScale = obstacleScale;

            RaycastHit hit;

            if (Physics.Raycast(allObstacles[i].transform.position, -allObstacles[i].transform.up, out hit, Mathf.Infinity, layerMask))
            {
                yPos = hit.point.y;
            }

            allObstacles[i].transform.position = new Vector3(allObstacles[i].transform.position.x, yPos, allObstacles[i].transform.position.z);
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
                                                 10f,
                                                 Random.Range(-fishManager.bounds.z, fishManager.bounds.z));
        

        return spawnPosition;
    }

    public void DropdownValueHandle(TMP_Dropdown val)
    {
        if (val.value == 0)
        {
            slow = true;
            medium = false;
            fast = false;
        }

        if (val.value == 1)
        {
            slow = false;
            medium = true;
            fast = false;
        }

        if (val.value == 2)
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
