using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTheme : MonoBehaviour
{
    [ColorHtmlProperty]
    public Color[] ThemeColor;

    void Start()
    {

    }

    public void Test()
    {
        ThemeColor = new Color[10];
        ThemeColor[0] = new Color32(239, 127, 66, 255); //f0e4d7
        ThemeColor[1] = new Color32(239, 127, 66, 255);
        ThemeColor[2] = new Color32(39, 60, 236, 255);
        ThemeColor[3] = new Color32(123, 166, 159, 255);
        ThemeColor[4] = new Color32(175, 51, 217, 255);
        ThemeColor[5] = new Color32(234, 124, 125, 255);
        ThemeColor[6] = new Color32(255, 255, 255, 255);
        ThemeColor[7] = new Color32(213, 180, 171, 255);
        ThemeColor[8] = new Color32(247, 207, 44, 255);
        ThemeColor[9] = new Color32(0, 0, 0, 255);
    }
}
