using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kelp : MonoBehaviour
{
    [Header("Kelp Setup")]
    public float triggerRange;
    public float foodToGive;
    public bool canBeEaten;

    private void Awake()
    {
        this.GetComponent<SphereCollider>().radius = triggerRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fish") && canBeEaten)
        {
            Debug.Log("Fish In Range");

            other.GetComponent<FishHealth>().AddFood(foodToGive);
            Destroy(gameObject);
        }
    }
}
