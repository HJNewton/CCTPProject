using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    public float timeOfDay;
    public int timeScale = 1;
    public TextMeshProUGUI timeText;
    public GraphWindow fishGraph;
    public GameObject fishGraphCanvas;
    public GraphWindow kelpGraph;
    public GameObject kelpGraphCanvas;
    public GraphWindow sharkGraph;
    public GameObject sharkGraphCanvas;
    public GameObject tabBar;


    FishGroupManager fishManager;
    EnvironmentManager environmentManager;
    SharkGroupManager sharkManager;

    private void Awake()
    {
        fishGraphCanvas.SetActive(true);
        fishGraphCanvas.GetComponent<Canvas>().enabled = false;
        fishManager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>();
        fishGraph.valueList.Add(fishManager.fishCount);
        fishGraph.ShowGraph(fishGraph.valueList, -1);

        kelpGraphCanvas.SetActive(true);
        kelpGraphCanvas.GetComponent<Canvas>().enabled = false;
        environmentManager = GameObject.FindGameObjectWithTag("EnvironmentManager").GetComponent<EnvironmentManager>();
        kelpGraph.valueList.Add(environmentManager.kelpCount);
        kelpGraph.ShowGraph(kelpGraph.valueList, -1);

        sharkGraphCanvas.SetActive(true);
        sharkGraphCanvas.GetComponent<Canvas>().enabled = false;
        sharkManager = GameObject.FindGameObjectWithTag("SharkManager").GetComponent<SharkGroupManager>();
        sharkGraph.valueList.Add(sharkManager.sharkCount);
        sharkGraph.ShowGraph(sharkGraph.valueList, -1);

        StartCoroutine("UpdateGraph");
    }

    private void Update()
    {
        ChangeTime();
        
        var ts = TimeSpan.FromSeconds(timeOfDay);
        timeText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

        if (Input.GetKeyDown(KeyCode.G))
        {
            tabBar.SetActive(!tabBar.activeSelf);
            fishGraphCanvas.GetComponent<Canvas>().enabled = false;
            kelpGraphCanvas.GetComponent<Canvas>().enabled = false;
            sharkGraphCanvas.GetComponent<Canvas>().enabled = false;
        }
    }

    void ChangeTime()
    {
        timeOfDay += Time.deltaTime * timeScale;
        timeOfDay %= 1440; // Clamp between 0-24
    }

    IEnumerator UpdateGraph()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);

            fishGraph.valueList.Add(FishGroupManager.instance.allFish.Count);
            fishGraph.ShowGraph(fishGraph.valueList, -1);

            kelpGraph.valueList.Add(EnvironmentManager.instance.allKelp.Count);
            kelpGraph.ShowGraph(kelpGraph.valueList, -1);

            sharkGraph.valueList.Add(SharkGroupManager.instance.allSharks.Count);
            sharkGraph.ShowGraph(sharkGraph.valueList, -1);
        }
    }
}
