using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicScreen : Triggerable {

    public bool showBeforeLevel = false;
    public ComicWindow[] windows;

    bool showedStart = false;
    bool hasStarted = false;
    int currentIndex = -1;

    ButtonListener listener;

    public override void onTrigger()
    {
        hasStarted = true;
        showNext();

        //freezes game
        Time.timeScale = 0f;
    }

    // Use this for initialization
    void Start () {
        closeAll();
        listener = GameObject.FindGameObjectWithTag("GameController").GetComponent<ButtonListener>();
	}
	
	// Update is called once per frame
	void Update () {
        if (showBeforeLevel && !showedStart) {
            onTrigger();
            showedStart = true;
            return;
        }
        if (hasStarted && isTouched()) {
            Debug.Log("Was touched");
            showNext();
            
        }
	}

    void showNext() {
        Debug.Log("going through");
        //checks if null, if so complete
        if (iterate() == null) {
            onComplete();
            return;
        }

        //checks if wants to clear the screen
        if (getCurrent().clearScreen) {
            closeAll();
        }
        //sets active
        getCurrent().setActive(true);
    }

    void closeAll() {
        foreach (ComicWindow win in windows)
        {
            win.setActive(false);
        }
    }

    private ComicWindow iterate() {
        currentIndex++;
        return getCurrent();
    }

    private ComicWindow getCurrent() {
        //checks if in range
        if (!(currentIndex >= windows.Length))
        {
            return windows[currentIndex];
        }

        //otherwise return null and complete
        return null;
    }

    private void onComplete() {
        closeAll();
        Time.timeScale = 1f;
        hasStarted = false;
    }

    private bool isTouched() {
        return listener.wasTapped();
    }

}
