using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ControlPad : GameBehavior
{
    private static ControlPad _singleton;
    public static ControlPad singleton
    {
        get
        {
            if(_singleton != null && !_singleton.IsValidate())
            {
                _singleton = null;
            }
            return _singleton;
        }
    }

    protected void TrySetSingleton(ControlPad controlPad)
    {
        if(_singleton != null && _singleton.IsValidate())
        {
            throw new System.Exception("ControlPad have had a singleton. If you want to create new one, let's destroy singleton, check if ControlPad.singleton != null then call singleton.Destroy()");
        }
        _singleton = controlPad;
    }
     
    public bool IsValidate()
    {
        return subject;
    }

    public void Destroy()
    {
        if (_singleton == this)
        {
            _singleton = null;
        }
        OnDestroy();
    }

    protected abstract void OnDestroy();

    public enum Type { basic};

    protected Type _type;
    public Type type
    {
        get
        {
            return _type;
        }
    }
}

public class ControlPadBasic : ControlPad
{

    //public
    public bool isUp, isDown, isLeft, isRight, isFire;

    public ControlPadBasic(MonoBehaviour subject)
    {
        SetSubject(subject);
        TrySetSingleton(this);
        _type = Type.basic;
        coroutineControl = subject.StartCoroutine(CoroutineControl());
    }

    Coroutine coroutineControl;
    IEnumerator CoroutineControl()
    {
        while (true)
        {
            isUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            isDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            isLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            isRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            isFire = Input.GetMouseButtonDown(0);
            yield return null;
        }
    }

    protected override void OnDestroy()
    {
        StopAllCoroutine();
    }

    void StopAllCoroutine()
    {
        if (subject)
        {
            if (coroutineControl != null)
            {
                subject.StopCoroutine(coroutineControl);
                coroutineControl = null;
            }
        }
    }
}
