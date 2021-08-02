using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePlayObjectPhysic : GamePlayObject
{
    //private
    Rigidbody _rigid;

    //public
    public Rigidbody rigid
    {
        get
        {
            if (!_rigid)
            {
                _rigid = GetComponent<Rigidbody>();
            }
            return _rigid;
        }
    }
}

public class BehaviorFloat
{
    protected GamePlayObjectPhysic subject;
    public float surface,foreArchimedes = 20, maxSag = 1, speedFadeForceArchimedes = 10f;
    public bool isBalance = true;
    
    public BehaviorFloat(GamePlayObjectPhysic gamePlayObjectPhysic)
    {
        subject = gamePlayObjectPhysic;
    }

    public void Float()
    {
        if(coroutineFloat == null)
        {
            coroutineFloat = subject.StartCoroutine(CoroutineFloat());
        }
        if (coroutineSink != null)
        {
            subject.StopCoroutine(coroutineSink);
        }
    }

    public void Sink()
    {
        if(coroutineSink == null)
        {
            coroutineSink = subject.StartCoroutine(CoroutineSink());
        }
        if (coroutineFloat != null)
        {
            subject.StopCoroutine(coroutineFloat);
        }
    }

    Coroutine coroutineFloat;
    IEnumerator CoroutineFloat()
    {
        while (true)
        {
            subject.rigid.AddForce(0, foreArchimedes * Mathf.Clamp01((surface - subject.rigid.position.y) / maxSag), 0);
            if (isBalance)
            {
                subject.rigid.AddTorque(subject.rigid.rotation.x * -100f, 0, subject.rigid.rotation.z * -100f);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    Coroutine coroutineSink;
    IEnumerator CoroutineSink()
    {
        float foreArchimedes = this.foreArchimedes;
        while (foreArchimedes > 0)
        {
            subject.rigid.AddForce(0, foreArchimedes * Mathf.Clamp01((surface - subject.rigid.position.y) / maxSag), 0);
            foreArchimedes -= speedFadeForceArchimedes;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(10);
        GameObject.Destroy(subject.gameObject);
    }
}
