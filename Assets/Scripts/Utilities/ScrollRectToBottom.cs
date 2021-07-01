using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectToBottom : MonoBehaviour
{
    private ScrollRect scrollRect;


    public void Start()
    {
        scrollRect = this.GetComponent<ScrollRect>();
    }

    public void OnAddWords()
    {
        Canvas.ForceUpdateCanvases();

        this.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
        this.GetComponent<ContentSizeFitter>().SetLayoutVertical();

        scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
        scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();

        scrollRect.verticalNormalizedPosition = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
