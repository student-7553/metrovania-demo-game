using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCharger : BaseEnemy
{
    public override void StartAfter()
    {
        isGroundBased = false;
    }
    void FixedUpdate()
    {
        
    }
}
