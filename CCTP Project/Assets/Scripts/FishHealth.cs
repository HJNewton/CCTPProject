using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHealth : MonoBehaviour
{
    public float currentFoodAmount = 2000;

    void Update()
    {
        RemoveFood();
    }

    public void AddFood(float foodToAdd)
    {
        currentFoodAmount += foodToAdd;
    }

    void RemoveFood()
    {
        currentFoodAmount -= Time.deltaTime;

        if (currentFoodAmount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
