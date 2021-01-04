using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kelp : MonoBehaviour
{
    public float triggerRange;
    public float foodToGive;

    private void Start()
    {
        this.GetComponent<SphereCollider>().radius = triggerRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Fish"))
        {
            Debug.Log("Fish In Range");

            other.GetComponent<FishHealth>().AddFood(foodToGive);
        }
    }
}
