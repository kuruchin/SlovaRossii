using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinking : MonoBehaviour
{
    private RawImage background;
    private byte alpha;
    private bool blinkSwitcher;
    private Color color;

    [SerializeField]
    private int intensity = 1;
    private int blinkCounter;

    public void OnEnable()
    {
        background = this.gameObject.GetComponent<RawImage>();
        alpha = 255;
        blinkSwitcher = false;
        blinkCounter = 255 / intensity;
        Transparenting(255);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (blinkCounter == 1)
        {
            blinkCounter = 255 / intensity;
            blinkSwitcher = !blinkSwitcher;
        }
        blinkCounter--;
        Transparenting(alpha);
    }

    private void Transparenting(byte thisAlpha)
    {
        if (blinkSwitcher)
            alpha = (byte)(alpha + intensity);
        else
            alpha = (byte)(alpha - intensity);
        Debug.Log(alpha);
        color = new Color32(0, 0, 0, thisAlpha);
        if (background) background.color = new Color(background.color.r, background.color.g, background.color.b, color.a);
    }
}
