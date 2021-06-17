using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderIndicator : MonoBehaviour
{
    [SerializeField]
    private Slider thisSlider;

    [SerializeField]
    private Text thisText;

    // Update is called once per frame
    void Update()
    {
        thisText.text = thisSlider.value.ToString();
    }
}
