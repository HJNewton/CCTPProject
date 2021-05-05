using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    [Header("Movement Setup")]
    public float moveSpeed;

    Vector3 newPosition;

    FishGroupManager fishManager;

    private void Start()
    {
        fishManager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>();

        GenerateNewPositon();
    }

    private void Update()
    {
        RelocateTarget();
    }

    // Generates a new positon for the target to move towards utilising the bounds
    void GenerateNewPositon()
    {
        newPosition = new Vector3(Random.Range(-fishManager.bounds.x, fishManager.bounds.x),
                                         Random.Range(-fishManager.bounds.y, fishManager.bounds.y),
                                         Random.Range(-fishManager.bounds.z, fishManager.bounds.z));
    }

    void RelocateTarget()
    {
        // Moves target towards newPosition when it is not at that position
        if (transform.position != newPosition) 
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
        }

        // Generates a new position when the target reaches newPosition
        else if (transform.position == newPosition)
        {
            GenerateNewPositon();
        }
    }
}
