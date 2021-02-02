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
    public GraphWindow graph;
    public GameObject fishGraphCanvas;

    FishGroupManager fishManager;

    private void Awake()
    {
        fishGraphCanvas.SetActive(true);
        fishGraphCanvas.GetComponent<Canvas>().enabled = false;
        fishManager = GameObject.FindGameObjectWithTag("FishManager").GetComponent<FishGroupManager>();
        graph.valueList.Add(fishManager.fishCount);
        graph.ShowGraph(graph.valueList, -1);

        StartCoroutine("UpdateGraph");
    }

    private void Update()
    {
        ChangeTime();
        
        var ts = TimeSpan.FromSeconds(timeOfDay);
        timeText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

        if (Input.GetKeyDown(KeyCode.G))
        {
            fishGraphCanvas.GetComponent<Canvas>().enabled = !fishGraphCanvas.GetComponent<Canvas>().enabled;
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

            graph.valueList.Add(fishManager.allFish.Count);
            graph.ShowGraph(graph.valueList, -1);
        }
    }
}
