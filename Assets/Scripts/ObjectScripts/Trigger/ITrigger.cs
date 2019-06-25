using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITrigger : Triggerable {

    public GameObject[] objectsWithTriggableI;

    public override void onTrigger()
    {
        foreach (GameObject objects in objectsWithTriggableI) {
            objects.GetComponent<TriggerableI>().onTrigger();
        }
    }
}
