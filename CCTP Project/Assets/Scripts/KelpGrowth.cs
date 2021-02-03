using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpGrowth : MonoBehaviour
{
    [Header("Growth Setup")]
    public bool isGrowing;
    public float growingSpeed;
    [SerializeField] Vector3 targetScale;

    Kelp kelp;
    EnvironmentManager manager;

    private void Awake()
    {
        kelp = gameObject.GetComponentInChildren<Kelp>();
        manager = GameObject.FindGameObjectWithTag("EnvironmentManager").GetComponent<EnvironmentManager>();
    }

    private void Start()
    {
        isGrowing = true;

        targetScale = new Vector3(Random.Range(0.75f, 1.5f), // Set a target scale for the kelp to grow to
                                  Random.Range(0.75f, 1.5f),
                                  Random.Range(0.75f, 1.5f));

        SetInitialScale();
    }

    private void Update()
    {
        Growing();
    }

    void SetInitialScale()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // Set the initial scale
    }

    void Growing()
    {
        if (isGrowing)
        {
            transform.localScale += Vector3.one * Time.deltaTime * growingSpeed;

            if (transform.localScale.x >= targetScale.x &&
                transform.localScale.y >= targetScale.y &&
                transform.localScale.z >= targetScale.z)
            {
                isGrowing = false;
                gameObject.tag = "Kelp";
                kelp.canBeEaten = true;
            }
        }
    }
    
    private void OnDestroy()
    {
        manager.allKelp.Remove(this.gameObject);
    }
}
