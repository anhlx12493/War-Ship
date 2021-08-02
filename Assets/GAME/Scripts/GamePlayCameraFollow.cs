using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CameraFollow : GameBehavior
{
    static CameraFollow _singleton;


    public static CameraFollow singleton
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

    protected void TryActive(MonoBehaviour subject)
    {
        TrySetSingleton(this);
        SetSubject(subject);
        coroutineFollow = subject.StartCoroutine(CoroutineFollow());
    }

    protected void TrySetSingleton(CameraFollow cameraFollow)
    {
        if (_singleton != null && _singleton.IsValidate())
        {
            throw new System.Exception("CameraFollow have had a singleton. If you want to create new one, let's destroy singleton, check if CameraFollow.singleton != null, call singleton.Destroy()");
        }
        _singleton = cameraFollow;
    }

    public bool IsValidate()
    {
        return subject;
    }

    public void Destroy()
    {
        if(_singleton == this)
        {
            StopAllCoroutine();
            _singleton = null;
            OnDestroy();
        }
    }

    protected abstract void OnDestroy();
    
    void StopAllCoroutine()
    {
        if (subject)
        {
            if (coroutineFollow != null)
            {
                subject.StopCoroutine(coroutineFollow);
                coroutineFollow = null;
            }
        }
    }

    Coroutine coroutineFollow;
    IEnumerator CoroutineFollow()
    {
        while (true)
        {
            UpdateFollow();
            yield return new WaitForFixedUpdate();
        }
    }

    protected abstract void UpdateFollow();

}

public class CameraFollowPosition : CameraFollow
{
    Vector3 localPosition;
    public CameraFollowPosition(MonoBehaviour subject)
    {
        TryActive(subject);
        localPosition = Camera.main.transform.position;
    }

    protected override void OnDestroy()
    {

    }

    protected override void UpdateFollow()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, subject.transform.position + localPosition, 0.01f);
    }
}

public class ScreenToPoint : GameBehavior
{
    Vector3 _position;
    public Vector3 position
    {
        get
        {
            return _position;
        }
    }

    public ScreenToPoint(MonoBehaviour subject)
    {
        SetSubject(subject);
        coroutineUpdatePosition = subject.StartCoroutine(CoroutineUpdatePosition());
    }

    public void Destroy()
    {
        if (subject)
        {
            if (coroutineUpdatePosition != null)
            {
                subject.StopCoroutine(coroutineUpdatePosition);
            }
        }
    }

    Coroutine coroutineUpdatePosition;
    IEnumerator CoroutineUpdatePosition()
    {
        while (true)
        {
            yield return null;
        }
    }
}
