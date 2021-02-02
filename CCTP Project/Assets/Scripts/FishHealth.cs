using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHealth : MonoBehaviour
{
    [Header("Fish Health Stuff")]
    public float initialFood = 200;
    public float currentFoodAmount;
    public float hungerRate; // Hunger rate determines how much food is removed per second
    public float currentAge;
    public float ageToDie;
    public float ageRate;

    bool setReproduction;

    FishBehaviour fishBehaviour;

    private void Awake()
    {
        fishBehaviour = GetComponent<FishBehaviour>();

        currentFoodAmount = initialFood;

        currentAge = 0;
        ageToDie = Random.Range(375, 425);
    }

    void Update()
    {
        RemoveFood();
        AgeFish();
    }

    public void ModifyFood(float foodAmount)
    {
        currentFoodAmount += foodAmount;
    }

    void RemoveFood()
    {
        currentFoodAmount -= hungerRate * Time.deltaTime;

        if (currentFoodAmount <= 0)
        {
            Destroy(gameObject);
        }
    }

    void AgeFish()
    {
        currentAge += ageRate * Time.deltaTime;

        if (currentAge >= 200 && !setReproduction)
        {
            setReproduction = true; 

            fishBehaviour.canReproduce = true;
        }

        if (currentAge >= ageToDie)
        {
            Destroy(gameObject);
        }
    }
}
