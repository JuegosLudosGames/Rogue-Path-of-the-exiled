using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : Trigger {

    private Image toFade;

    //time to fade
    private float StartingTime;
    public float time = 0f;
    public float maxAlpha = 0;

    public bool triggered = false;

    private bool isFading = false;

	// Use this for initialization
	void Start () {
        toFade = GetComponent<Image>();
        maxAlpha /= 1000;
        Reset();
	}

    // Update is called once per frame
    private void Update()
    {
        if (triggered) {
            triggered = false;
            isFading = true;
            StartingTime = Time.time;
        }

        onFade();
    }

    void onFade () {
        if (isFading)
        {
            if (toFade.color.a < maxAlpha)
            {
                Color newColor = toFade.color;
                newColor.a = ((Time.time - StartingTime) / time);
                toFade.color = newColor;
                if (toFade.color.a >= maxAlpha)
                {
                    isFading = false;
                    trigger();
                }
            }
        }
	}

    public void Reset()
    {
        toFade.color = new Color(toFade.color.r, toFade.color.g, toFade.color.b, 0);
    }

}
