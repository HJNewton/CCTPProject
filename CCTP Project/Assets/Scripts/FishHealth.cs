using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHealth : MonoBehaviour
{
    [Header("Fish Health Stuff")]
    public bool male;
    public bool female;
    public float initialNutrients;
    public float currentNutrientsAmount;
    public float hungerRate; // Hunger rate determines how much food is removed per second
    public float currentAge;
    public float ageToDieMin;
    public float ageToDieMax;
    [SerializeField] private float ageToDie;
    public float ageRate;
    public int timesReproduced;
    public GameObject maleFish, femaleFish;

    FishBehaviour fishBehaviour;

    private void Awake()
    {
        fishBehaviour = GetComponent<FishBehaviour>();

        currentNutrientsAmount = initialNutrients;

        currentAge = 0;
        ageToDie = Random.Range(ageToDieMin, ageToDieMax);

        if(Random.value < 0.5f)
        {
            male = true;
            female = false;
        }

        else
        {
            male = false;
            female = true;
        }

        if (male) 
        {
            maleFish.SetActive(true);
            femaleFish.SetActive(false);
        }

        if (female)
        {
            maleFish.SetActive(false);
            femaleFish.SetActive(true);
        }
    }

    void Update()
    {
        RemoveFood();
        AgeFish();
    }

    public void ModifyFood(float foodAmount)
    {
        currentNutrientsAmount += foodAmount;
    }

    void RemoveFood()
    {
        currentNutrientsAmount -= hungerRate * Time.deltaTime;

        if (currentNutrientsAmount <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Age related behaviour including reproduction bracket and death
    void AgeFish()
    {
        currentAge += ageRate * Time.deltaTime;

        if (currentAge >= 200 && timesReproduced != 2 && female) // If the fish is of age, hasn't reproduced too many times and is a female
        {
            fishBehaviour.canReproduce = true;
        }

        if (currentAge >= ageToDie)
        {
            Destroy(gameObject);
        }
    }
}
