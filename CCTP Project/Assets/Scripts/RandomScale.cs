using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScale : MonoBehaviour
{
    public Vector3 scaleLowerBounds;
    public Vector3 scaleUpperBounds;
    public Vector3 initialScale;
    [SerializeField] private Vector3 targetScale; 
    public float growthSpeed;
    private bool isGrowing;

    private void Start()
    {
        gameObject.transform.localScale = initialScale;

        targetScale = new Vector3(Random.Range(scaleLowerBounds.x, scaleUpperBounds.x), 
                                  Random.Range(scaleLowerBounds.y, scaleUpperBounds.y), 
                                  Random.Range(scaleLowerBounds.z, scaleUpperBounds.z));

        isGrowing = true;
    }

    private void Update()
    {
        if (isGrowing)
        {
            Grow();
        }
    }

    private void Grow()
    {
        transform.localScale += Vector3.one * Time.deltaTime * growthSpeed;

        if (transform.localScale.x >= targetScale.x &&
            transform.localScale.y >= targetScale.y &&
            transform.localScale.z >= targetScale.z)
        {
            isGrowing = false;
        }
    }
}
