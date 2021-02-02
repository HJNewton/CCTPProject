﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHealth : MonoBehaviour
{
    [Header("Fish Health Stuff")]
    public bool male;
    public bool female;
    public float initialFood = 200;
    public float currentFoodAmount;
    public float hungerRate; // Hunger rate determines how much food is removed per second
    public float currentAge;
    public float ageToDieMin;
    public float ageToDieMax;
    [SerializeField] private float ageToDie;
    public float ageRate;
    public int timesReproduced;

    FishBehaviour fishBehaviour;

    private void Awake()
    {
        fishBehaviour = GetComponent<FishBehaviour>();

        currentFoodAmount = initialFood;

        currentAge = 0;
        ageToDie = Random.Range(ageToDieMin, ageToDieMax);

        if(Random.value < 0.35f)
        {
            male = true;
            female = false;
        }

        else
        {
            male = false;
            female = true;
        }
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

        if (currentAge >= 200 && timesReproduced != 3 && female) // If the fish is of age, hasn't reproduced too many times and is a female
        {
            fishBehaviour.canReproduce = true;
        }

        if (currentAge >= ageToDie)
        {
            Destroy(gameObject);
        }
    }
}
