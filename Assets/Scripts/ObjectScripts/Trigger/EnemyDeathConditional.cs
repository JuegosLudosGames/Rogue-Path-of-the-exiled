using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathConditional : Trigger, TriggerableI {

    public bool isActive;

    public bool trackDeaths;

    public GameObject[] enemiesToTrackDeath;
    public int NumberKills;
    int currentKills;

    public void onTrigger()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update () {
        if (isActive)
        {
            if (trackDeaths)
            {
                foreach (GameObject objects in enemiesToTrackDeath)
                {
                    if (objects != null)
                    {
                        return;
                    }
                    trigger();
                    Destroy(gameObject);
                }
            }
            else
            {
                if (currentKills >= NumberKills)
                {
                    trigger();
                    Destroy(gameObject);
                }
            }
        }
	}

    public void addKill(int amount) {
        if (isActive)
            currentKills++;
    }

}
