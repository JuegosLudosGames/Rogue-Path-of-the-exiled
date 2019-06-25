using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingWall : Triggerable {

    public override void onTrigger()
    {
        disappear();
    }

    void disappear() {
        Destroy(gameObject);
    }

}
