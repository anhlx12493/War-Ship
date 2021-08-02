using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;


public class GamePlayObject : NetworkBehaviour
{
}

public class GameBehavior
{
    protected MonoBehaviour subject;

    public void SetSubject(MonoBehaviour subject)
    {
        this.subject = subject;
    }
}
