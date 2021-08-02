using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class GamePlayShipCannon : GamePlayShip
{

    public override void NetworkStart()
    {
        Active();
    }

    public new void Active()
    {
        SetFloat();
        if (IsOwner)
        {
            SetBehavior(new BehaviorShipBasic(this));
            SetCameraFollowThis();
        }
        behaviorFloat.foreArchimedes = 100;
        base.Active();
    }
}
