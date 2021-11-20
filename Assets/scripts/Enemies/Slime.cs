using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : BaseEnemy, BaseEnemyKnockBackInterface
{

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isAlive)
        {
            return;
        }

        if (!isAbleToMove)
        {
            return;
        }
        this.sentryLocationUpdate();
    }

    public override void StartAfter()
    {
        isGroundBased = true;
    }



}
