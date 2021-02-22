using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkHealth : MonoBehaviour
{
    [Header("Shark Health Stuff")]
    public bool male;
    public bool female;
    public float initialFood ;
    public float currentFoodAmount;
    public float hungerRate; // Hunger rate determines how much food is removed per second
    public float currentAge;
    public float ageToDieMin;
    public float ageToDieMax;
    [SerializeField] private float ageToDie;
    public float ageRate;
    public int timesReproduced;

    SharkBehaviour sharkBehaviour;

    private void Awake()
    {
        sharkBehaviour = GetComponent<SharkBehaviour>();

        currentFoodAmount = initialFood;

        currentAge = 0;
        ageToDie = Random.Range(ageToDieMin, ageToDieMax);

        if (Random.value < 0.5f)
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
        AgeShark();
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

    // Age related behaviour including reproduction bracket and death
    void AgeShark()
    {
        currentAge += ageRate * Time.deltaTime;

        if (currentAge >= 500 && timesReproduced != 2 && female) // If the shark is of age, hasn't reproduced too many times and is a female
        {
            sharkBehaviour.canReproduce = true;
        }

        if (currentAge >= ageToDie)
        {
            Destroy(gameObject);
        }
    }
}
