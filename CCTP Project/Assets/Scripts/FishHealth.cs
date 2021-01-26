using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHealth : MonoBehaviour
{
    [Header("Fish Food Stuff")]
    public float currentFoodAmount = 200;
    public float hungerRate; // Hunger rate determines how much food is removed per second

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
        currentFoodAmount -= hungerRate * Time.deltaTime;

        if (currentFoodAmount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
