using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFishScale : MonoBehaviour
{
    private void Start()
    {
        gameObject.transform.localScale = new Vector3(Random.Range(0.75f, 1.25f), Random.Range(0.75f, 1.25f), Random.Range(0.75f, 1.25f));
    }
}
