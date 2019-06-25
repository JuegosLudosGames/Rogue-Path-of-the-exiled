using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPack : Pickupable {

    //the player controller of the player
    ControllerPlayer playerCon;

    public bool isPrimary = true;

    //the amount the player can be healed
    [Range(0, 10)]
    public int giveAmount;

    //used for intialisation
    override
    protected void onStart()
    {
        playerCon = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllerPlayer>();
    }

    //called once per frame before any updated by Pickable
    override
    protected void onFixedUpdatePre()
    {
        //checks if health is full
        //if so then stop from picking up
        if (playerCon.isAmmoCountFull(isPrimary))
            canPickup = false;
        else
            canPickup = true;
    }

    //called when picked up
    override
    protected void onPickup()
    {
        //heals player
        playerCon.addAmmo(isPrimary, giveAmount);
    }
}
