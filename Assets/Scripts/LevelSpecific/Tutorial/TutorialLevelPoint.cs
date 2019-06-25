using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelPoint : Trigger {

    //onActivate
    public bool TriggerOnActivate;

    public Triggerable[] overrideTrigger;
    public TutorialLevelController controller;

    //if the point is timed
    public bool isTimed;
    public float timeToWait = 0.0f;

    //if point is waiting for an enemy to die
    public bool isWaitingEnemyDead;
    public GameObject enemy;

	// Use this for initialization
	void Awake() {
        if (TriggerOnActivate)
        {
            triggerObjects = overrideTrigger;
        }
        else {
            triggerObjects = new Triggerable[] { controller };
        }
	}

    void Update()
    {
        if (isWaitingEnemyDead) {
            if (enemy == null) {
                trigger();
            }
        }
    }

    public void onActivate() {
        trigger();
        triggerObjects = new Triggerable[] { controller };
    }

}
