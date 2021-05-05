using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphWindow : MonoBehaviour
{
    // This tutorial helped a great deal: https://www.youtube.com/watch?v=CmU5-v-v1Qo&ab_channel=CodeMonkey

    [Header("Graph Setup")]
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphDotsContainer;
    private List<GameObject> objectsList;
    public List<int> valueList;

    private void Awake()
    {
        valueList = new List<int>();
        objectsList = new List<GameObject>();
        
        graphDotsContainer = transform.Find("Graph Dots Container").GetComponent<RectTransform>();
    }
    

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject circleObject = new GameObject("Circle", typeof(Image));
        circleObject.transform.SetParent(graphDotsContainer, false);
        circleObject.GetComponent<Image>().sprite = circleSprite;

        RectTransform rectTransform = circleObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return circleObject;
    }

    public void ShowGraph(List<int> valueList, int maxVisibleValueAmount = -1)
    {
        if(maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valueList.Count;
        }

        foreach (GameObject obj in objectsList)
        {
            Destroy(obj);
        }

        objectsList.Clear();

        float graphWidth = graphDotsContainer.sizeDelta.x;
        float graphHeight = graphDotsContainer.sizeDelta.y;

        float yMaximum = valueList[0]; // Defines height max of graph
        float yMinimum = valueList[0];
        float xSize = graphWidth / (maxVisibleValueAmount + 1); // Distance between each point on x axis

        GameObject lastCircleGameObject = null;

        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            int value = valueList[i];

            if (value > yMaximum)
            {
                yMaximum = value;
            }

            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;

        if(yDifference <= 0)
        {
            yDifference = 5f;
        }

        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        int xIndex = 0;

        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            objectsList.Add(circleGameObject);

            if (lastCircleGameObject != null)
            {
                GameObject dotConnectionObject = CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
                objectsList.Add(dotConnectionObject);
            }

            lastCircleGameObject = circleGameObject;

            xIndex++;
        }
    }

    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject connectionObject = new GameObject("Dot Connection", typeof(Image));
        connectionObject.transform.SetParent(graphDotsContainer, false);
        connectionObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

        RectTransform rectTransform = connectionObject.GetComponent<RectTransform>();
        Vector2 direction = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        rectTransform.anchoredPosition = dotPositionA + direction * distance * 0.5f; // Placed in middle between A + B
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);

        return connectionObject;
    }
}
