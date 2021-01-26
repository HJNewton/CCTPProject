using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimulationStateManager : MonoBehaviour
{
    public enum CurrentSimState
    {
        PreSim,
        InSim,
    }

    [Header("Current State")]
    public CurrentSimState currentState;

    [Header("Canvases")]
    public GameObject preSimCanvas;
    public GameObject inSimCanvas;

    [Header("Simulation Elements")]
    public EnvironmentManager environmentManager;
    public FishGroupManager fishManager;
    public SharkGroupManager sharkManager;
    public DayNightCycle timeManager;
    public MoveTarget fishTarget;
    public PlayerController playerController;

    [Header("Environment Manager UI Elements")]
    public Slider coralSlider;
    public TextMeshProUGUI coralCountText;
    public Slider kelpSlider;
    public TextMeshProUGUI kelpCountText;

    [Header("Fish Manager UI Elements")]
    public Slider fishCountSlider;
    public TextMeshProUGUI fishCountText;
    public Slider fishMinSpeedSlider;
    public TextMeshProUGUI fishMinSpeedText;
    public Slider fishMaxSpeedSlider;
    public TextMeshProUGUI fishMaxSpeedText;
    public Slider fishNeighbourDistanceSlider;
    public TextMeshProUGUI fishNeighbourDistanceText;
    public Slider fishRotationSpeedSlider;
    public TextMeshProUGUI fishRotationSpeedText;
    public Slider fishAwarenessRangeSlider;
    public TextMeshProUGUI fishAwarenessRangeText;

    [Header("Shark Manager UI Elements")]
    public Slider sharkCountSlider;
    public TextMeshProUGUI sharkCountText;
    public Slider sharkMinSpeedSlider;
    public TextMeshProUGUI sharkMinSpeedText;
    public Slider sharkMaxSpeedSlider;
    public TextMeshProUGUI sharkMaxSpeedText;
    public Slider sharkRotationSpeedSlider;
    public TextMeshProUGUI sharkRotationSpeedText;
    public Slider sharkEatRadiusSlider;
    public TextMeshProUGUI sharkEatRadiusText;

    private void Awake()
    {
        currentState = CurrentSimState.PreSim; // Sets the initial state of the sim to be pre-sim.

        //      Sets all of the required managers to inactive
        //      on the simulation start so as to avoid them
        //      carrying out behaviour before values are set.
        environmentManager.gameObject.SetActive(false);
        fishManager.gameObject.SetActive(false);
        sharkManager.gameObject.SetActive(false);
        timeManager.gameObject.SetActive(false);
        fishTarget.gameObject.SetActive(false);
        playerController.enabled = false; // Only set this script and not object to false as we still need the camera it is attached to
    }

    private void Update()
    {
        if (currentState == CurrentSimState.InSim)
        {
            environmentManager.gameObject.SetActive(true);
            fishManager.gameObject.SetActive(true);
            sharkManager.gameObject.SetActive(true);
            timeManager.gameObject.SetActive(true);
            fishTarget.gameObject.SetActive(true);
            playerController.enabled = true;

            inSimCanvas.SetActive(true);
            preSimCanvas.SetActive(false);
        }

        else if (currentState == CurrentSimState.PreSim)
        {
            inSimCanvas.SetActive(false);
            preSimCanvas.SetActive(true);

            UpdateEnvironmentValues();
            UpdateFishValues();
            UpdateSharkValues();
        }
    }

    public void StartSimulation()
    {
        currentState = CurrentSimState.InSim;
    }

    void UpdateEnvironmentValues()
    {
        environmentManager.coralCount = (int)coralSlider.value; // Set coral count equal to the value of the coral slider casted to an int
        coralCountText.text = environmentManager.coralCount.ToString();

        environmentManager.kelpCount = (int)kelpSlider.value; // Set kelp count equal to the value of the coral slider casted to an int
        kelpCountText.text = environmentManager.kelpCount.ToString();
    }

    void UpdateFishValues()
    {
        fishManager.fishCount = (int)fishCountSlider.value; // Set fish count equal to the value of the fish slider casted to an int
        fishCountText.text = fishManager.fishCount.ToString();

        fishManager.minSpeed = fishMinSpeedSlider.value; // Set fish min speed equal to the value of the fish min speed slider
        fishMinSpeedText.text = fishManager.minSpeed.ToString();

        fishManager.maxSpeed = fishMaxSpeedSlider.value; // Set fish max speed equal to the value of the fish max speed slider
        fishMaxSpeedText.text = fishManager.maxSpeed.ToString();

        fishManager.distanceToNeighbours = fishNeighbourDistanceSlider.value; // Set fish distance to keep from neighbours
        fishNeighbourDistanceText.text = fishManager.distanceToNeighbours.ToString();

        fishManager.rotationSpeed = fishRotationSpeedSlider.value; // Set fish rotation speed
        fishRotationSpeedText.text = fishManager.rotationSpeed.ToString();

        fishManager.awarenessRange = fishAwarenessRangeSlider.value; // Set fish awareness range for sharks in range
        fishAwarenessRangeText.text = fishManager.awarenessRange.ToString();
    }

    void UpdateSharkValues()
    {
        sharkManager.sharkCount = (int)sharkCountSlider.value; // Set shark count equal to the value of the shark slider casted to an int
        sharkCountText.text = sharkManager.sharkCount.ToString();

        sharkManager.minSpeed = sharkMinSpeedSlider.value; // Set shark min speed equal to the value of the shark min speed slider
        sharkMinSpeedText.text = sharkManager.minSpeed.ToString();

        sharkManager.maxSpeed = sharkMaxSpeedSlider.value; // Set shark max speed equal to the value of the shark max speed slider
        sharkMaxSpeedText.text = sharkManager.maxSpeed.ToString();

        sharkManager.rotationSpeed = sharkRotationSpeedSlider.value; // Set shark rotation speed equal to the value of the shark rotation speed slider
        sharkRotationSpeedText.text = sharkManager.rotationSpeed.ToString();

        sharkManager.eatRadius = sharkEatRadiusSlider.value; // Set distance the shark can find a new target to eat
        sharkEatRadiusText.text = sharkManager.eatRadius.ToString();
    }
}
