using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicWindow : MonoBehaviour {

    public bool clearScreen = false;

    bool Active = false;
    public bool isActive {
        get { return Active; }
    }

    public bool trig;
    private void Update()
    {
        if (trig) {
            setActive(true);
            trig = false;
        }
    }

    public GameObject contents;

    public void setActive(bool state) {
        Active = state;

        //makes active
        contents.SetActive(state);

        if (state)
        {
            FadePanel fade = contents.GetComponent<FadePanel>();
            if (fade != null)
            {
                fade.triggered = true;
            }
        }
    }

}
