using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelController : Triggerable {

    public GameObject[] LevelPoints;
    int index = 0;
    GameObject currentPoint;
    float timeLeft = 0.0f;
    bool isTiming = false;

    public override void onTrigger()
    {
        nextPoint();
    }

    // Use this for initialization
    void Start () {
        foreach (GameObject objects in LevelPoints) {
            objects.SetActive(false);
        }

        //activates first point
        currentPoint = LevelPoints[0];
        currentPoint.SetActive(true);

        //get levelpoint
        TutorialLevelPoint point = currentPoint.GetComponent<TutorialLevelPoint>();

        //activates if point can be triggered
        if (point.TriggerOnActivate)
            point.onActivate();

        //checks if timed
        if (point.isTimed)
        {
            isTiming = true;
            timeLeft = point.timeToWait;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isTiming)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0) {
                isTiming = false;
                nextPoint();
            }
        }
	}

    void nextPoint() {

        //checks if has next point
        if (!hasNext()) {
            GameObject.FindGameObjectWithTag("LevelEnd").GetComponent<LevelEnd>().onTrigger();
        }

        //deactivates old gameobject
        currentPoint.SetActive(false);

        //gets gameobject
        index++;
        currentPoint = LevelPoints[index];

        //activates
        currentPoint.SetActive(true);

        //get levelpoint
        TutorialLevelPoint point = currentPoint.GetComponent<TutorialLevelPoint>();

        //activates if point can be triggered
        if (point.TriggerOnActivate)
        {
            point.onActivate();
        }

        //checks if timed
        if (point.isTimed) {
            isTiming = true;
            timeLeft = point.timeToWait;
        }

    }

    bool hasNext() {
        if ((index + 1) >= LevelPoints.Length)
            return false;
        return true;
    }

}
