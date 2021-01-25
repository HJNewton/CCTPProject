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
        }
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
    }
}
