using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransparentingOnEnable : MonoBehaviour
{
    [SerializeField]
    private RawImage background;
    [SerializeField]
    private TextMeshProUGUI text;

    private bool isEnabled = false;

    private RawImage rawImg = null;
    private byte alpha;
    private bool isPreWaitSwitch;
    private int preWait;

    public void OnEnable()
    {
        isEnabled = true;
        isPreWaitSwitch = true;
        alpha = 255;
        Transparenting(255);

        preWait = 2 * 120;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPreWaitSwitch)
        {
            preWait--;
            if (preWait < 0)
            {
                isPreWaitSwitch = false;
            }
        }
        else
        {
            if(alpha < 1)
            {
                gameObject.SetActive(false);
            }
            else
            {
                Transparenting(alpha);
            }
        }
    }

    private void Transparenting(byte thisAlpha)
    {
        alpha--;
        Color color;
        color = new Color32(0, 0, 0, thisAlpha);
        if (background) background.color = new Color(background.color.r, background.color.g, background.color.b, color.a);
        if (text) text.color = new Color(text.color.r, text.color.g, text.color.b, color.a);
    }
}
