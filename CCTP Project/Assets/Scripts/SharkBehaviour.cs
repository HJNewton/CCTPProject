﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkBehaviour : MonoBehaviour
{
    // TODO: 
    // Give shark it's own manager properties rather than using fishmanager stuff
    // Rewrite comments on this script to be relevant

    [Header("Shark Setup")]
    [SerializeField] private SharkGroupManager sharkManager;
    [SerializeField] private FishGroupManager fishManager;
    [SerializeField] private float speed;
    [SerializeField] bool turning;
    [SerializeField] GameObject target;
    public GameObject centre;
    public float eatRadius;
    public LayerMask sharkLayer;
    
    [SerializeField] float outlierDistance;

    private void Start()
    {
        sharkManager = GameObject.FindGameObjectWithTag("SharkManager").GetComponent<SharkGroupManager>(); // Fetches the Fish Group Manager script from the Fish Manager object
        fishManager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>(); // Fetches the Fish Group Manager script from the Fish Manager object
        speed = Random.Range(sharkManager.minSpeed, sharkManager.maxSpeed);

        Invoke("UpdateTarget", 0);
        //InvokeRepeating("UpdateTarget", 0, 5f);
    }

    private void Update()
    {
        Movement();

        if (target == null)
        {
            UpdateTarget();
        }

        Collider[] overlappedObjects = Physics.OverlapSphere(centre.transform.position, eatRadius);
        foreach (Collider overlappedObject in overlappedObjects)
        {
            if (overlappedObject.gameObject == target.gameObject) // Check if the overlapped object is the current target fish
            {
                Debug.Log("Yum");

                Destroy(overlappedObject.gameObject);

                UpdateTarget();
            }
        }
    }

    void Movement()
    {
        if (target == null)
        {
            UpdateTarget();
        }

        // Generates a bounds that the fish should stay inside of
        Bounds bounds = new Bounds(sharkManager.transform.position, fishManager.bounds * 2);

        RaycastHit hit;
        Vector3 direction = target.transform.position - transform.position;

        if (!bounds.Contains(transform.position)) // Checks if fish is out of bounds
        {
            turning = true;
            direction = target.transform.position - transform.position; // Set direction of out of bounds fish to be towards target
        }

        else if (Physics.Raycast(transform.position, this.transform.forward * 2, out hit, sharkLayer)) // Checks if fish is going to hit an object
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal); //Deflects the fish away from the object it is going to hit by reflecting the angle in a "<" like fashion (incoming angle at the top, reflected towards the bottom for example)

            Debug.DrawRay(this.transform.position, this.transform.forward * 2, Color.red);
        }

        else
        {
            turning = false;
        }

        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), sharkManager.rotationSpeed * Time.deltaTime); // A variation of the regular rotation behaviour that just turns towards the centre of the box
        }

        if(!turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), sharkManager.rotationSpeed * Time.deltaTime);
        }

        transform.Translate(0, 0, speed * Time.deltaTime); // Moves the fish
    }

    void UpdateTarget()
    {
        outlierDistance = 0;

        Debug.Log("Updating");
        
        List<GameObject> fishArray;
        fishArray = fishManager.allFish;

        foreach (GameObject fish in fishArray)
        {
            if (fish)
            {
                if (outlierDistance <= fish.GetComponent<FishBehaviour>().neighbourDistance)
                {
                    outlierDistance = fish.GetComponent<FishBehaviour>().neighbourDistance;
                    target = fish.gameObject;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(centre.transform.position, eatRadius);        
    }
}