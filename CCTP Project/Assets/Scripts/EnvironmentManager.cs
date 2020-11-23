using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Environment Elements Settings")]
    public GameObject coralPrefab;
    public int coralCount;
    public GameObject[] allCoral;
    public GameObject kelpPrefab;
    public int kelpCount;
    public GameObject[] allKelp;


    FishGroupManager fishManager;

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
                                                -1.0f,
                                                Random.Range(-fishManager.bounds.z, fishManager.bounds.z));

            Vector3 kelpScale = new Vector3(Random.Range(0.75f, 1.5f),
                                            Random.Range(0.75f, 1.5f),
                                            Random.Range(0.75f, 1.5f));

            allKelp[i] = Instantiate(kelpPrefab, spawnPosition, Quaternion.identity);
            allKelp[i].transform.localScale = kelpScale;

        }
    }

    // Functionality to add:
    // Kelp respawning as it is destroyed, determined by a food replenishment rate
    // Coral has physical properties that the fish must avoid
}
